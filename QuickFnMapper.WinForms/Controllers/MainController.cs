#region Using Directives
using QuickFnMapper.Core.Models;
using QuickFnMapper.Core.Services;
using QuickFnMapper.WinForms.Views;
using QuickFnMapper.WinForms.Views.Interfaces;
using QuickFnMapper.WinForms.Utils; // Cho ThemeManager
using QuickFnMapper.WinForms.Themes; // Cho ColorPalette
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing; // Cho Color
using System.Linq;
using System.Windows.Forms;
#endregion

namespace QuickFnMapper.WinForms.Controllers
{
    public class MainController
    {
        #region Fields
        private readonly IMainView _view;
        private readonly IKeyMappingService _keyMappingService;
        private readonly IAppSettingsService _appSettingsService;
        private readonly ThemeManager _themeManager; // << THÊM FIELD NÀY
        private AppSettings _currentAppSettings;

        private RuleEditorController? _ruleEditorController;
        private SettingsController? _settingsController;
        private IRuleEditorView? _activeRuleEditorViewInstance = null; // Lưu instance của editor view đang hoạt động


        // ... các fields khác ...
        #endregion

        // Sửa constructor để nhận ThemeManager
        public MainController(IMainView view,
                            IKeyMappingService keyMappingService,
                            IAppSettingsService appSettingsService,
                            ThemeManager themeManager) // << THÊM THAM SỐ
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _keyMappingService = keyMappingService ?? throw new ArgumentNullException(nameof(keyMappingService));
            _appSettingsService = appSettingsService ?? throw new ArgumentNullException(nameof(appSettingsService));
            _themeManager = themeManager ?? throw new ArgumentNullException(nameof(themeManager)); // << LƯU LẠI

            _currentAppSettings = _appSettingsService.LoadSettings();

            // ... các đăng ký event ...
            _view.ViewLoaded += OnViewLoaded;
            _view.ToggleServiceStatusClicked += OnToggleServiceStatusClicked;
            _view.AddRuleClicked += OnAddRuleClicked;
            _view.EditRuleClicked += OnEditRuleClicked;
            _view.DeleteRuleClicked += OnDeleteRuleClicked;
            _view.OpenSettingsClicked += OnOpenSettingsClicked;
            _view.ViewClosing += OnViewClosing;
            _keyMappingService.NotificationRequested += OnKeyMappingServiceNotificationRequested;
        }

        // Giờ đây Đại ca có thể dùng _themeManager trong các phương thức của MainController
        // Ví dụ:
        public void UpdateHomeViewData(IHomeView homeView)
        {
            if (homeView == null || _themeManager == null) return; // Kiểm tra null
            _currentAppSettings = _appSettingsService.LoadSettings();

            homeView.WelcomeMessage = "Welcome to QuickFn Mapper!";
            bool isServiceRunning = _keyMappingService.IsServiceEnabled;
            homeView.ServiceStatusText = isServiceRunning ? "Service is currently RUNNING" : "Service is currently STOPPED";

            // Sử dụng _themeManager.CurrentPalette ở đây
            bool isDarkTheme = _themeManager.CurrentPalette.Name?.Contains("Dark") ?? false;
            homeView.ServiceStatusColor = isServiceRunning ?
                (isDarkTheme ? Color.LightGreen : Color.DarkGreen) :
                (isDarkTheme ? Color.LightCoral : Color.DarkRed);
        }

        public void SettingsEditorSaved() // Được gọi bởi SettingsController
        {
            _currentAppSettings = _appSettingsService.LoadSettings();
            _view.IsServiceEnabledUI = _currentAppSettings.IsGlobalHookEnabled;

            if (_themeManager != null) // Kiểm tra null cho _themeManager
            {
                _themeManager.CheckAndApplyTheme(); // Áp dụng theme mới
            }

            _view.ShowUserNotification("Settings applied successfully.", "Settings Saved", MessageBoxIcon.Information);
            NavigateToHome(); 
        }


        // ... các phương thức khác của MainController ...
        #region Event Handlers from KeyMappingService
        private void OnKeyMappingServiceNotificationRequested(object? sender, NotificationEventArgs e)
        {
            if (e?.Info != null)
            {
                _view.ShowUserNotification(e.Info.Message, e.Info.Title, ConvertToMessageBoxIcon(e.Info.Type));
            }
        }

        private MessageBoxIcon ConvertToMessageBoxIcon(NotificationType notificationType)
        {
            switch (notificationType)
            {
                case NotificationType.Error: return MessageBoxIcon.Error;
                case NotificationType.Warning: return MessageBoxIcon.Warning;
                case NotificationType.Info:
                case NotificationType.None:
                default: return MessageBoxIcon.Information;
            }
        }
        #endregion

        #region Event Handlers from View
        private void OnViewLoaded(object? sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine("[INFO] MainController: ViewLoaded event received. Initializing...");
                _currentAppSettings = _appSettingsService.LoadSettings();
                _keyMappingService.LoadRules();
                LoadRulesIntoView();

                _view.IsServiceEnabledUI = _currentAppSettings.IsGlobalHookEnabled;
                if (_currentAppSettings.IsGlobalHookEnabled)
                {
                    _keyMappingService.EnableService();
                }
                NavigateToHome();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] MainController: Error during application initialization. {ex.Message}");
                _view.ShowUserNotification($"Error initializing application: {ex.Message}", "Initialization Error", MessageBoxIcon.Error);
            }
        }

        private void OnToggleServiceStatusClicked(object? sender, EventArgs e)
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
                _currentAppSettings.IsGlobalHookEnabled = newServiceState;
                _appSettingsService.SaveSettings(_currentAppSettings);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] MainController: Error toggling service. {ex.Message}");
                _view.ShowUserNotification($"Error toggling service: {ex.Message}", "Service Error", MessageBoxIcon.Error);
                if (_keyMappingService != null)
                {
                    _view.IsServiceEnabledUI = _keyMappingService.IsServiceEnabled;
                }
            }
        }

        private void OnAddRuleClicked(object? sender, EventArgs e)
        {
            _view.ShowRuleEditor(null);
        }

        private void OnEditRuleClicked(object? sender, EventArgs e)
        {
            Guid? selectedRuleId = _view.SelectedRuleId;
            if (selectedRuleId.HasValue)
            {
                KeyMappingRule? ruleToEdit = _keyMappingService.GetRuleById(selectedRuleId.Value);
                if (ruleToEdit != null)
                {
                    _view.ShowRuleEditor(ruleToEdit);
                }
                else
                {
                    _view.ShowUserNotification("The selected rule could not be found (it may have been deleted).", "Rule Not Found", MessageBoxIcon.Warning);
                    LoadRulesIntoView();
                }
            }
            else
            {
                _view.ShowUserNotification("Please select a rule to edit.", "No Rule Selected", MessageBoxIcon.Information);
            }
        }

        private void OnDeleteRuleClicked(object? sender, EventArgs e)
        {
            Guid? selectedRuleId = _view.SelectedRuleId;
            if (selectedRuleId.HasValue)
            {
                try
                {
                    _keyMappingService.DeleteRule(selectedRuleId.Value);
                    LoadRulesIntoView();
                    _view.ClearRuleSelection();
                    _view.ShowUserNotification("Rule deleted successfully.", "Delete Rule", MessageBoxIcon.Information);
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

        private void OnOpenSettingsClicked(object? sender, EventArgs e)
        {
            _currentAppSettings = _appSettingsService.LoadSettings();
            _view.ShowSettingsEditor(_currentAppSettings);
        }

        private void OnViewClosing(object? sender, CancelEventArgs e)
        {
            if (!e.Cancel)
            {
                try
                {
                    Debug.WriteLine("[INFO] MainController: View is actually closing. Performing final cleanup...");
                    if (_keyMappingService.IsServiceEnabled)
                    {
                        _keyMappingService.DisableService();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[ERROR] MainController: Error during application cleanup on exit. {ex.Message}");
                }
            }
        }
        #endregion

        #region Methods called by MainForm (hoặc các Controller khác)

        public void PrepareAndShowRuleEditor(KeyMappingRule? ruleToEdit, IRuleEditorView ruleEditorView)
        {
            if (ruleEditorView == null)
            {
                Debug.WriteLine("[ERROR] MainController.PrepareAndShowRuleEditor: ruleEditorView is null.");
                return;
            }

            // Hủy đăng ký event Cancelled từ view instance CŨ nếu có
            if (_activeRuleEditorViewInstance != null && _activeRuleEditorViewInstance != ruleEditorView)
            {
                _activeRuleEditorViewInstance.EditorCancelled -= OnRuleEditor_EditorCancelled;
            }

            // Hủy đăng ký các event của RuleEditorController CŨ khỏi view (quan trọng!)
            _ruleEditorController?.UnsubscribeViewEvents();


            // Tạo RuleEditorController MỚI
            _ruleEditorController = new RuleEditorController(ruleEditorView, _keyMappingService, this);
            _ruleEditorController.PrepareForEditing(ruleToEdit);

            // Đăng ký vào event Cancelled của view instance MỚI
            _activeRuleEditorViewInstance = ruleEditorView;
            // Đảm bảo không đăng ký trùng lặp nếu PrepareAndShowRuleEditor được gọi nhiều lần với cùng view instance
            _activeRuleEditorViewInstance.EditorCancelled -= OnRuleEditor_EditorCancelled; // Hủy trước cho chắc
            _activeRuleEditorViewInstance.EditorCancelled += OnRuleEditor_EditorCancelled;
        }
        private void OnRuleEditor_EditorCancelled(object? sender, EventArgs e)
        {
            Debug.WriteLine("[INFO] MainController: RuleEditor_EditorCancelled event received.");
            if (_activeRuleEditorViewInstance != null)
            {
                _activeRuleEditorViewInstance.EditorCancelled -= OnRuleEditor_EditorCancelled; // Hủy đăng ký
                _activeRuleEditorViewInstance = null;
            }
            // RuleEditorControl đã tự ẩn qua CloseEditor(). Giờ MainController điều hướng.
            _view.ShowRuleListView(); // Điều hướng về trang quản lý rule
                                      // LoadRulesIntoView(); // Không cần load lại rules vì không có gì thay đổi
        }
        public void PrepareAndShowSettingsEditor(AppSettings currentSettings, ISettingsView settingsView)
        {
            if (settingsView == null)
            {
                Debug.WriteLine("[ERROR] MainController.PrepareAndShowSettingsEditor: settingsView is null.");
                return;
            }
            //  if (_settingsController == null || _settingsController.IsDisposed)
            // {
            _settingsController = new SettingsController(settingsView, _appSettingsService, this);
            // }
            _settingsController.LoadAndDisplaySettingsOnView();
        }

        //public void RuleEditorSaved(KeyMappingRule rule)
        //{
        //    LoadRulesIntoView();
        //    _view.ShowUserNotification($"Rule '{rule.Name}' saved successfully.", "Rule Saved", MessageBoxIcon.Information);
        //    NavigateToRuleManagement(null);
        //}
        public void RuleSuccessfullySaved(KeyMappingRule rule)
        {
            if (rule == null) return;

            // Hủy đăng ký event cancel cho editor view hiện tại vì nó sẽ đóng lại
            if (_activeRuleEditorViewInstance != null)
            {
                _activeRuleEditorViewInstance.EditorCancelled -= OnRuleEditor_EditorCancelled;
                _activeRuleEditorViewInstance = null;
            }

            LoadRulesIntoView();
            _view.ShowUserNotification($"Rule '{rule.Name}' saved successfully.", "Rule Saved", MessageBoxIcon.Information);
            _view.ShowRuleListView();
        }

        public void NavigateToHome()
        {
            _view.ShowHomeControl();
        }

        public void NavigateToRuleManagement(Control? listViewControl)
        {
            // MainForm sẽ xử lý việc hiển thị listViewRules khi phương thức này được gọi
            if (_view is MainForm mainFormInstance)
            {
                mainFormInstance.ShowUserControlInPanel(listViewControl); // Truyền control cụ thể nếu cần
            }
            LoadRulesIntoView(); // Đảm bảo rules được cập nhật
        }
        public void NavigateToRuleManagement()
        {
            _view.ShowRuleListView(); // Yêu cầu MainForm hiển thị view danh sách rule
            LoadRulesIntoView();      // Đảm bảo danh sách rule được cập nhật
        }

        // Được gọi từ MainForm khi HomeControl raise event
        //public void OnHomeManageRulesRequested()
        //{
        //    // MainForm có thể có một control ListView cố định hoặc một UserControl riêng cho quản lý rule
        //    // Ở đây, chúng ta chỉ cần yêu cầu MainForm hiển thị nó.
        //    // MainForm sẽ tự biết control nào cần hiển thị.
        //    if (_view is MainForm mainFormInstance)
        //    {
        //        // Giả sử listViewRules là một control có sẵn trên MainForm và được quản lý trong pnlMainContent
        //        mainFormInstance.ShowUserControlInPanel(mainFormInstance.Controls.Find("listViewRules", true).FirstOrDefault());
        //    }
        //    LoadRulesIntoView(); // Hiển thị danh sách rules
        //}

        // Phương thức OnHomeManageRulesRequested, khi người dùng click "Manage Rules" từ Home
        public void OnHomeManageRulesRequested()
        {
            NavigateToRuleManagement(); // Điều hướng đến view quản lý rule
        }

        #endregion

        #region Private Helper Methods
        private void LoadRulesIntoView()
        {
            try
            {
                IEnumerable<KeyMappingRule> rules = _keyMappingService.GetAllRules()
                                                                    .OrderBy(r => r.Order)
                                                                    .ThenBy(r => r.Name ?? string.Empty);
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