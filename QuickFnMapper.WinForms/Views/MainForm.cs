#region Using Directives
using QuickFnMapper.Core.Models;
using QuickFnMapper.Core.Services;
using QuickFnMapper.WinForms.Views;
using QuickFnMapper.WinForms.Controllers;
using QuickFnMapper.WinForms.Views; // << THÊM USING NÀY ĐỂ THẤY CÁC USERCONTROL
using QuickFnMapper.WinForms.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        // Các service sẽ được inject từ Program.cs
        private readonly IAppSettingsService _appSettingsService;
        private readonly IGlobalHookService _globalHookService; // MainForm sẽ quản lý việc dispose service này
        private readonly IKeyMappingService _keyMappingService;
        #endregion

        #region Constructors
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="MainForm"/> class.</para>
        /// <para>Khởi tạo một đối tượng mới của lớp <see cref="MainForm"/>.</para>
        /// </summary>
        /// <param name="appSettingsService">The application settings service.</param>
        /// <param name="globalHookService">The global keyboard hook service.</param>
        /// <param name="keyMappingService">The key mapping rule service.</param>
        public MainForm(IAppSettingsService appSettingsService, IGlobalHookService globalHookService, IKeyMappingService keyMappingService)
        {
            InitializeComponent();

            // Gán các service được inject
            _appSettingsService = appSettingsService ?? throw new ArgumentNullException(nameof(appSettingsService));
            _globalHookService = globalHookService ?? throw new ArgumentNullException(nameof(globalHookService));
            _keyMappingService = keyMappingService ?? throw new ArgumentNullException(nameof(keyMappingService));

            // Khởi tạo MainController với các service đã được inject
            _mainController = new MainController(this, _keyMappingService, _appSettingsService);

            this.Load += MainForm_Load;
            // Quan trọng: Đảm bảo FormClosing được đăng ký để MainController có thể xử lý
            // và để MainForm có thể Dispose _globalHookService nếu cần.
            this.FormClosing += MainForm_FormClosing;

            ShowHomeControl();
        }
        #endregion

        #region IMainView Implementation

        public bool IsServiceEnabledUI
        {
            get
            {
                if (tsbToggleService != null)
                {
                    return tsbToggleService.Checked;
                }
                return false;
            }
            set
            {
                if (tsbToggleService != null)
                {
                    tsbToggleService.Checked = value;
                    tsbToggleService.Text = value ? "Disable Service" : "Enable Service";
                    // Cập nhật ToolStripMenuItem tương ứng nếu có
                    if (toggleServiceToolStripMenuItem != null)
                    {
                        toggleServiceToolStripMenuItem.Checked = value;
                        toggleServiceToolStripMenuItem.Text = value ? "Disable Service" : "Enable Service";
                    }
                }
                if (lblStatusService != null)
                {
                    lblStatusService.Text = value ? "Service: Running" : "Service: Stopped";
                    lblStatusService.ForeColor = value ? Color.DarkGreen : Color.DarkRed;
                }
            }
        }

        public Guid? SelectedRuleId
        {
            get
            {
                if (listViewRules.SelectedItems.Count > 0)
                {
                    // Sử dụng 'as' để tránh exception nếu Tag không phải Guid hoặc null
                    if (listViewRules.SelectedItems[0].Tag is Guid id)
                    {
                        return id;
                    }
                }
                return null;
            }
        }

        public void DisplayRules(IEnumerable<KeyMappingRule> rules)
        {
            listViewRules.BeginUpdate();
            listViewRules.Items.Clear();
            foreach (var rule in rules) // rules được đảm bảo non-null từ controller
            {
                string ruleName = rule.Name ?? "Unnamed Rule"; // Xử lý Name có thể null
                string originalKeyStr = rule.OriginalKey?.ToString() ?? "N/A"; // OriginalKey có thể null
                string actionStr = rule.TargetActionDetails?.ToString() ?? "N/A"; // TargetActionDetails có thể null

                var item = new ListViewItem(new[] {
                    rule.IsEnabled ? "Yes" : "No",
                    ruleName,
                    originalKeyStr,
                    actionStr,
                    rule.Order.ToString(),
                    rule.LastModifiedDate.ToLocalTime().ToString("g") // "g" là General short date/time
                });
                item.Tag = rule.Id;
                item.Checked = rule.IsEnabled;
                listViewRules.Items.Add(item);
            }
            listViewRules.EndUpdate();
        }

        private void ShowUserControlInPanel(UserControl? controlToShow) // controlToShow có thể là null
        {
            if (pnlMainContent == null) return; // Kiểm tra pnlMainContent

            pnlMainContent.Controls.Clear();
            if (controlToShow != null)
            {
                controlToShow.Dock = DockStyle.Fill;
                pnlMainContent.Controls.Add(controlToShow);
            }
        }

        public void ShowRuleEditor(KeyMappingRule? ruleToEdit) // ruleToEdit có thể null
        {
            if (_ruleEditorControl == null || _ruleEditorControl.IsDisposed)
            {
                _ruleEditorControl = new RuleEditorControl(); // Tạo mới nếu chưa có hoặc đã dispose
            }

            // _ruleEditorControl lúc này chắc chắn không null
            var ruleEditorController = new RuleEditorController(_ruleEditorControl, _keyMappingService, _mainController);
            ruleEditorController.PrepareForEditing(ruleToEdit);

            ShowUserControlInPanel(_ruleEditorControl);
        }

        public void ShowSettingsEditor(AppSettings currentSettings) // currentSettings được đảm bảo non-null từ controller
        {
            if (_settingsControl == null || _settingsControl.IsDisposed)
            {
                _settingsControl = new SettingsControl(); // Tạo mới
            }

            // _settingsControl lúc này chắc chắn không null
            var settingsController = new SettingsController(_settingsControl, _appSettingsService, _mainController);
            settingsController.PrepareForSettings();

            ShowUserControlInPanel(_settingsControl);
        }

        public void ShowHomeControl()
        {
            if (_homeControl == null || _homeControl.IsDisposed)
            {
                _homeControl = new HomeControl();
                // Nếu HomeControl có Controller riêng và cần khởi tạo:
                // var homeController = new HomeController(_homeControl, ...services...);
                // homeController.StartInitialization(); (hoặc tên phương thức tương tự)
            }

            // Đảm bảo _homeControl không null trước khi truy cập
            if (_homeControl != null)
            {
                _homeControl.WelcomeMessage = "Welcome to QuickFn Mapper!";
                bool isServiceRunning = _keyMappingService.IsServiceEnabled;
                _homeControl.ServiceStatusText = isServiceRunning ? "Service is currently RUNNING" : "Service is currently STOPPED";
                _homeControl.ServiceStatusColor = isServiceRunning ? Color.DarkGreen : Color.DarkRed;

                _homeControl.ManageRulesRequested -= OnHomeManageRulesRequested;
                _homeControl.ManageRulesRequested += OnHomeManageRulesRequested;
                _homeControl.ToggleServiceStatusRequested -= OnHomeToggleServiceRequested;
                _homeControl.ToggleServiceStatusRequested += OnHomeToggleServiceRequested;
            }
            ShowUserControlInPanel(_homeControl);
        }

        public void ShowUserNotification(string message, string caption, MessageBoxIcon icon)
        {
            MessageBox.Show(this, message, caption, MessageBoxButtons.OK, icon);
        }

        public void ClearRuleSelection()
        {
            listViewRules.SelectedItems.Clear();
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
        private void MainForm_Load(object? sender, EventArgs e) // Sửa: object? sender
        {
            ViewLoaded?.Invoke(this, EventArgs.Empty);
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e) // Sửa: object? sender
        {
            CancelEventArgs cancelArgs = new CancelEventArgs(e.Cancel);
            ViewClosing?.Invoke(this, cancelArgs);
            e.Cancel = cancelArgs.Cancel;
        }
        #endregion

        #region UI Control Event Handlers
        // LƯU Ý: Đảm bảo mỗi phương thức xử lý sự kiện dưới đây CHỈ ĐƯỢC ĐỊNH NGHĨA MỘT LẦN DUY NHẤT
        // trong file MainForm.cs này để tránh lỗi "Ambiguous Call".

        private void exitToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbToggleService_Click(object? sender, EventArgs e)
        {
            ToggleServiceStatusClicked?.Invoke(this, EventArgs.Empty);
        }

        private void tsbAddRule_Click(object? sender, EventArgs e)
        {
            AddRuleClicked?.Invoke(this, EventArgs.Empty);
        }

        private void tsbEditRule_Click(object? sender, EventArgs e)
        {
            EditRuleClicked?.Invoke(this, EventArgs.Empty);
        }

        private void tsbDeleteRule_Click(object? sender, EventArgs e)
        {
            DeleteRuleClicked?.Invoke(this, EventArgs.Empty);
        }

        private void tsbSettings_Click(object? sender, EventArgs e)
        {
            OpenSettingsClicked?.Invoke(this, EventArgs.Empty);
        }

        private void tsbShowHome_Click(object? sender, EventArgs e) // Thêm handler cho nút Home mới (nếu có)
        {
            ShowHomeControl();
        }

        private void OnHomeManageRulesRequested(object? sender, EventArgs e)
        {
            ShowUserControlInPanel(null); // Ẩn UserControl hiện tại (HomeControl)
            // Giả định listViewRules nằm trực tiếp trên MainForm và sẽ hiện ra khi pnlMainContent rỗng
            // Hoặc nếu listViewRules nằm trong một UserControl khác (ví dụ RuleListControl):
            // ShowRuleListControl(); // Một phương thức tương tự ShowHomeControl
            if (pnlMainContent.Controls.Contains(listViewRules)) // Nếu listViewRules là con trực tiếp của panel
            {
                // Không cần làm gì thêm nếu listViewRules đã nằm trong panel và panel được clear
            }
            else // Nếu listViewRules nằm ngoài panel và panel đang che nó
            {
                pnlMainContent.Visible = false; // Ẩn panel
                listViewRules.Visible = true; // Hiện ListView
            }
            listViewRules.Focus();
        }

        private void OnHomeToggleServiceRequested(object? sender, EventArgs e)
        {
            ToggleServiceStatusClicked?.Invoke(this, EventArgs.Empty);
        }

        private void listViewRules_DoubleClick(object? sender, EventArgs e)
        {
            if (listViewRules.SelectedItems.Count > 0)
            {
                EditRuleClicked?.Invoke(this, EventArgs.Empty);
            }
        }

        private void listViewRules_ItemChecked(object? sender, ItemCheckedEventArgs e)
        {
            if (e.Item != null && e.Item.Tag is Guid ruleId)
            {
                KeyMappingRule? rule = _keyMappingService.GetRuleById(ruleId); // GetRuleById trả về nullable
                if (rule != null)
                {
                    bool newIsEnabledState = e.Item.Checked;
                    if (rule.IsEnabled != newIsEnabledState)
                    {
                        rule.IsEnabled = newIsEnabledState;
                        try
                        {
                            _keyMappingService.UpdateRule(rule);
                            _mainController.RefreshRulesDisplay();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[ERROR] MainForm: Error updating rule IsEnabled state via ListView. {ex.Message}");
                            ShowUserNotification($"Failed to update rule status: {ex.Message}", "Update Error", MessageBoxIcon.Error);
                            e.Item.Checked = !newIsEnabledState; // Khôi phục lại trạng thái checkbox
                        }
                    }
                }
            }
        }

        // Các handler cho MenuItems (nếu dùng lại handler của ToolStripButtons thì không cần định nghĩa lại)
        // Ví dụ:
        // private void addRuleToolStripMenuItem_Click(object? sender, EventArgs e)
        // {
        //    tsbAddRule_Click(sender, e);
        // }
        // private void editRuleToolStripMenuItem_Click(object? sender, EventArgs e)
        // {
        //    tsbEditRule_Click(sender, e);
        // }
        // private void deleteRuleToolStripMenuItem_Click(object? sender, EventArgs e)
        // {
        //    tsbDeleteRule_Click(sender, e);
        // }
        // private void toggleServiceToolStripMenuItem_Click(object? sender, EventArgs e)
        // {
        //    tsbToggleService_Click(sender, e);
        // }
        // private void settingsToolStripMenuItem_Click(object? sender, EventArgs e)
        // {
        //    tsbSettings_Click(sender, e);
        // }
        // private void aboutToolStripMenuItem_Click(object? sender, EventArgs e) { /* ... */ }


        #endregion

        #region IDisposable Support for owned services
        // Nếu MainForm chịu trách nhiệm dispose các service mà nó tạo ra
        // (trong trường hợp này là _globalHookService vì nó IDisposable)
        // thì cần thêm vào Dispose method của MainForm (trong MainForm.Designer.cs)
        // Em đã thêm gợi ý này vào file Designer minh họa.
        #endregion
    }
}