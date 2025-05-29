#region Using Directives
using QuickFnMapper.Core.Models;
using QuickFnMapper.Core.Services;
using QuickFnMapper.WinForms.Views.Interfaces;
using System;
using System.Collections.Generic; // For IEnumerable
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel; // For CancelEventArgs
using System.Diagnostics; // For Debug
#endregion

namespace QuickFnMapper.WinForms.Controllers
{
    /// <summary>
    /// <para>Controller for the main application window (<see cref="IMainView"/>).</para>
    /// <para>Manages the overall application state, user interactions from the main view, and coordinates with services.</para>
    /// <para>Controller cho cửa sổ ứng dụng chính (<see cref="IMainView"/>).</para>
    /// <para>Quản lý trạng thái tổng thể của ứng dụng, các tương tác người dùng từ view chính, và phối hợp với các dịch vụ.</para>
    /// </summary>
    public class MainController
    {
        #region Fields
        private readonly IMainView _view;
        private readonly IKeyMappingService _keyMappingService;
        private readonly IAppSettingsService _appSettingsService;
        #endregion

        #region Constructors
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="MainController"/> class.</para>
        /// <para>Khởi tạo một đối tượng mới của lớp <see cref="MainController"/>.</para>
        /// </summary>
        public MainController(IMainView view, IKeyMappingService keyMappingService, IAppSettingsService appSettingsService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _keyMappingService = keyMappingService ?? throw new ArgumentNullException(nameof(keyMappingService));
            _appSettingsService = appSettingsService ?? throw new ArgumentNullException(nameof(appSettingsService));

            // Subscribe to view events
            _view.ViewLoaded += OnViewLoaded; // View sẽ tự kích hoạt event này khi nó load xong
            _view.ToggleServiceStatusClicked += OnToggleServiceStatusClicked;
            _view.AddRuleClicked += OnAddRuleClicked;
            _view.EditRuleClicked += OnEditRuleClicked;
            _view.DeleteRuleClicked += OnDeleteRuleClicked;
            _view.OpenSettingsClicked += OnOpenSettingsClicked;
            _view.ViewClosing += OnViewClosing;
        }
        #endregion

        #region Event Handlers from View

        // Called when the MainView (MainForm) raises its ViewLoaded event
        private void OnViewLoaded(object? sender, EventArgs e) // Sửa: object? sender
        {
            try
            {
                Debug.WriteLine("[INFO] MainController: ViewLoaded event received. Initializing...");
                _keyMappingService.LoadRules();
                LoadRulesIntoView();

                AppSettings settings = _appSettingsService.LoadSettings(); // LoadSettings đảm bảo non-null
                _view.IsServiceEnabledUI = settings.IsGlobalHookEnabled;

                if (settings.IsGlobalHookEnabled)
                {
                    _keyMappingService.EnableService();
                }
                // else, service is already disabled or will be by default.
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] MainController: Error during application initialization. {ex.Message}");
                _view.ShowUserNotification($"Error initializing application: {ex.Message}", "Initialization Error", MessageBoxIcon.Error);
            }
        }

        private void OnToggleServiceStatusClicked(object? sender, EventArgs e) // Sửa: object? sender
        {
            try
            {
                bool newServiceState;
                if (_keyMappingService.IsServiceEnabled)
                {
                    _keyMappingService.DisableService();
                    newServiceState = false;
                }
                else
                {
                    _keyMappingService.EnableService();
                    newServiceState = true;
                }
                _view.IsServiceEnabledUI = newServiceState;

                AppSettings settings = _appSettingsService.LoadSettings();
                settings.IsGlobalHookEnabled = newServiceState;
                _appSettingsService.SaveSettings(settings);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] MainController: Error toggling service. {ex.Message}");
                _view.ShowUserNotification($"Error toggling service: {ex.Message}", "Service Error", MessageBoxIcon.Error);
                _view.IsServiceEnabledUI = _keyMappingService.IsServiceEnabled; // Đồng bộ lại UI với trạng thái thực tế
            }
        }

        private void OnAddRuleClicked(object? sender, EventArgs e) // Sửa: object? sender
        {
            _view.ShowRuleEditor(null); // Truyền null để báo hiệu là thêm mới
        }

        private void OnEditRuleClicked(object? sender, EventArgs e) // Sửa: object? sender
        {
            Guid? selectedRuleId = _view.SelectedRuleId; // SelectedRuleId là Guid?
            if (selectedRuleId.HasValue)
            {
                KeyMappingRule? ruleToEdit = _keyMappingService.GetRuleById(selectedRuleId.Value); // GetRuleById trả về KeyMappingRule?
                if (ruleToEdit != null)
                {
                    _view.ShowRuleEditor(ruleToEdit);
                }
                else
                {
                    _view.ShowUserNotification("The selected rule could not be found (it may have been deleted).", "Rule Not Found", MessageBoxIcon.Warning);
                    LoadRulesIntoView(); // Làm mới danh sách
                }
            }
            else
            {
                _view.ShowUserNotification("Please select a rule to edit.", "No Rule Selected", MessageBoxIcon.Information);
            }
        }

        private void OnDeleteRuleClicked(object? sender, EventArgs e) // Sửa: object? sender
        {
            Guid? selectedRuleId = _view.SelectedRuleId;
            if (selectedRuleId.HasValue)
            {
                // Đại ca có thể muốn thêm hộp thoại xác nhận ở đây, gọi từ View
                // Ví dụ: if (_view.ShowConfirmation("Are you sure?")) { ... }
                try
                {
                    _keyMappingService.DeleteRule(selectedRuleId.Value);
                    LoadRulesIntoView();
                    _view.ClearRuleSelection();
                    _view.ShowUserNotification("Rule deleted successfully.", "Delete Rule", MessageBoxIcon.Information);
                }
                catch (InvalidOperationException ex)
                {
                    _view.ShowUserNotification(ex.Message, "Delete Error", MessageBoxIcon.Warning);
                    LoadRulesIntoView();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[ERROR] MainController: Error deleting rule. {ex.Message}");
                    _view.ShowUserNotification($"Error deleting rule: {ex.Message}", "Delete Error", MessageBoxIcon.Error);
                }
            }
            else
            {
                _view.ShowUserNotification("Please select a rule to delete.", "No Rule Selected", MessageBoxIcon.Information);
            }
        }

        private void OnOpenSettingsClicked(object? sender, EventArgs e) // Sửa: object? sender
        {
            AppSettings currentSettings = _appSettingsService.LoadSettings(); // LoadSettings đảm bảo non-null
            _view.ShowSettingsEditor(currentSettings);
            // Logic cập nhật sau khi SettingsEditor đóng sẽ được xử lý bởi SettingsEditorSaved()
        }

        private void OnViewClosing(object? sender, CancelEventArgs e) // Sửa: object? sender
        {
            // Ví dụ về xác nhận thoát, View sẽ cần implement một phương thức ShowConfirmation
            // bool confirmExit = _view.ShowConfirmation("Exit QuickFn Mapper?", "Confirm Exit", MessageBoxIcon.Question);
            // if (!confirmExit)
            // {
            //    e.Cancel = true; // Ngăn form đóng
            //    return;
            // }

            try
            {
                Debug.WriteLine("[INFO] MainController: ViewClosing event received. Cleaning up...");
                if (_keyMappingService.IsServiceEnabled)
                {
                    _keyMappingService.DisableService(); // Đảm bảo hook được dừng
                }
                // Cân nhắc lưu AppSettings một lần cuối nếu có thay đổi nào đó chưa được lưu
                // AppSettings settings = _appSettingsService.LoadSettings();
                // if (settings.IsGlobalHookEnabled != _keyMappingService.IsServiceEnabled) // Ví dụ 1 kiểm tra
                // {
                //    settings.IsGlobalHookEnabled = _keyMappingService.IsServiceEnabled;
                //    _appSettingsService.SaveSettings(settings);
                // }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] MainController: Error during application cleanup on exit. {ex.Message}");
                // Không nên ném lỗi ở đây để tránh ứng dụng không thoát được
            }
        }

        #endregion

        #region Public Methods (called by other controllers or after dialogs close)

        /// <summary>
        /// <para>Reloads the rules from the service and updates the view.</para>
        /// <para>Tải lại các quy tắc từ dịch vụ và cập nhật view.</para>
        /// </summary>
        public void RefreshRulesDisplay()
        {
            LoadRulesIntoView();
        }

        /// <summary>
        /// <para>Called by the RuleEditor's controller after a rule has been successfully saved (added/updated).</para>
        /// <para>Được gọi bởi controller của RuleEditor sau khi một quy tắc đã được lưu thành công (thêm/cập nhật).</para>
        /// </summary>
        /// <param name="rule">
        /// <para>The rule that was saved. Expected to be non-null.</para>
        /// <para>Quy tắc đã được lưu. Được mong đợi là không null.</para>
        /// </param>
        public void RuleEditorSaved(KeyMappingRule rule) // rule ở đây là non-null
        {
            // KeyMappingService đã tự động SaveRules() rồi.
            // Chỉ cần làm mới danh sách trên UI.
            LoadRulesIntoView();
            _view.ShowUserNotification($"Rule '{rule.Name}' saved successfully.", "Rule Saved", MessageBoxIcon.Information);
            // Có thể chọn lại rule vừa được sửa/thêm trên UI nếu cần
        }

        /// <summary>
        /// <para>Called by the SettingsEditor's controller after settings have been successfully saved.</para>
        /// <para>Được gọi bởi controller của SettingsEditor sau khi cài đặt đã được lưu thành công.</para>
        /// </summary>
        public void SettingsEditorSaved()
        {
            // AppSettingsService đã lưu rồi.
            // Cập nhật lại trạng thái UI của MainView nếu cần thiết dựa trên settings mới.
            AppSettings newSettings = _appSettingsService.LoadSettings(); // LoadSettings đảm bảo non-null

            bool previousUiHookState = _view.IsServiceEnabledUI;
            _view.IsServiceEnabledUI = newSettings.IsGlobalHookEnabled;

            // Đồng bộ trạng thái của KeyMappingService với cài đặt mới
            if (newSettings.IsGlobalHookEnabled && !previousUiHookState && !_keyMappingService.IsServiceEnabled)
            {
                _keyMappingService.EnableService();
            }
            else if (!newSettings.IsGlobalHookEnabled && previousUiHookState && _keyMappingService.IsServiceEnabled)
            {
                _keyMappingService.DisableService();
            }
            _view.ShowUserNotification("Settings applied successfully.", "Settings Saved", MessageBoxIcon.Information);
            // Cập nhật các UI khác dựa trên settings nếu có
        }
        #endregion

        #region Private Helper Methods

        /// <summary>
        /// <para>Loads rules from the KeyMappingService and tells the view to display them.</para>
        /// <para>Tải quy tắc từ KeyMappingService và yêu cầu view hiển thị chúng.</para>
        /// </summary>
        private void LoadRulesIntoView()
        {
            try
            {
                // GetAllRules() trả về IEnumerable<KeyMappingRule> (non-null)
                IEnumerable<KeyMappingRule> rules = _keyMappingService.GetAllRules()
                                                                  .OrderBy(r => r.Order)
                                                                  .ThenBy(r => r.Name ?? string.Empty); // Xử lý Name có thể null
                _view.DisplayRules(rules);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] MainController: Error loading rules into view. {ex.Message}");
                _view.ShowUserNotification($"Error loading rules into view: {ex.Message}", "View Error", MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}