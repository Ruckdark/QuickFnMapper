#region Using Directives
using QuickFnMapper.Core.Models; // Đại ca đã có OriginalKeyData, ActionType ở đây [cite: 1035]
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
using QuickFnMapper.WinForms.Utils;   // Cho UIMessageHelper (nếu dùng) và Program.AppThemeManager [cite: 1036]
using QuickFnMapper.WinForms.Themes; // Cho ColorPalette [cite: 1036]
#endregion

namespace QuickFnMapper.WinForms.Views
{
    public partial class RuleEditorControl : UserControl, IRuleEditorView
    {
        private bool _isCapturingKey = false;
        private OriginalKeyData _capturedKeyData = new OriginalKeyData(Keys.None); // [cite: 1037]
        private OriginalKeyData _selectedOriginalKeyField = new OriginalKeyData(Keys.None); // [cite: 1038]

        // Event này có thể được Controller sử dụng để biết key đã thay đổi mà không cần phải get SelectedOriginalKey liên tục.
        // Hoặc nếu Controller không cần, có thể bỏ đi.
        public event EventHandler<OriginalKeyData>? UserSelectedOriginalKeyChanged;
        public event EventHandler? EditorCancelled; // Implement event từ interface

        public RuleEditorControl()
        {
            InitializeComponent(); // [cite: 1039]
            this.Load += RuleEditorControl_Load; // [cite: 1039]

            // Đăng ký và áp dụng theme
            if (Program.AppThemeManager != null)
            {
                Program.AppThemeManager.ThemeChanged += OnAppThemeChanged; // [cite: 1040]
                Program.AppThemeManager.ApplyThemeToFormOrUserControl(this); // Áp dụng theme ban đầu [cite: 1040]
            }
            this.Disposed += RuleEditorControl_Disposed; // Đăng ký sự kiện Disposed [cite: 1041]
        }

        #region Theme Handling
        private void OnAppThemeChanged(object? sender, EventArgs e) // Đại ca đã có EventArgs, không phải ThemeChangedEventArgs
        {
            if (Program.AppThemeManager != null)
            {
                Program.AppThemeManager.ApplyThemeToFormOrUserControl(this); // [cite: 1042]
            }
            // Thêm logic cập nhật màu nền cho txtOriginalKeyDisplay dựa trên theme và trạng thái capture
            UpdateOriginalKeyDisplayAppearance();
        }

        private void RuleEditorControl_Disposed(object? sender, EventArgs e)
        {
            // Hủy đăng ký sự kiện ThemeChanged
            if (Program.AppThemeManager != null)
            {
                Program.AppThemeManager.ThemeChanged -= OnAppThemeChanged; // [cite: 1043]
                Debug.WriteLine("[INFO] RuleEditorControl_Disposed: Unsubscribed from AppThemeManager.ThemeChanged."); // [cite: 1043]
            }
        }
        #endregion

        #region Key Capture Logic

        private void UpdateOriginalKeyDisplayAppearance()
        {
            if (txtOriginalKeyDisplay == null || Program.AppThemeManager == null) return;

            if (_isCapturingKey)
            {
                // Màu khi đang capture (có thể làm khác biệt hơn)
                // Ví dụ: dùng một màu vàng nhạt hoặc một màu từ palette nếu có
                txtOriginalKeyDisplay.BackColor = ControlPaint.Light(Program.AppThemeManager.CurrentPalette.ControlBackground, 0.3f); // Sáng hơn một chút
            }
            else
            {
                txtOriginalKeyDisplay.BackColor = Program.AppThemeManager.CurrentPalette.ControlBackground;
            }
        }

        private void btnCaptureKey_Click(object? sender, EventArgs e)
        {
            _isCapturingKey = true; // [cite: 1045]
            _capturedKeyData = new OriginalKeyData(Keys.None); // Reset lại key đang capture
            if (txtOriginalKeyDisplay != null)
            {
                txtOriginalKeyDisplay.Text = "Press any key combination..."; // [cite: 1046]
                UpdateOriginalKeyDisplayAppearance(); // Cập nhật màu nền
                txtOriginalKeyDisplay.Focus(); // [cite: 1046]
            }
            CaptureOriginalKeyRequested?.Invoke(this, EventArgs.Empty); // [cite: 1092] Event này vẫn có thể giữ lại nếu Controller cần biết
        }

        private void txtOriginalKeyDisplay_Enter(object? sender, EventArgs e)
        {
            // Không làm gì khi focus bằng Tab, chỉ thay đổi khi nhấn nút "Capture Key"
        }

        private void txtOriginalKeyDisplay_Leave(object? sender, EventArgs e)
        {
            if (_isCapturingKey) // Nếu người dùng click ra ngoài mà chưa chọn xong key
            {
                _isCapturingKey = false; // Dừng capture [cite: 1049]
                // Hiển thị key đã chọn trước đó (nếu có) hoặc "None"
                if (txtOriginalKeyDisplay != null)
                {
                    txtOriginalKeyDisplay.Text = _selectedOriginalKeyField?.ToString() ?? Keys.None.ToString(); // [cite: 1050]
                    UpdateOriginalKeyDisplayAppearance(); // Reset màu nền
                }
                Debug.WriteLine("[INFO] RuleEditorControl: Key capture stopped due to focus leave.");
            }
        }

        private void txtOriginalKeyDisplay_KeyDown(object? sender, KeyEventArgs e)
        {
            if (_isCapturingKey)
            {
                e.SuppressKeyPress = true; // [cite: 1051]
                e.Handled = true; // [cite: 1051]

                // Kiểm tra xem chỉ có phím bổ trợ (modifier) được nhấn hay không
                // Và không phải là chính phím bổ trợ đó đang được kiểm tra (e.Modifiers == e.KeyCode)
                bool isOnlyModifierWithoutOtherKey = (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.Menu || e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin);

                if (isOnlyModifierWithoutOtherKey && e.Modifiers == e.KeyCode && !(e.Control && e.KeyCode != Keys.ControlKey) && !(e.Shift && e.KeyCode != Keys.ShiftKey) && !(e.Alt && e.KeyCode != Keys.Menu))
                {
                    // Nếu chỉ nhấn một phím modifier (Ctrl, Shift, Alt, Win) thì hiển thị nó và tiếp tục chờ
                    _capturedKeyData = new OriginalKeyData(Keys.None, e.Control, e.Shift, e.Alt, (Control.ModifierKeys & Keys.LWin) == Keys.LWin || (Control.ModifierKeys & Keys.RWin) == Keys.RWin); // [cite: 1053]
                    if (txtOriginalKeyDisplay != null)
                    {
                        string displayText = _capturedKeyData.ToString();
                        if (displayText.EndsWith("None") && displayText != "None") // Ví dụ: "Ctrl + None"
                        {
                            displayText = displayText.Replace("None", "...") + " + ???";
                        }
                        else if (displayText == "None")
                        {
                            displayText = "Press a key...";
                        }
                        else
                        {
                            displayText += " + ???";
                        }
                        txtOriginalKeyDisplay.Text = displayText;
                    }
                    return; // Tiếp tục chờ phím chính
                }

                // Đã bắt được tổ hợp phím hoàn chỉnh (có phím chính)
                _capturedKeyData = new OriginalKeyData(e.KeyCode, e.Control, e.Shift, e.Alt, (Control.ModifierKeys & Keys.LWin) == Keys.LWin || (Control.ModifierKeys & Keys.RWin) == Keys.RWin); // [cite: 1055]

                _selectedOriginalKeyField = _capturedKeyData; // Gán vào field lưu trữ giá trị đã chọn
                _isCapturingKey = false; // Kết thúc trạng thái capture *TRƯỚC KHI* cập nhật UI hoặc focus

                // Cập nhật UI hiển thị key đã bắt
                if (txtOriginalKeyDisplay != null)
                {
                    txtOriginalKeyDisplay.Text = _selectedOriginalKeyField.ToString();
                    UpdateOriginalKeyDisplayAppearance(); // Reset màu nền
                }

                UserSelectedOriginalKeyChanged?.Invoke(this, _selectedOriginalKeyField); // Thông báo cho controller (nếu có đăng ký)

                btnSaveRule?.Focus(); // Chuyển focus sang nút Save [cite: 1055]
            }
        }
        #endregion

        #region Event Handlers for Controller (Được đăng ký trong Designer.cs)
        private void RuleEditorControl_Load(object? sender, EventArgs e)
        {
            // Program.AppThemeManager.ApplyThemeToFormOrUserControl(this) đã gọi trong constructor
            // nhưng gọi lại ở đây để đảm bảo nếu có control con được thêm sau constructor.
            if (Program.AppThemeManager != null) Program.AppThemeManager.ApplyThemeToFormOrUserControl(this);
            ViewInitialized?.Invoke(this, EventArgs.Empty); // [cite: 1056]
        }
        private void cmbActionType_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbActionType?.SelectedItem is ActionType selectedType)
                SelectedActionTypeChanged?.Invoke(this, selectedType); // [cite: 1057]
        }
        private void btnSaveRule_Click(object? sender, EventArgs e) { SaveRuleClicked?.Invoke(this, EventArgs.Empty); } // [cite: 1058]
        private void btnCancelRule_Click(object? sender, EventArgs e)
        {
            Debug.WriteLine($"[INFO] RuleEditorControl '{this.Name}': btnCancelRule_Click fired.");
            CancelClicked?.Invoke(this, EventArgs.Empty); // Event này cho RuleEditorController biết để gọi CloseEditor
            EditorCancelled?.Invoke(this, EventArgs.Empty); // Event mới này cho MainController biết để điều hướng
        }
        #endregion

        #region IRuleEditorView Implementation
        public string RuleName
        {
            get => txtRuleName?.Text ?? string.Empty; // [cite: 1060]
            set { if (txtRuleName != null) txtRuleName.Text = value; } // [cite: 1061]
        }
        public bool IsRuleEnabled
        {
            get => chkRuleEnabled?.Checked ?? false; // [cite: 1062]
            set { if (chkRuleEnabled != null) chkRuleEnabled.Checked = value; } // [cite: 1062]
        }
        public OriginalKeyData SelectedOriginalKey
        {
            get => _selectedOriginalKeyField; // [cite: 1063]
            set
            {
                _selectedOriginalKeyField = value ?? new OriginalKeyData(Keys.None); // [cite: 1064]
                if (txtOriginalKeyDisplay != null && !_isCapturingKey) // Chỉ cập nhật nếu không phải đang trong quá trình capture từ KeyDown [cite: 1064]
                {
                    txtOriginalKeyDisplay.Text = _selectedOriginalKeyField.ToString(); // [cite: 1064]
                }
            }
        }
        public ActionType SelectedActionType
        {
            get
            {
                if (cmbActionType?.SelectedItem is ActionType type) return type; // [cite: 1065]
                return ActionType.None; // [cite: 1065] (Hoặc giá trị mặc định an toàn khác)
            }
            set
            {
                if (cmbActionType != null) cmbActionType.SelectedItem = value; // [cite: 1066]
            }
        }
        public string ActionParameterValue
        {
            get
            {
                string val = txtActionParameter?.Text ?? string.Empty;
                Debug.WriteLine($"[DEBUG] RuleEditorControl.ActionParameterValue GET: Returning '{val}' from txtActionParameter."); // Thêm log ở đây
                return val;
            }
            set { if (txtActionParameter != null) txtActionParameter.Text = value; }
        }
        public string? ActionParameterSecondaryValue
        {
            get => txtActionParameterSecondary?.Text; // [cite: 1068]
            set { if (txtActionParameterSecondary != null) txtActionParameterSecondary.Text = value; } // [cite: 1069]
        }
        public int RuleOrder
        {
            get => (int)(numOrder?.Value ?? 0); // [cite: 1070]
            set { if (numOrder != null) numOrder.Value = Math.Max(numOrder.Minimum, Math.Min(numOrder.Maximum, value)); } // [cite: 1071]
        }

        public void PopulateActionTypes(IEnumerable<ActionType> actionTypes)
        {
            if (this.cmbActionType != null)
            {
                object? cur = this.cmbActionType.SelectedItem; // [cite: 1072]
                this.cmbActionType.DataSource = null; // [cite: 1072]
                this.cmbActionType.DataSource = actionTypes.ToList(); // [cite: 1072]
                if (cur != null && this.cmbActionType.Items.Contains(cur)) // [cite: 1073]
                {
                    this.cmbActionType.SelectedItem = cur; // [cite: 1073]
                }
                else if (this.cmbActionType.Items.Count > 0) // [cite: 1073]
                {
                    this.cmbActionType.SelectedIndex = 0; // [cite: 1073]
                }
            }
        }
        public void PopulateMediaKeyParameters(IEnumerable<string> mediaKeyNames)
        {
            if (txtActionParameter != null && lblActionParameterPrompt != null)
            {
                var tt = new ToolTip(); // [cite: 1074]
                tt.SetToolTip(txtActionParameter, "Examples: VolumeUp, PlayPause, Mute, or VK code (e.g., 0xAE for VolumeDown).\nAvailable: " + string.Join(", ", mediaKeyNames)); // [cite: 1075]
            }
        }
        public void ConfigureActionParameterControls(ActionType actionType)
        {
            if (lblActionParameterPrompt == null || txtActionParameter == null || lblActionParameterSecondaryPrompt == null || txtActionParameterSecondary == null) return; // [cite: 1076]
            bool pVis = false; string pPrm = "Parameter:"; bool sVis = false; string sPrm = "Sec. Parameter:"; // [cite: 1077]

            // Giữ lại giá trị nếu người dùng đã nhập và chỉ thay đổi ActionType qua lại
            // string currentParam1 = txtActionParameter.Text;
            // string currentParam2 = txtActionParameterSecondary.Text;

            // Reset text nếu ActionType thực sự thay đổi sang một loại khác yêu cầu input khác
            // if (actionType != (cmbActionType.Tag as ActionType?)) // Giả sử lưu ActionType trước đó vào Tag
            // {
            //     txtActionParameter.Text = string.Empty;
            //     txtActionParameterSecondary.Text = string.Empty;
            // }
            // cmbActionType.Tag = actionType; // Lưu lại action type hiện tại

            switch (actionType)
            {
                case ActionType.SendMediaKey: pVis = true; pPrm = "Media Key Name/Code:"; break; // [cite: 1078]
                case ActionType.RunApplication: pVis = true; pPrm = "Application Path:"; sVis = true; sPrm = "Arguments (Optional):"; break; // [cite: 1079] // Thêm sVis và sPrm
                case ActionType.OpenUrl: pVis = true; pPrm = "URL / Path:"; break; // [cite: 1079]
                case ActionType.SendText: pVis = true; pPrm = "Text to Send:"; break; // [cite: 1080]
                case ActionType.TriggerHotkeyOrCommand: pVis = true; pPrm = "Hotkey/Command String:"; sVis = true; sPrm = "Type (Hotkey/Command):"; break; // [cite: 1081]
                case ActionType.SetScreenBrightness:
                    pVis = true; // [cite: 1082]
                    pPrm = "Brightness (+/-Value or Absolute Value 0-100):"; // Mô tả cho người dùng [cite: 1082]
                    sVis = false; // [cite: 1084]
                    break; // [cite: 1085]
                case ActionType.None: // [cite: 1086]
                default:
                    // pVis, sVis giữ nguyên là false (đã reset ở trên)
                    pPrm = "Parameter:"; // Reset về mặc định
                    break; // [cite: 1086]
            }
            lblActionParameterPrompt.Text = pPrm;
            txtActionParameter.Visible = pVis;
            lblActionParameterPrompt.Visible = pVis;
            lblActionParameterSecondaryPrompt.Text = sPrm;
            txtActionParameterSecondary.Visible = sVis;
            lblActionParameterSecondaryPrompt.Visible = sVis; // [cite: 1087]
        }
        public void ShowErrorMessage(string message, string caption)
        {
            // Sử dụng UIMessageHelper nếu đã có và muốn chuẩn hóa
            // UIMessageHelper.ShowError(message, caption, this.FindForm());
            MessageBox.Show(this, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error); // [cite: 1088]
        }
        public void CloseView(bool success)
        {
            var pForm = this.FindForm(); // [cite: 1089]
            if (pForm != null)
            {
                if (!(pForm is MainForm mainFrm)) // [cite: 1090]
                {
                    pForm.DialogResult = success ? DialogResult.OK : DialogResult.Cancel; // [cite: 1090]
                    pForm.Close(); // [cite: 1090]
                }
                else
                {
                    this.Visible = false; // [cite: 1090]
                    mainFrm.ShowHomeControl(); // [cite: 1090] // Quay về HomeControl khi đóng
                }
            }
        }
        public void CloseEditor()
        {
            Debug.WriteLine($"[INFO] RuleEditorControl '{this.Name}': CloseEditor called.");
            this.Visible = false;
            // Không tự động điều hướng ở đây. MainController sẽ quyết định.
            // Nếu UserControl này được tạo mới mỗi lần, có thể cân nhắc Dispose ở đây
            // hoặc để MainForm quản lý vòng đời của nó.
            // Hiện tại, chỉ ẩn đi là đủ vì MainForm sẽ thay thế nó trong panel.
        }

        public event EventHandler? ViewInitialized; // [cite: 1091]
        public event EventHandler? SaveRuleClicked; // [cite: 1091]
        public event EventHandler? CancelClicked; // [cite: 1091]
        public event EventHandler<ActionType>? SelectedActionTypeChanged; // [cite: 1091]
        public event EventHandler? CaptureOriginalKeyRequested; // Event này vẫn có thể giữ lại nếu Controller cần biết [cite: 1092]
        #endregion
    }
}