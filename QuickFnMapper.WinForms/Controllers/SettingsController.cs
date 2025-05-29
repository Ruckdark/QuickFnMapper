#region Using Directives
using QuickFnMapper.Core.Models;
using QuickFnMapper.Core.Services;
using QuickFnMapper.WinForms.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics; // For Debug
#endregion

namespace QuickFnMapper.WinForms.Controllers
{
    /// <summary>
    /// <para>Controller for the <see cref="ISettingsView"/>.</para>
    /// <para>Manages the logic for viewing and modifying application settings.</para>
    /// <para>Controller cho <see cref="ISettingsView"/>.</para>
    /// <para>Quản lý logic để xem và sửa đổi cài đặt ứng dụng.</para>
    /// </summary>
    public class SettingsController
    {
        #region Fields
        private readonly ISettingsView _view;
        private readonly IAppSettingsService _appSettingsService;
        private AppSettings? _currentSettings; // Sửa: Cho phép nullable, sẽ được gán trong PrepareForSettings
        private readonly MainController _mainController;
        #endregion

        #region Constructors
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="SettingsController"/> class.</para>
        /// <para>Khởi tạo một đối tượng mới của lớp <see cref="SettingsController"/>.</para>
        /// </summary>
        public SettingsController(ISettingsView view, IAppSettingsService appSettingsService, MainController mainController)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _appSettingsService = appSettingsService ?? throw new ArgumentNullException(nameof(appSettingsService));
            _mainController = mainController ?? throw new ArgumentNullException(nameof(mainController));

            // Subscribe to view events
            _view.ViewInitialized += OnViewInitializedByView; // Sửa: Đăng ký vào event của View
            _view.SaveSettingsClicked += OnSaveSettingsClicked;
            _view.CancelSettingsClicked += OnCancelSettingsClicked;
            _view.BrowseRulesFilePathClicked += OnBrowseRulesFilePathClicked;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// <para>Prepares the controller with the current application settings.</para>
        /// <para>The actual data loading to the view happens when the view raises the ViewInitialized event.</para>
        /// <para>Chuẩn bị controller với các cài đặt ứng dụng hiện tại.</para>
        /// <para>Việc tải dữ liệu thực tế vào view xảy ra khi view kích hoạt sự kiện ViewInitialized.</para>
        /// </summary>
        public void PrepareForSettings()
        {
            _currentSettings = _appSettingsService.LoadSettings(); // LoadSettings() đảm bảo trả về non-null
            // Logic điền dữ liệu vào View sẽ được thực hiện trong OnViewInitializedByView
            // khi View đã sẵn sàng và kích hoạt event ViewInitialized.
        }
        #endregion

        #region Event Handlers from View

        // This method is called when THE VIEW raises its ViewInitialized event
        private void OnViewInitializedByView(object? sender, EventArgs e) // Sửa: object? sender
        {
            if (_currentSettings == null) // Kiểm tra phòng trường hợp PrepareForSettings chưa được gọi hoặc LoadSettings lỗi
            {
                Debug.WriteLine("[WARN] SettingsController: _currentSettings is null in OnViewInitializedByView. Loading default settings.");
                _currentSettings = _appSettingsService.GetDefaultSettings(); // LoadSettings() đã có fallback về default
            }
            PopulateViewWithSettingsData();

            var themes = new List<string> { "SystemDefault", "Light", "Dark" };
            _view.PopulateApplicationThemes(themes);
        }

        private void OnSaveSettingsClicked(object? sender, EventArgs e) // Sửa: object? sender
        {
            if (_currentSettings == null)
            {
                _view.ShowUserNotification("Cannot save settings: current settings data is missing.", "Internal Error", MessageBoxIcon.Error);
                return;
            }

            if (!ValidateSettingsInput())
            {
                return;
            }

            _currentSettings.StartWithWindows = _view.StartWithWindows;
            _currentSettings.IsGlobalHookEnabled = _view.IsGlobalHookEnabledView;
            _currentSettings.MinimizeToTrayOnClose = _view.MinimizeToTrayOnClose;
            _currentSettings.ShowNotifications = _view.ShowNotifications;
            _currentSettings.RulesFilePath = _view.RulesFilePathView;
            _currentSettings.ApplicationTheme = _view.ApplicationThemeView;

            try
            {
                _appSettingsService.SaveSettings(_currentSettings);
                _mainController.SettingsEditorSaved();
                _view.ShowUserNotification("Settings saved successfully.", "Settings Saved", MessageBoxIcon.Information);
                _view.CloseView(true);
            }
            catch (Exception ex)
            {
                _view.ShowUserNotification($"An unexpected error occurred while saving settings: {ex.Message}", "Save Error", MessageBoxIcon.Error);
            }
        }

        private void OnCancelSettingsClicked(object? sender, EventArgs e) // Sửa: object? sender
        {
            _view.CloseView(false);
        }

        private void OnBrowseRulesFilePathClicked(object? sender, EventArgs e) // Sửa: object? sender
        {
            // Logic để mở SaveFileDialog hoặc OpenFileDialog cho phép người dùng chọn đường dẫn mới
            // Cần đảm bảo rằng View (SettingsControl) sẽ là parent của dialog này, hoặc có cách xử lý focus đúng.
            // Để Controller không phụ thuộc trực tiếp vào System.Windows.Forms.SaveFileDialog,
            // tốt nhất là ISettingsView có một phương thức: string? PromptForRulesFilePath(string initialPath);
            // Controller sẽ gọi phương thức đó của View.
            // Tạm thời giữ logic đơn giản để minh họa:
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
                sfd.Title = "Select Key Mappings File Path";

                // Lấy đường dẫn hiện tại từ view để làm giá trị mặc định cho dialog
                string currentPath = _view.RulesFilePathView;
                if (!string.IsNullOrWhiteSpace(currentPath))
                {
                    try
                    {
                        sfd.FileName = System.IO.Path.GetFileName(currentPath);
                        sfd.InitialDirectory = System.IO.Path.GetDirectoryName(currentPath);
                    }
                    catch (ArgumentException) // Path không hợp lệ
                    {
                        // Bỏ qua, dùng mặc định của dialog
                    }
                }


                // Đây là cách gọi dialog không lý tưởng từ Controller.
                // View nên tự quản lý việc hiển thị dialog.
                Form? parentForm = (_view as Control)?.FindForm(); // Cố gắng tìm form cha
                DialogResult result = (parentForm != null) ? sfd.ShowDialog(parentForm) : sfd.ShowDialog();

                if (result == DialogResult.OK)
                {
                    _view.RulesFilePathView = sfd.FileName;
                }
            }
        }

        #endregion

        #region Private Helper Methods

        private void PopulateViewWithSettingsData()
        {
            if (_currentSettings == null) return; // Kiểm tra null

            _view.StartWithWindows = _currentSettings.StartWithWindows;
            _view.IsGlobalHookEnabledView = _currentSettings.IsGlobalHookEnabled;
            _view.MinimizeToTrayOnClose = _currentSettings.MinimizeToTrayOnClose;
            _view.ShowNotifications = _currentSettings.ShowNotifications;
            // RulesFilePath và ApplicationTheme trong _currentSettings đã được đảm bảo non-null bởi AppSettings constructor
            _view.RulesFilePathView = _currentSettings.RulesFilePath;
            _view.ApplicationThemeView = _currentSettings.ApplicationTheme;
        }

        private bool ValidateSettingsInput()
        {
            if (string.IsNullOrWhiteSpace(_view.RulesFilePathView))
            {
                _view.ShowUserNotification("Rules file path cannot be empty.", "Validation Error", MessageBoxIcon.Warning);
                return false;
            }
            try
            {
                // Kiểm tra xem đường dẫn có hợp lệ không (không nhất thiết file phải tồn tại ngay lúc này)
                // FileInfo sẽ ném lỗi nếu path chứa ký tự không hợp lệ.
                var fi = new System.IO.FileInfo(_view.RulesFilePathView);
            }
            catch (ArgumentException ex) // Path không hợp lệ
            {
                _view.ShowUserNotification($"Invalid rules file path (ArgumentException): {ex.Message}", "Validation Error", MessageBoxIcon.Warning);
                return false;
            }
            catch (PathTooLongException ex)
            {
                _view.ShowUserNotification($"Invalid rules file path (PathTooLongException): {ex.Message}", "Validation Error", MessageBoxIcon.Warning);
                return false;
            }
            catch (NotSupportedException ex) // Path có dạng không được hỗ trợ (VD: UNC mà không có UseShellExecute)
            {
                _view.ShowUserNotification($"Invalid rules file path (NotSupportedException): {ex.Message}", "Validation Error", MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
        #endregion
    }
}