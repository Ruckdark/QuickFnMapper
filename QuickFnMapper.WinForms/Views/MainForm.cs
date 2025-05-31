#region Using Directives
using QuickFnMapper.Core.Models;
using QuickFnMapper.Core.Services;
using QuickFnMapper.WinForms.Controllers; // Cần cho _mainController
using QuickFnMapper.WinForms.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using QuickFnMapper.WinForms.Utils;    // Cho UIMessageHelper và Program.AppThemeManager
using QuickFnMapper.WinForms.Themes;  // Cho ColorPalette và AppThemes
#endregion

namespace QuickFnMapper.WinForms.Views
{
    public partial class MainForm : Form, IMainView
    {
        #region Fields
        private readonly MainController _mainController;
        private HomeControl? _homeControl;
        private RuleEditorControl? _ruleEditorControl;
        private SettingsControl? _settingsControl;

        private readonly IAppSettingsService _appSettingsService;
        private readonly IGlobalHookService _globalHookService;
        private readonly IKeyMappingService _keyMappingService;

        private bool _isExitingApplication = false;

        private readonly ThemeManager _themeManager;
        #endregion

        #region Constructors
        public MainForm(IAppSettingsService appSettingsService,
                        IGlobalHookService globalHookService,
                        IKeyMappingService keyMappingService,
                        ThemeManager themeManager)
        {
            InitializeComponent();

            _appSettingsService = appSettingsService ?? throw new ArgumentNullException(nameof(appSettingsService));
            _globalHookService = globalHookService ?? throw new ArgumentNullException(nameof(globalHookService));
            _keyMappingService = keyMappingService ?? throw new ArgumentNullException(nameof(keyMappingService));
            _themeManager = themeManager ?? throw new ArgumentNullException(nameof(themeManager));

            _mainController = new MainController(this, _keyMappingService, _appSettingsService, _themeManager);

            // Event handlers cho NotifyIcon
            if (this.trayNotifyIcon != null)
            {
                this.trayNotifyIcon.DoubleClick += TrayNotifyIcon_DoubleClick;
            }
            if (this.showToolStripMenuItemTray != null)
            {
                this.showToolStripMenuItemTray.Click += ShowToolStripMenuItemTray_Click;
            }
            if (this.toggleServiceTrayMenuItem != null)
            {
                this.toggleServiceTrayMenuItem.Click += ToggleServiceTrayMenuItem_Click;
            }
            if (this.exitToolStripMenuItemTray != null)
            {
                this.exitToolStripMenuItemTray.Click += ExitToolStripMenuItemTray_Click;
            }

            _themeManager.ThemeChanged -= OnThemeManagerThemeChanged;
            _themeManager.ThemeChanged += OnThemeManagerThemeChanged;
        }

        private void OnThemeManagerThemeChanged(object? sender, EventArgs e)
        {
            ApplyCurrentTheme();
        }

        private void ApplyCurrentTheme()
        {
            if (this.IsHandleCreated && !this.IsDisposed && _themeManager != null)
            {
                ColorPalette currentPalette = _themeManager.CurrentPalette; // Lấy palette trước
                _themeManager.ApplyThemeToFormOrUserControl(this);

                // Set lại BackColor cho listViewRules một cách tường minh
                if (this.listViewRules != null)
                {
                    this.listViewRules.BackColor = currentPalette.ControlBackground; // Sử dụng palette đã lấy
                    Debug.WriteLine($"[MainForm.ApplyCurrentTheme] Explicitly set listViewRules.BackColor to {currentPalette.ControlBackground}");
                    // Có thể không cần set ForeColor ở đây nữa nếu DisplayRules đã xử lý
                }

                // Gọi lại DisplayRules để các item cũng được cập nhật màu (nếu DisplayRules có logic màu item)
                if (_keyMappingService != null)
                {
                    var rules = _keyMappingService.GetAllRules();
                    if (rules != null)
                    {
                        this.DisplayRules(rules.OrderBy(r => r.Order).ThenBy(r => r.Name ?? string.Empty));
                    }
                }
            }
        }
        #endregion

        #region IMainView Implementation
        public bool IsServiceEnabledUI
        {
            get { return tsbToggleService?.Checked ?? false; }
            set
            {
                if (tsbToggleService != null)
                {
                    tsbToggleService.Checked = value;
                    if (value)
                    {
                        tsbToggleService.Image = global::QuickFnMapper.Properties.Resources.ServiceOffIcon;
                        tsbToggleService.ToolTipText = "Disable Service (Currently ON)";
                    }
                    else
                    {
                        tsbToggleService.Image = global::QuickFnMapper.Properties.Resources.ServiceOnIcon;
                        tsbToggleService.ToolTipText = "Enable Service (Currently OFF)";
                    }
                }

                if (toggleServiceToolStripMenuItem != null)
                {
                    toggleServiceToolStripMenuItem.Checked = value;
                    toggleServiceToolStripMenuItem.Text = value ? "&Disable Service" : "&Enable Service";
                }

                if (toggleServiceTrayMenuItem != null)
                {
                    toggleServiceTrayMenuItem.Checked = value;
                    toggleServiceTrayMenuItem.Text = value ? "Disable Service" : "Enable Service";
                }
                if (lblStatusService != null && _themeManager != null) // Thêm kiểm tra _themeManager
                {
                    lblStatusService.Text = value ? "Service: Running" : "Service: Stopped";
                    bool isDarkTheme = _themeManager.CurrentPalette.Name?.Contains("Dark") ?? false;
                    lblStatusService.ForeColor = value ?
                        (isDarkTheme ? Color.LightGreen : Color.DarkGreen) :
                        (isDarkTheme ? Color.LightCoral : Color.DarkRed);
                }

                if (trayNotifyIcon != null)
                {
                    string appName = "QuickFn Mapper";
                    if (value)
                    {
                        trayNotifyIcon.Icon = global::QuickFnMapper.Properties.Resources.AppTrayEnabledIcon;
                        trayNotifyIcon.Text = $"{appName} - Service Enabled";
                    }
                    else
                    {
                        trayNotifyIcon.Icon = global::QuickFnMapper.Properties.Resources.AppTrayDisabledIcon;
                        trayNotifyIcon.Text = $"{appName} - Service Disabled";
                    }
                }
            }
        }
        public Guid? SelectedRuleId
        {
            get
            {
                if (this.listViewRules != null && this.listViewRules.SelectedItems.Count > 0)
                {
                    if (this.listViewRules.SelectedItems[0].Tag is Guid id) { return id; }
                }
                return null;
            }
        }

        public void DisplayRules(IEnumerable<KeyMappingRule> rules)
        {
            if (this.listViewRules == null || _themeManager == null) // Thêm kiểm tra _themeManager
            {
                Debug.WriteLine("[WARNING] MainForm.DisplayRules: listViewRules or _themeManager is null.");
                return;
            }

            this.listViewRules.ItemChecked -= listViewRules_ItemChecked;
            this.listViewRules.BeginUpdate();
            this.listViewRules.Items.Clear();

            ColorPalette themePalette = _themeManager.CurrentPalette;

            foreach (var rule in rules)
            {
                string ruleName = rule.Name ?? "Unnamed Rule";
                string originalKeyStr = rule.OriginalKey?.ToString() ?? "N/A";
                string actionStr = rule.TargetActionDetails?.ToString() ?? "N/A";
                var item = new ListViewItem(new[] {
                    rule.IsEnabled ? "Active" : "Inactive",
                    ruleName,
                    originalKeyStr,
                    actionStr,
                    rule.Order.ToString(),
                    rule.LastModifiedDate.ToLocalTime().ToString("g")
                });
                item.Tag = rule.Id;
                item.Checked = rule.IsEnabled;

                item.BackColor = themePalette.ControlBackground;
                if (rule.IsEnabled)
                {
                    item.ForeColor = themePalette.TextColor;
                }
                else
                {
                    float darkFactor = 0.3f;
                    item.ForeColor = ControlPaint.Dark(themePalette.TextColor, darkFactor);
                    // Sử dụng themePalette.Name để kiểm tra theme
                    if (item.ForeColor == themePalette.ControlBackground || (item.ForeColor == Color.Black && (themePalette.Name?.Contains("Dark") ?? false)))
                    {
                        item.ForeColor = Color.Gray;
                    }
                }
                this.listViewRules.Items.Add(item);
            }
            this.listViewRules.EndUpdate();
            this.listViewRules.ItemChecked += listViewRules_ItemChecked;
        }

        internal void ShowUserControlInPanel(Control? controlToShow)
        {
            if (pnlMainContent == null)
            {
                Debug.WriteLine("[ERROR] MainForm.ShowUserControlInPanel: pnlMainContent is NULL!");
                return;
            }
            if (controlToShow == null)
            {
                Debug.WriteLine("[WARN] MainForm.ShowUserControlInPanel: controlToShow is null. Clearing panel.");
                pnlMainContent.Controls.Clear();
                return;
            }

            Debug.WriteLine($"[INFO] MainForm.ShowUserControlInPanel: Request to show '{controlToShow.Name}'. Current Visible: {controlToShow.Visible}, IsDisposed: {controlToShow.IsDisposed}");

            if (controlToShow.IsDisposed)
            {
                Debug.WriteLine($"[ERROR] MainForm.ShowUserControlInPanel: Control '{controlToShow.Name}' is disposed. Cannot show.");
                return;
            }

            if (pnlMainContent.InvokeRequired)
            {
                pnlMainContent.Invoke(new Action(() => PerformShowUserControl(controlToShow)));
            }
            else
            {
                PerformShowUserControl(controlToShow);
            }
        }

        private void PerformShowUserControl(Control controlToShow)
        {
            if (!pnlMainContent.Controls.Contains(controlToShow))
            {
                pnlMainContent.Controls.Clear();
                controlToShow.Dock = DockStyle.Fill;
                controlToShow.Visible = true;
                pnlMainContent.Controls.Add(controlToShow);
                Debug.WriteLine($"[DEBUG] MainForm.PerformShowUserControl: Added '{controlToShow.Name}' to pnlMainContent. Controls count: {pnlMainContent.Controls.Count}");
            }
            else
            {
                Debug.WriteLine($"[DEBUG] MainForm.PerformShowUserControl: '{controlToShow.Name}' already in pnlMainContent.");
            }

            controlToShow.Visible = true;
            controlToShow.BringToFront();
            controlToShow.Focus();

            Application.DoEvents();
            pnlMainContent.PerformLayout();
            controlToShow.PerformLayout();
            // controlToShow.Refresh(); 

            Debug.WriteLine($"[INFO] MainForm.PerformShowUserControl: '{controlToShow.Name}' brought to front. Final Visible: {controlToShow.Visible}, Size: {controlToShow.Size}, Parent: {controlToShow.Parent?.Name}");
            VerifyControlInPanel(controlToShow);
        }

        private void VerifyControlInPanel(Control controlToVerify)
        {
            if (pnlMainContent.Controls.Contains(controlToVerify) && controlToVerify.Visible)
            {
                Debug.WriteLine($"[VERIFY SUCCESS] MainForm.PerformShowUserControl: '{controlToVerify.Name}' IS in pnlMainContent and IS Visible.");
            }
            else
            {
                Debug.WriteLine($"[VERIFY ERROR] MainForm.PerformShowUserControl: '{controlToVerify.Name}' NOT in pnlMainContent or NOT Visible. InPanel: {pnlMainContent.Controls.Contains(controlToVerify)}, Visible: {controlToVerify.Visible}, Parent: {controlToVerify.Parent?.Name}, IsDisposed: {controlToVerify.IsDisposed}");
            }
        }

        public void ShowRuleEditor(KeyMappingRule? ruleToEdit)
        {
            Debug.WriteLine($"[INFO] MainForm.ShowRuleEditor: Request to show. Rule: '{ruleToEdit?.Name ?? "NEW"}'");
            if (_ruleEditorControl == null || _ruleEditorControl.IsDisposed)
            {
                _ruleEditorControl = new RuleEditorControl();
                Debug.WriteLine("[INFO] MainForm.ShowRuleEditor: New RuleEditorControl instance created.");
            }
            _mainController.PrepareAndShowRuleEditor(ruleToEdit, _ruleEditorControl); // Controller sẽ chuẩn bị View
            ShowUserControlInPanel(_ruleEditorControl); // Sau đó MainForm hiển thị nó
        }

        public void ShowSettingsEditor(AppSettings currentSettings)
        {
            Debug.WriteLine("[INFO] MainForm.ShowSettingsEditor: Request to show.");
            try
            {
                if (_settingsControl == null || _settingsControl.IsDisposed)
                {
                    Debug.WriteLine("[INFO] MainForm.ShowSettingsEditor: Creating new SettingsControl instance.");
                    _settingsControl = new SettingsControl();
                    if (_settingsControl == null)
                    {
                        Debug.WriteLine("[FATAL ERROR] MainForm.ShowSettingsEditor: Failed to create SettingsControl instance!");
                        ShowUserNotification("Cannot open settings: component failed to load.", "Error", MessageBoxIcon.Error);
                        return;
                    }
                    Debug.WriteLine($"[INFO] MainForm.ShowSettingsEditor: New SettingsControl created. Name: '{_settingsControl.Name}', Initial Visible: {_settingsControl.Visible}");
                }
                else
                {
                    Debug.WriteLine($"[INFO] MainForm.ShowSettingsEditor: Reusing existing SettingsControl. Name: '{_settingsControl.Name}', Current Visible: {_settingsControl.Visible}");
                }

                _mainController.PrepareAndShowSettingsEditor(currentSettings, _settingsControl); // Controller chuẩn bị View
                ShowUserControlInPanel(_settingsControl); // MainForm hiển thị
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[FATAL EXCEPTION] MainForm.ShowSettingsEditor: {ex.ToString()}");
                UIMessageHelper.ShowError($"An critical error occurred while trying to show settings: {ex.Message}", "Error Showing Settings", this);
            }
        }

        public void ClearRuleSelection() { if (this.listViewRules != null) this.listViewRules.SelectedItems.Clear(); }

        public void ShowHomeControl()
        {
            Debug.WriteLine("[INFO] MainForm.ShowHomeControl: Request to show.");
            if (_homeControl == null || _homeControl.IsDisposed)
            {
                _homeControl = new HomeControl();
                Debug.WriteLine("[INFO] MainForm.ShowHomeControl: New HomeControl instance created.");
                _homeControl.ManageRulesRequested -= HandleHomeManageRulesRequested;
                _homeControl.ManageRulesRequested += HandleHomeManageRulesRequested;
                _homeControl.ToggleServiceStatusRequested -= HandleHomeToggleServiceRequested;
                _homeControl.ToggleServiceStatusRequested += HandleHomeToggleServiceRequested;
            }
            _mainController.UpdateHomeViewData(_homeControl);
            ShowUserControlInPanel(_homeControl);
        }

        public void ShowUserNotification(string message, string caption, MessageBoxIcon icon)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ActualShowUserNotification(message, caption, icon)));
                return;
            }
            ActualShowUserNotification(message, caption, icon);
        }

        private void ActualShowUserNotification(string message, string caption, MessageBoxIcon icon)
        {
            AppSettings settings = _appSettingsService.LoadSettings();
            if (settings.ShowNotifications)
            {
                if (trayNotifyIcon != null && trayNotifyIcon.Visible && this.WindowState == FormWindowState.Minimized)
                {
                    ToolTipIcon toolTipIcon;
                    switch (icon)
                    {
                        case MessageBoxIcon.Error: toolTipIcon = ToolTipIcon.Error; break;
                        case MessageBoxIcon.Warning: toolTipIcon = ToolTipIcon.Warning; break;
                        case MessageBoxIcon.Information:
                        // case MessageBoxIcon.None: // MessageBoxIcon không có None
                        default: toolTipIcon = ToolTipIcon.Info; break;
                    }
                    trayNotifyIcon.ShowBalloonTip(3000, caption, message, toolTipIcon);
                }
                else
                {
                    MessageBox.Show(this, message, caption, MessageBoxButtons.OK, icon);
                }
            }
            else
            {
                Debug.WriteLine($"[Notification Suppressed (Setting Disabled)] Title: {caption}, Message: {message}");
            }
        }

        public event EventHandler? ViewLoaded;
        public event EventHandler? ToggleServiceStatusClicked;
        public event EventHandler? AddRuleClicked;
        public event EventHandler? EditRuleClicked;
        public event EventHandler? DeleteRuleClicked;
        public event EventHandler? OpenSettingsClicked;
        public event CancelEventHandler? ViewClosing;

        #endregion

        #region Form Event Handlers
        private void MainForm_Load(object? sender, EventArgs e)
        {
            Debug.WriteLine($"[INFO] MainForm '{this.Name}': Load event fired. IsHandleCreated: {this.IsHandleCreated}");
            ApplyCurrentTheme();
            ViewLoaded?.Invoke(this, EventArgs.Empty);
            _mainController.NavigateToHome();
            
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            AppSettings settings = _appSettingsService.LoadSettings();
            if (settings.MinimizeToTrayOnClose && !_isExitingApplication && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                if (trayNotifyIcon != null)
                {
                    trayNotifyIcon.Visible = true;
                }
                Debug.WriteLine("[INFO] MainForm_FormClosing: Minimized to tray.");
            }
            else
            {
                Debug.WriteLine("[INFO] MainForm_FormClosing: Proceeding with application exit.");
                ViewClosing?.Invoke(this, e);

                if (!e.Cancel)
                {
                    if (_themeManager != null)
                    {
                        _themeManager.ThemeChanged -= OnThemeManagerThemeChanged;
                    }

                    if (trayNotifyIcon != null)
                    {
                        trayNotifyIcon.Visible = false;
                        trayNotifyIcon.Dispose();
                    }
                    _homeControl?.Dispose();
                    _ruleEditorControl?.Dispose();
                    _settingsControl?.Dispose();
                    _globalHookService?.Dispose();
                    Debug.WriteLine("[INFO] MainForm_FormClosing: Disposed all resources.");
                }
            }
        }
        #endregion

        #region NotifyIcon and ContextMenu Event Handlers
        private void RestoreApplicationWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            if (trayNotifyIcon != null) trayNotifyIcon.Visible = false; // Ẩn icon tray khi form hiện lại
        }
        private void TrayNotifyIcon_DoubleClick(object? sender, EventArgs e) { RestoreApplicationWindow(); }
        private void ShowToolStripMenuItemTray_Click(object? sender, EventArgs e) { RestoreApplicationWindow(); }
        private void ExitToolStripMenuItemTray_Click(object? sender, EventArgs e)
        {
            _isExitingApplication = true;
            Application.Exit();
        }
        #endregion

        #region UI Control Event Handlers
        private void exitToolStripMenuItem_Click(object? sender, EventArgs e) { ExitToolStripMenuItemTray_Click(sender, e); }
        private void tsbToggleService_Click(object? sender, EventArgs e) { ToggleServiceStatusClicked?.Invoke(this, EventArgs.Empty); }
        private void tsbAddRule_Click(object? sender, EventArgs e) { AddRuleClicked?.Invoke(this, EventArgs.Empty); }
        private void tsbEditRule_Click(object? sender, EventArgs e)
        {
            EditRuleClicked?.Invoke(this, EventArgs.Empty);
        }
        private void tsbDeleteRule_Click(object? sender, EventArgs e)
        {
            if (SelectedRuleId.HasValue)
            {
                if (UIMessageHelper.ShowConfirmation("Are you sure you want to delete the selected rule?", "Confirm Delete", this) == DialogResult.Yes)
                {
                    DeleteRuleClicked?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                ShowUserNotification("Please select a rule to delete.", "No Rule Selected", MessageBoxIcon.Information);
            }
        }
        private void tsbSettings_Click(object? sender, EventArgs e) { OpenSettingsClicked?.Invoke(this, EventArgs.Empty); }
        private void tsbShowHome_Click(object? sender, EventArgs e)
        {
            _mainController.NavigateToHome();
        }

        // Sửa chữ ký của các handler cho HomeControl events
        private void HandleHomeManageRulesRequested(object? sender, EventArgs e)
        {
            // listViewRules được truyền vào vì NavigateToRuleManagement trong MainController có tham số này
            // Nếu MainController không cần, có thể bỏ qua
            _mainController.NavigateToRuleManagement(this.listViewRules);
        }
        private void HandleHomeToggleServiceRequested(object? sender, EventArgs e)
        {
            ToggleServiceStatusClicked?.Invoke(this, EventArgs.Empty);
        }

        private void listViewRules_DoubleClick(object? sender, EventArgs e)
        {
            EditRuleClicked?.Invoke(this, EventArgs.Empty);
        }

        private void listViewRules_ItemChecked(object? sender, ItemCheckedEventArgs e)
        {
            if (e.Item?.Tag is Guid id && _themeManager != null && _keyMappingService != null) // Thêm kiểm tra _keyMappingService
            {
                KeyMappingRule? r = _keyMappingService.GetRuleById(id);
                if (r != null && r.IsEnabled != e.Item.Checked)
                {
                    bool originalState = r.IsEnabled;
                    r.IsEnabled = e.Item.Checked;
                    try
                    {
                        _keyMappingService.UpdateRule(r);
                        ColorPalette themePalette = _themeManager.CurrentPalette;
                        e.Item.SubItems[0].Text = r.IsEnabled ? "Active" : "Inactive";
                        if (r.IsEnabled)
                        {
                            e.Item.ForeColor = themePalette.TextColor;
                        }
                        else
                        {
                            e.Item.ForeColor = ControlPaint.Dark(themePalette.TextColor, 0.3f);
                            // Sửa lỗi "item" không tồn tại
                            if (e.Item.ForeColor == themePalette.ControlBackground || (e.Item.ForeColor == Color.Black && (themePalette.Name?.Contains("Dark") ?? false)))
                            {
                                e.Item.ForeColor = Color.Gray;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[ERROR] MainForm.listViewRules_ItemChecked: Updating rule IsEnabled failed for rule '{r.Name}'. {ex.Message}");
                        ShowUserNotification($"Failed to update rule '{r.Name}': {ex.Message}", "Error", MessageBoxIcon.Error);
                        r.IsEnabled = originalState;
                        e.Item.Checked = originalState;
                    }
                }
            }
        }
        private void ToggleServiceTrayMenuItem_Click(object? sender, EventArgs e)
        {
            ToggleServiceStatusClicked?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}