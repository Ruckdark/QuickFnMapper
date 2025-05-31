// MINH HỌA - ĐẠI CA NÊN DÙNG VISUAL STUDIO DESIGNER ĐỂ TẠO FILE NÀY
namespace QuickFnMapper.WinForms.Views
{
    partial class RuleEditorControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblRuleName = new Label();
            txtRuleName = new TextBox();
            chkRuleEnabled = new CheckBox();
            grpOriginalKey = new GroupBox();
            txtOriginalKeyDisplay = new TextBox();
            btnCaptureKey = new Button();
            grpTargetAction = new GroupBox();
            lblActionParameterSecondaryPrompt = new Label();
            txtActionParameterSecondary = new TextBox();
            lblActionParameterPrompt = new Label();
            txtActionParameter = new TextBox();
            cmbActionType = new ComboBox();
            lblActionType = new Label();
            lblOrder = new Label();
            numOrder = new NumericUpDown();
            btnSaveRule = new Button();
            btnCancelRule = new Button();
            grpOriginalKey.SuspendLayout();
            grpTargetAction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numOrder).BeginInit();
            SuspendLayout();
            // 
            // lblRuleName
            // 
            lblRuleName.AutoSize = true;
            lblRuleName.Font = new Font("Segoe UI", 9F);
            lblRuleName.Location = new Point(17, 20);
            lblRuleName.Name = "lblRuleName";
            lblRuleName.Size = new Size(102, 25);
            lblRuleName.TabIndex = 0;
            lblRuleName.Text = "Rule Name:";
            // 
            // txtRuleName
            // 
            txtRuleName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtRuleName.Font = new Font("Segoe UI", 9F);
            txtRuleName.Location = new Point(120, 17);
            txtRuleName.Name = "txtRuleName";
            txtRuleName.Size = new Size(831, 31);
            txtRuleName.TabIndex = 1;
            // 
            // chkRuleEnabled
            // 
            chkRuleEnabled.AutoSize = true;
            chkRuleEnabled.Font = new Font("Segoe UI", 9F);
            chkRuleEnabled.Location = new Point(120, 46);
            chkRuleEnabled.Name = "chkRuleEnabled";
            chkRuleEnabled.Size = new Size(140, 29);
            chkRuleEnabled.TabIndex = 2;
            chkRuleEnabled.Text = "Rule Enabled";
            chkRuleEnabled.UseVisualStyleBackColor = true;
            // 
            // grpOriginalKey
            // 
            grpOriginalKey.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpOriginalKey.Controls.Add(txtOriginalKeyDisplay);
            grpOriginalKey.Controls.Add(btnCaptureKey);
            grpOriginalKey.Font = new Font("Segoe UI", 9F);
            grpOriginalKey.Location = new Point(20, 78);
            grpOriginalKey.Name = "grpOriginalKey";
            grpOriginalKey.Padding = new Padding(10, 5, 10, 10);
            grpOriginalKey.Size = new Size(931, 65);
            grpOriginalKey.TabIndex = 3;
            grpOriginalKey.TabStop = false;
            grpOriginalKey.Text = "Original Key Combination";
            // 
            // txtOriginalKeyDisplay
            // 
            txtOriginalKeyDisplay.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtOriginalKeyDisplay.Location = new Point(13, 25);
            txtOriginalKeyDisplay.Name = "txtOriginalKeyDisplay";
            txtOriginalKeyDisplay.ReadOnly = true;
            txtOriginalKeyDisplay.Size = new Size(801, 31);
            txtOriginalKeyDisplay.TabIndex = 0;
            txtOriginalKeyDisplay.TextAlign = HorizontalAlignment.Center;
            txtOriginalKeyDisplay.Enter += txtOriginalKeyDisplay_Enter;
            txtOriginalKeyDisplay.KeyDown += txtOriginalKeyDisplay_KeyDown;
            txtOriginalKeyDisplay.Leave += txtOriginalKeyDisplay_Leave;
            // 
            // btnCaptureKey
            // 
            btnCaptureKey.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCaptureKey.Location = new Point(820, 24);
            btnCaptureKey.Name = "btnCaptureKey";
            btnCaptureKey.Size = new Size(98, 32);
            btnCaptureKey.TabIndex = 1;
            btnCaptureKey.Text = "Ca&pture Key";
            btnCaptureKey.UseVisualStyleBackColor = true;
            btnCaptureKey.Click += btnCaptureKey_Click;
            // 
            // grpTargetAction
            // 
            grpTargetAction.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpTargetAction.Controls.Add(lblActionParameterSecondaryPrompt);
            grpTargetAction.Controls.Add(txtActionParameterSecondary);
            grpTargetAction.Controls.Add(lblActionParameterPrompt);
            grpTargetAction.Controls.Add(txtActionParameter);
            grpTargetAction.Controls.Add(cmbActionType);
            grpTargetAction.Controls.Add(lblActionType);
            grpTargetAction.Font = new Font("Segoe UI", 9F);
            grpTargetAction.Location = new Point(20, 155);
            grpTargetAction.Name = "grpTargetAction";
            grpTargetAction.Padding = new Padding(10, 5, 10, 10);
            grpTargetAction.Size = new Size(931, 130);
            grpTargetAction.TabIndex = 4;
            grpTargetAction.TabStop = false;
            grpTargetAction.Text = "Target Action";
            // 
            // lblActionParameterSecondaryPrompt
            // 
            lblActionParameterSecondaryPrompt.AutoSize = true;
            lblActionParameterSecondaryPrompt.Location = new Point(10, 92);
            lblActionParameterSecondaryPrompt.Name = "lblActionParameterSecondaryPrompt";
            lblActionParameterSecondaryPrompt.Size = new Size(131, 25);
            lblActionParameterSecondaryPrompt.TabIndex = 4;
            lblActionParameterSecondaryPrompt.Text = "Sec. Parameter:";
            lblActionParameterSecondaryPrompt.Visible = false;
            // 
            // txtActionParameterSecondary
            // 
            txtActionParameterSecondary.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtActionParameterSecondary.Location = new Point(147, 89);
            txtActionParameterSecondary.Name = "txtActionParameterSecondary";
            txtActionParameterSecondary.Size = new Size(771, 31);
            txtActionParameterSecondary.TabIndex = 5;
            txtActionParameterSecondary.Visible = false;
            // 
            // lblActionParameterPrompt
            // 
            lblActionParameterPrompt.AutoSize = true;
            lblActionParameterPrompt.Location = new Point(10, 60);
            lblActionParameterPrompt.Name = "lblActionParameterPrompt";
            lblActionParameterPrompt.Size = new Size(95, 25);
            lblActionParameterPrompt.TabIndex = 2;
            lblActionParameterPrompt.Text = "Parameter:";
            // 
            // txtActionParameter
            // 
            txtActionParameter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtActionParameter.Location = new Point(147, 57);
            txtActionParameter.Name = "txtActionParameter";
            txtActionParameter.Size = new Size(771, 31);
            txtActionParameter.TabIndex = 3;
            // 
            // cmbActionType
            // 
            cmbActionType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbActionType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbActionType.FormattingEnabled = true;
            cmbActionType.Location = new Point(147, 25);
            cmbActionType.Name = "cmbActionType";
            cmbActionType.Size = new Size(771, 33);
            cmbActionType.TabIndex = 1;
            cmbActionType.SelectedIndexChanged += cmbActionType_SelectedIndexChanged;
            // 
            // lblActionType
            // 
            lblActionType.AutoSize = true;
            lblActionType.Location = new Point(10, 28);
            lblActionType.Name = "lblActionType";
            lblActionType.Size = new Size(109, 25);
            lblActionType.TabIndex = 0;
            lblActionType.Text = "Action Type:";
            // 
            // lblOrder
            // 
            lblOrder.AutoSize = true;
            lblOrder.Font = new Font("Segoe UI", 9F);
            lblOrder.Location = new Point(17, 297);
            lblOrder.Name = "lblOrder";
            lblOrder.Size = new Size(101, 25);
            lblOrder.TabIndex = 5;
            lblOrder.Text = "Rule Order:";
            // 
            // numOrder
            // 
            numOrder.Font = new Font("Segoe UI", 9F);
            numOrder.Location = new Point(120, 295);
            numOrder.Name = "numOrder";
            numOrder.Size = new Size(70, 31);
            numOrder.TabIndex = 6;
            // 
            // btnSaveRule
            // 
            btnSaveRule.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSaveRule.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnSaveRule.Location = new Point(775, 442);
            btnSaveRule.Name = "btnSaveRule";
            btnSaveRule.Size = new Size(85, 43);
            btnSaveRule.TabIndex = 7;
            btnSaveRule.Text = "&Save";
            btnSaveRule.UseVisualStyleBackColor = true;
            btnSaveRule.Click += btnSaveRule_Click;
            // 
            // btnCancelRule
            // 
            btnCancelRule.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancelRule.Font = new Font("Segoe UI", 9F);
            btnCancelRule.Location = new Point(866, 442);
            btnCancelRule.Name = "btnCancelRule";
            btnCancelRule.Size = new Size(85, 43);
            btnCancelRule.TabIndex = 8;
            btnCancelRule.Text = "&Cancel";
            btnCancelRule.UseVisualStyleBackColor = true;
            btnCancelRule.Click += btnCancelRule_Click;
            // 
            // RuleEditorControl
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            Controls.Add(btnCancelRule);
            Controls.Add(btnSaveRule);
            Controls.Add(numOrder);
            Controls.Add(lblOrder);
            Controls.Add(grpTargetAction);
            Controls.Add(grpOriginalKey);
            Controls.Add(chkRuleEnabled);
            Controls.Add(txtRuleName);
            Controls.Add(lblRuleName);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "RuleEditorControl";
            Padding = new Padding(10);
            Size = new Size(971, 498);
            Load += RuleEditorControl_Load;
            grpOriginalKey.ResumeLayout(false);
            grpOriginalKey.PerformLayout();
            grpTargetAction.ResumeLayout(false);
            grpTargetAction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numOrder).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblRuleName;
        private System.Windows.Forms.TextBox txtRuleName;
        private System.Windows.Forms.CheckBox chkRuleEnabled;
        private System.Windows.Forms.GroupBox grpOriginalKey;
        private System.Windows.Forms.TextBox txtOriginalKeyDisplay;
        private System.Windows.Forms.Button btnCaptureKey;
        private System.Windows.Forms.GroupBox grpTargetAction;
        private System.Windows.Forms.Label lblActionType;
        private System.Windows.Forms.ComboBox cmbActionType;
        private System.Windows.Forms.Label lblActionParameterPrompt;
        private System.Windows.Forms.TextBox txtActionParameter;
        private System.Windows.Forms.Label lblActionParameterSecondaryPrompt;
        private System.Windows.Forms.TextBox txtActionParameterSecondary;
        private System.Windows.Forms.Label lblOrder;
        private System.Windows.Forms.NumericUpDown numOrder;
        private System.Windows.Forms.Button btnSaveRule;
        private System.Windows.Forms.Button btnCancelRule;
    }
}