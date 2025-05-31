#region Using Directives
using QuickFnMapper.Core.Models;
using QuickFnMapper.Core.Services;
using QuickFnMapper.WinForms.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics; // For Debug
using System.Windows.Forms;
using QuickFnMapper.WinForms.Utils; // 
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
        private readonly ISettingsView _view; // 
        private readonly IAppSettingsService _appSettingsService; // 
        private AppSettings? _currentSettings;
        private readonly MainController _mainController; // 
        #endregion

        #region Constructors
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="SettingsController"/> class.</para>
        /// <para>Khởi tạo một đối tượng mới của lớp <see cref="SettingsController"/>.</para>
        /// </summary>
        public SettingsController(ISettingsView view, IAppSettingsService appSettingsService, MainController mainController)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view)); // 
            _appSettingsService = appSettingsService ?? throw new ArgumentNullException(nameof(appSettingsService)); // 
            _mainController = mainController ?? throw new ArgumentNullException(nameof(mainController)); // 

            // KHÔNG ĐĂNG KÝ ViewInitialized ở đây nữa vì Controller sẽ chủ động populate View
            // _view.ViewInitialized += OnViewInitializedByView; // 

            // Subscribe to view events
            _view.SaveSettingsClicked += OnSaveSettingsClicked; // 
            _view.CancelSettingsClicked += OnCancelSettingsClicked; // 
            _view.BrowseRulesFilePathClicked += OnBrowseRulesFilePathClicked; // 
            Debug.WriteLine("[INFO] SettingsController: Constructor finished, events subscribed.");
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// <para>Loads settings from the service and populates the view with this data.</para>
        /// <para>Tải cài đặt từ dịch vụ và điền dữ liệu này vào view.</para>
        /// </summary>
        public void LoadAndDisplaySettingsOnView()
        {
            Debug.WriteLine("[INFO] SettingsController.LoadAndDisplaySettingsOnView: Entered method.");
            if (_view == null)
            {
                Debug.WriteLine("[ERROR] SettingsController.LoadAndDisplaySettingsOnView: _view is NULL!");
                return;
            }
            if (_appSettingsService == null)
            {
                Debug.WriteLine("[ERROR] SettingsController.LoadAndDisplaySettingsOnView: _appSettingsService is NULL!");
                return;
            }

            try
            {
                _currentSettings = _appSettingsService.LoadSettings(); // 
                if (_currentSettings == null)
                {
                    Debug.WriteLine("[WARN] SettingsController.LoadAndDisplaySettingsOnView: _currentSettings is null after LoadSettings. Using default settings.");
                    _currentSettings = _appSettingsService.GetDefaultSettings(); // 
                    if (_currentSettings == null)
                    {
                        Debug.WriteLine("[FATAL ERROR] SettingsController.LoadAndDisplaySettingsOnView: Failed to get default settings!");
                        _view.ShowUserNotification("Failed to load default settings.", "Critical Error", MessageBoxIcon.Error);
                        return;
                    }
                }
                Debug.WriteLine("[INFO] SettingsController.LoadAndDisplaySettingsOnView: Settings loaded. Calling PopulateViewWithSettingsData.");
                PopulateViewWithSettingsData();
                Debug.WriteLine("[INFO] SettingsController.LoadAndDisplaySettingsOnView: PopulateViewWithSettingsData finished. Populating themes.");
                var themes = new List<string> { "SystemDefault", "Light", "Dark" }; // 
                _view.PopulateApplicationThemes(themes); // 
                Debug.WriteLine("[INFO] SettingsController.LoadAndDisplaySettingsOnView: Themes populated and method finished.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[FATAL EXCEPTION] SettingsController.LoadAndDisplaySettingsOnView: {ex.ToString()}");
                _view.ShowUserNotification($"Error loading settings data: {ex.Message}", "Load Settings Error", MessageBoxIcon.Error); // 
            }
        }
        #endregion

        #region Event Handlers from View
        private void OnSaveSettingsClicked(object? sender, EventArgs e) // 
        {
            Debug.WriteLine("[INFO] SettingsController.OnSaveSettingsClicked: Entered.");
            if (_currentSettings == null)
            {
                _view.ShowUserNotification("Cannot save settings: current settings data is missing.", "Internal Error", MessageBoxIcon.Error); // 
                return;
            }

            if (!ValidateSettingsInput()) // 
            {
                Debug.WriteLine("[WARN] SettingsController.OnSaveSettingsClicked: Validation failed.");
                return;
            }

            bool previousStartWithWindows = _currentSettings.StartWithWindows; // 

            // Lấy giá trị từ View
            _currentSettings.StartWithWindows = _view.StartWithWindows; // 
            _currentSettings.IsGlobalHookEnabled = _view.IsGlobalHookEnabledView; // 
            _currentSettings.MinimizeToTrayOnClose = _view.MinimizeToTrayOnClose; // 
            _currentSettings.ShowNotifications = _view.ShowNotifications; // 
            _currentSettings.RulesFilePath = _view.RulesFilePathView; // 
            _currentSettings.ApplicationTheme = _view.ApplicationThemeView; // 
            Debug.WriteLine("[INFO] SettingsController.OnSaveSettingsClicked: Settings collected from view.");

            try
            {
                _appSettingsService.SaveSettings(_currentSettings); // 
                Debug.WriteLine("[INFO] SettingsController.OnSaveSettingsClicked: Settings saved via AppSettingsService.");

                if (_currentSettings.StartWithWindows != previousStartWithWindows) // 
                {
                    Debug.WriteLine($"[INFO] SettingsController.OnSaveSettingsClicked: StartWithWindows changed from {previousStartWithWindows} to {_currentSettings.StartWithWindows}. Updating registry.");
                    try
                    {
                        StartupRegistryHelper.SetStartup(_currentSettings.StartWithWindows); // 
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[ERROR] SettingsController.OnSaveSettingsClicked: Failed to update 'Start with Windows' setting in registry. {ex.Message}");
                        _view.ShowUserNotification($"Failed to update 'Start with Windows' setting: {ex.Message}", "Startup Setting Error", MessageBoxIcon.Error); // 
                        _currentSettings.StartWithWindows = previousStartWithWindows; // 
                        _view.StartWithWindows = previousStartWithWindows; // Revert UI
                        _appSettingsService.SaveSettings(_currentSettings); // Save reverted setting
                    }
                }
                _mainController.SettingsEditorSaved(); // 
                // _view.ShowUserNotification("Settings saved successfully.", "Settings Saved", MessageBoxIcon.Information); // lặp thông báo
                _view.CloseView(true); // 
                Debug.WriteLine("[INFO] SettingsController.OnSaveSettingsClicked: Save process completed.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[FATAL EXCEPTION] SettingsController.OnSaveSettingsClicked: {ex.ToString()}");
                _view.ShowUserNotification($"An unexpected error occurred while saving settings: {ex.Message}", "Save Error", MessageBoxIcon.Error); // 
            }
        }

        private void OnCancelSettingsClicked(object? sender, EventArgs e) // 
        {
            Debug.WriteLine("[INFO] SettingsController.OnCancelSettingsClicked: Entered. Closing view.");
            _view.CloseView(false); // 
        }

        private void OnBrowseRulesFilePathClicked(object? sender, EventArgs e) // 
        {
            Debug.WriteLine("[INFO] SettingsController.OnBrowseRulesFilePathClicked: Entered.");
            try
            {
                using (var sfd = new SaveFileDialog()) // 
                {
                    sfd.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*"; // 
                    sfd.Title = "Select Key Mappings File Path"; // 

                    string currentPath = _view.RulesFilePathView; // 
                    if (!string.IsNullOrWhiteSpace(currentPath))
                    {
                        try
                        {
                            sfd.FileName = System.IO.Path.GetFileName(currentPath); // 
                            sfd.InitialDirectory = System.IO.Path.GetDirectoryName(currentPath); // 
                        }
                        catch (ArgumentException argEx) // 
                        {
                            Debug.WriteLine($"[WARN] SettingsController.OnBrowseRulesFilePathClicked: Invalid current path '{currentPath}'. {argEx.Message}");
                        }
                    }

                    Form? parentForm = (_view as Control)?.FindForm(); // 
                    DialogResult result = (parentForm != null) ? sfd.ShowDialog(parentForm) : sfd.ShowDialog(); // 

                    if (result == DialogResult.OK)
                    {
                        Debug.WriteLine($"[INFO] SettingsController.OnBrowseRulesFilePathClicked: New rules file path selected: '{sfd.FileName}'");
                        _view.RulesFilePathView = sfd.FileName; // 
                    }
                    else
                    {
                        Debug.WriteLine("[INFO] SettingsController.OnBrowseRulesFilePathClicked: Browse dialog cancelled.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] SettingsController.OnBrowseRulesFilePathClicked: Exception occurred. {ex.ToString()}");
                _view.ShowUserNotification($"Error Browse for file: {ex.Message}", "Browse Error", MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Private Helper Methods
        private void PopulateViewWithSettingsData()
        {
            Debug.WriteLine("[INFO] SettingsController.PopulateViewWithSettingsData: Entered method.");
            if (_currentSettings == null) { Debug.WriteLine("[ERROR] SettingsController.PopulateViewWithSettingsData: _currentSettings is NULL."); return; }
            if (_view == null) { Debug.WriteLine("[ERROR] SettingsController.PopulateViewWithSettingsData: _view is NULL."); return; }

            Debug.WriteLine($"[INFO] SettingsController.PopulateViewWithSettingsData: Applying StartWithWindows = {_currentSettings.StartWithWindows}");
            _view.StartWithWindows = _currentSettings.StartWithWindows; // 

            Debug.WriteLine($"[INFO] SettingsController.PopulateViewWithSettingsData: Applying IsGlobalHookEnabledView = {_currentSettings.IsGlobalHookEnabled}");
            _view.IsGlobalHookEnabledView = _currentSettings.IsGlobalHookEnabled; // 

            Debug.WriteLine($"[INFO] SettingsController.PopulateViewWithSettingsData: Applying MinimizeToTrayOnClose = {_currentSettings.MinimizeToTrayOnClose}");
            _view.MinimizeToTrayOnClose = _currentSettings.MinimizeToTrayOnClose; // 

            Debug.WriteLine($"[INFO] SettingsController.PopulateViewWithSettingsData: Applying ShowNotifications = {_currentSettings.ShowNotifications}");
            _view.ShowNotifications = _currentSettings.ShowNotifications; // 

            Debug.WriteLine($"[INFO] SettingsController.PopulateViewWithSettingsData: Applying RulesFilePathView = '{_currentSettings.RulesFilePath}'");
            _view.RulesFilePathView = _currentSettings.RulesFilePath; // 

            Debug.WriteLine($"[INFO] SettingsController.PopulateViewWithSettingsData: Applying ApplicationThemeView = '{_currentSettings.ApplicationTheme}'");
            _view.ApplicationThemeView = _currentSettings.ApplicationTheme; // 
            Debug.WriteLine("[INFO] SettingsController.PopulateViewWithSettingsData: Finished applying settings to view properties.");
        }

        private bool ValidateSettingsInput()
        {
            Debug.WriteLine("[INFO] SettingsController.ValidateSettingsInput: Entered.");
            if (string.IsNullOrWhiteSpace(_view.RulesFilePathView)) // 
            {
                _view.ShowUserNotification("Rules file path cannot be empty.", "Validation Error", MessageBoxIcon.Warning); // 
                Debug.WriteLine("[WARN] SettingsController.ValidateSettingsInput: RulesFilePathView is empty.");
                return false;
            }
            try
            {
                // Kiểm tra xem đường dẫn có hợp lệ không (không nhất thiết file phải tồn tại ngay lúc này)
                var fi = new System.IO.FileInfo(_view.RulesFilePathView); // 
                Debug.WriteLine($"[INFO] SettingsController.ValidateSettingsInput: RulesFilePathView ('{_view.RulesFilePathView}') seems valid.");
            }
            catch (ArgumentException ex) // 
            {
                _view.ShowUserNotification($"Invalid rules file path (ArgumentException): {ex.Message}", "Validation Error", MessageBoxIcon.Warning); // 
                Debug.WriteLine($"[WARN] SettingsController.ValidateSettingsInput: Invalid RulesFilePathView (ArgumentException). {ex.Message}");
                return false;
            }
            catch (System.IO.PathTooLongException ex) // 
            {
                _view.ShowUserNotification($"Invalid rules file path (PathTooLongException): {ex.Message}", "Validation Error", MessageBoxIcon.Warning); // 
                Debug.WriteLine($"[WARN] SettingsController.ValidateSettingsInput: Invalid RulesFilePathView (PathTooLongException). {ex.Message}");
                return false;
            }
            catch (NotSupportedException ex) // 
            {
                _view.ShowUserNotification($"Invalid rules file path (NotSupportedException): {ex.Message}", "Validation Error", MessageBoxIcon.Warning); // 
                Debug.WriteLine($"[WARN] SettingsController.ValidateSettingsInput: Invalid RulesFilePathView (NotSupportedException). {ex.Message}");
                return false;
            }
            Debug.WriteLine("[INFO] SettingsController.ValidateSettingsInput: Validation successful.");
            return true; // 
        }
        #endregion
    }
}