#region Using Directives
using QuickFnMapper.Core.Models;
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
#endregion

namespace QuickFnMapper.WinForms.Views
{
    // Quan trọng: Đảm bảo là 'partial class'
    public partial class RuleEditorControl : UserControl, IRuleEditorView
    {
        public RuleEditorControl()
        {
            InitializeComponent(); // Gọi phương thức từ file .Designer.cs

            // Đăng ký các sự kiện từ control UI để kích hoạt event của Interface
            // Đại ca sẽ tạo các control này trong Designer
            // Giả sử các control đã được tạo:
            if (this.btnSaveRule != null) // btnSaveRule là Button "Lưu"
                this.btnSaveRule.Click += (s, e) => SaveRuleClicked?.Invoke(this, EventArgs.Empty);

            if (this.btnCancelRule != null) // btnCancelRule là Button "Hủy"
                this.btnCancelRule.Click += (s, e) => CancelClicked?.Invoke(this, EventArgs.Empty);

            if (this.cmbActionType != null) // cmbActionType là ComboBox chọn ActionType
                this.cmbActionType.SelectedIndexChanged += CmbActionType_SelectedIndexChanged;

            this.Load += RuleEditorControl_Load;
        }

        private void RuleEditorControl_Load(object? sender, EventArgs e)
        {
            ViewInitialized?.Invoke(this, EventArgs.Empty);
        }

        private void CmbActionType_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbActionType.SelectedItem is ActionType selectedType)
            {
                SelectedActionTypeChanged?.Invoke(this, selectedType);
            }
        }

        #region IRuleEditorView Implementation

        public string RuleName
        {
            get { return this.txtRuleName?.Text ?? string.Empty; } // Giả sử có TextBox tên là txtRuleName
            set { if (this.txtRuleName != null) this.txtRuleName.Text = value; }
        }

        public bool IsRuleEnabled
        {
            get { return this.chkRuleEnabled?.Checked ?? false; } // Giả sử có CheckBox tên là chkRuleEnabled
            set { if (this.chkRuleEnabled != null) this.chkRuleEnabled.Checked = value; }
        }

        private OriginalKeyData _selectedOriginalKey = new OriginalKeyData(Keys.None);
        public OriginalKeyData SelectedOriginalKey
        {
            get { return _selectedOriginalKey; }
            set
            {
                _selectedOriginalKey = value ?? new OriginalKeyData(Keys.None);
                // Cập nhật UI hiển thị phím này (ví dụ: một TextBox hiển thị tên phím)
                if (this.txtOriginalKeyDisplay != null) // Giả sử có TextBox txtOriginalKeyDisplay
                {
                    this.txtOriginalKeyDisplay.Text = _selectedOriginalKey.ToString();
                }
            }
        }

        public ActionType SelectedActionType
        {
            get { return this.cmbActionType?.SelectedItem is ActionType type ? type : ActionType.None; }
            set { if (this.cmbActionType != null) this.cmbActionType.SelectedItem = value; }
        }

        public string ActionParameterValue
        {
            get { return this.txtActionParameter?.Text ?? string.Empty; }
            set { if (this.txtActionParameter != null) this.txtActionParameter.Text = value; }
        }

        public string? ActionParameterSecondaryValue
        {
            get { return this.txtActionParameterSecondary?.Text; }
            set { if (this.txtActionParameterSecondary != null) this.txtActionParameterSecondary.Text = value; }
        }

        public int RuleOrder
        {
            get { return (int)(this.numOrder?.Value ?? 0); } // Giả sử có NumericUpDown tên là numOrder
            set { if (this.numOrder != null) this.numOrder.Value = Math.Max(this.numOrder.Minimum, Math.Min(this.numOrder.Maximum, value)); }
        }

        public void PopulateActionTypes(IEnumerable<ActionType> actionTypes)
        {
            if (this.cmbActionType != null)
            {
                this.cmbActionType.DataSource = actionTypes.ToList();
            }
        }

        public void PopulateMediaKeyParameters(IEnumerable<string> mediaKeyNames)
        {
            // Nếu ActionParameterValue là một ComboBox cho media keys
            // if (this.cmbMediaKeyParam != null)
            // {
            //     this.cmbMediaKeyParam.DataSource = mediaKeyNames.ToList();
            //     this.cmbMediaKeyParam.Visible = true;
            //     this.txtActionParameter.Visible = false; 
            // }
        }

        public void ConfigureActionParameterControls(ActionType actionType)
        {
            // Ví dụ:
            if (this.txtActionParameter != null && this.lblActionParameterPrompt != null)
            {
                bool isVisible = (actionType == ActionType.RunApplication ||
                                  actionType == ActionType.OpenUrl ||
                                  actionType == ActionType.SendText ||
                                  actionType == ActionType.SendMediaKey);
                this.txtActionParameter.Visible = isVisible;
                this.lblActionParameterPrompt.Visible = isVisible; // Giả sử có một Label lblActionParameterPrompt

                if (isVisible)
                {
                    switch (actionType)
                    {
                        case ActionType.RunApplication: this.lblActionParameterPrompt.Text = "Application Path:"; break;
                        case ActionType.OpenUrl: this.lblActionParameterPrompt.Text = "URL:"; break;
                        case ActionType.SendMediaKey: this.lblActionParameterPrompt.Text = "Media Key Name (e.g., VolumeUp):"; break;
                        case ActionType.SendText: this.lblActionParameterPrompt.Text = "Text to Send:"; break;
                    }
                }
            }
            if (this.txtActionParameterSecondary != null && this.lblActionParameterSecondaryPrompt != null)
            {
                // Hiện tại chưa có ActionType nào dùng tham số phụ, ví dụ:
                this.txtActionParameterSecondary.Visible = false;
                this.lblActionParameterSecondaryPrompt.Visible = false;
            }
        }

        public void ShowErrorMessage(string message, string caption)
        {
            MessageBox.Show(this, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void CloseView(bool success)
        {
            var parentForm = this.FindForm();
            if (parentForm != null)
            {
                if (!(parentForm is MainForm))
                {
                    // Nếu là dialog, có thể set DialogResult
                    // parentForm.DialogResult = success ? DialogResult.OK : DialogResult.Cancel;
                    parentForm.Close();
                }
                else
                {
                    this.Visible = false;
                    // MainController sẽ gọi RefreshRulesDisplay trên MainForm
                }
            }
        }

        public event EventHandler? ViewInitialized;
        public event EventHandler? SaveRuleClicked;
        public event EventHandler? CancelClicked;
        public event EventHandler<ActionType>? SelectedActionTypeChanged;
        #endregion

        // Các control cần được Đại ca tạo trong Designer:
        // private System.Windows.Forms.TextBox txtRuleName;
        // private System.Windows.Forms.CheckBox chkRuleEnabled;
        // private System.Windows.Forms.TextBox txtOriginalKeyDisplay; (Để hiển thị phím đã chọn)
        // private System.Windows.Forms.Button btnCaptureKey; (Nút để bắt đầu quá trình chọn phím)
        // private System.Windows.Forms.ComboBox cmbActionType;
        // private System.Windows.Forms.Label lblActionParameterPrompt;
        // private System.Windows.Forms.TextBox txtActionParameter;
        // private System.Windows.Forms.Label lblActionParameterSecondaryPrompt;
        // private System.Windows.Forms.TextBox txtActionParameterSecondary;
        // private System.Windows.Forms.NumericUpDown numOrder;
        // private System.Windows.Forms.Button btnSaveRule;
        // private System.Windows.Forms.Button btnCancelRule;
    }
}