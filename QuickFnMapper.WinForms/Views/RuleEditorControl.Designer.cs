namespace QuickFnMapper.WinForms.Views
{
    partial class RuleEditorControl
    {
        /// <summary> 
        /// <para>Required designer variable.</para>
        /// <para>Biến thiết kế bắt buộc.</para>
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// <para>Clean up any resources being used.</para>
        /// <para>Dọn dẹp mọi tài nguyên đang được sử dụng.</para>
        /// </summary>
        /// <param name="disposing">
        /// <para>true if managed resources should be disposed; otherwise, false.</para>
        /// <para>true nếu các tài nguyên được quản lý nên được giải phóng; ngược lại, false.</para>
        /// </param>
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
        /// <para>Required method for Designer support - do not modify</para> 
        /// <para>the contents of this method with the code editor.</para>
        /// <para>Phương thức bắt buộc cho hỗ trợ Designer - không sửa đổi</para>
        /// <para>nội dung của phương thức này bằng trình soạn thảo mã.</para>
        /// </summary>
        private void InitializeComponent()
        {
            this.lblRuleName = new System.Windows.Forms.Label();
            this.txtRuleName = new System.Windows.Forms.TextBox();
            this.chkRuleEnabled = new System.Windows.Forms.CheckBox();
            this.grpOriginalKey = new System.Windows.Forms.GroupBox();
            this.txtOriginalKeyDisplay = new System.Windows.Forms.TextBox();
            this.btnCaptureKey = new System.Windows.Forms.Button();
            this.grpTargetAction = new System.Windows.Forms.GroupBox();
            this.lblActionParameterSecondaryPrompt = new System.Windows.Forms.Label();
            this.txtActionParameterSecondary = new System.Windows.Forms.TextBox();
            this.lblActionParameterPrompt = new System.Windows.Forms.Label();
            this.txtActionParameter = new System.Windows.Forms.TextBox();
            this.cmbActionType = new System.Windows.Forms.ComboBox();
            this.lblActionType = new System.Windows.Forms.Label();
            this.lblOrder = new System.Windows.Forms.Label();
            this.numOrder = new System.Windows.Forms.NumericUpDown();
            this.btnSaveRule = new System.Windows.Forms.Button();
            this.btnCancelRule = new System.Windows.Forms.Button();
            this.grpOriginalKey.SuspendLayout();
            this.grpTargetAction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOrder)).BeginInit();
            this.SuspendLayout();
            // 
            // lblRuleName
            // 
            this.lblRuleName.AutoSize = true;
            this.lblRuleName.Location = new System.Drawing.Point(15, 18);
            this.lblRuleName.Name = "lblRuleName";
            this.lblRuleName.Size = new System.Drawing.Size(63, 13);
            this.lblRuleName.TabIndex = 0;
            this.lblRuleName.Text = "Rule Name:";
            // 
            // txtRuleName
            // 
            this.txtRuleName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRuleName.Location = new System.Drawing.Point(100, 15);
            this.txtRuleName.Name = "txtRuleName";
            this.txtRuleName.Size = new System.Drawing.Size(327, 20);
            this.txtRuleName.TabIndex = 1;
            // 
            // chkRuleEnabled
            // 
            this.chkRuleEnabled.AutoSize = true;
            this.chkRuleEnabled.Location = new System.Drawing.Point(100, 41);
            this.chkRuleEnabled.Name = "chkRuleEnabled";
            this.chkRuleEnabled.Size = new System.Drawing.Size(94, 17);
            this.chkRuleEnabled.TabIndex = 2;
            this.chkRuleEnabled.Text = "Rule Enabled";
            this.chkRuleEnabled.UseVisualStyleBackColor = true;
            // 
            // grpOriginalKey
            // 
            this.grpOriginalKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpOriginalKey.Controls.Add(this.txtOriginalKeyDisplay);
            this.grpOriginalKey.Controls.Add(this.btnCaptureKey);
            this.grpOriginalKey.Location = new System.Drawing.Point(18, 70);
            this.grpOriginalKey.Name = "grpOriginalKey";
            this.grpOriginalKey.Size = new System.Drawing.Size(409, 60);
            this.grpOriginalKey.TabIndex = 3;
            this.grpOriginalKey.TabStop = false;
            this.grpOriginalKey.Text = "Original Key Combination";
            // 
            // txtOriginalKeyDisplay
            // 
            this.txtOriginalKeyDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOriginalKeyDisplay.Location = new System.Drawing.Point(9, 23);
            this.txtOriginalKeyDisplay.Name = "txtOriginalKeyDisplay";
            this.txtOriginalKeyDisplay.ReadOnly = true;
            this.txtOriginalKeyDisplay.Size = new System.Drawing.Size(299, 20);
            this.txtOriginalKeyDisplay.TabIndex = 0;
            this.txtOriginalKeyDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnCaptureKey
            // 
            this.btnCaptureKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCaptureKey.Location = new System.Drawing.Point(314, 21);
            this.btnCaptureKey.Name = "btnCaptureKey";
            this.btnCaptureKey.Size = new System.Drawing.Size(89, 23);
            this.btnCaptureKey.TabIndex = 1;
            this.btnCaptureKey.Text = "Capture Key...";
            this.btnCaptureKey.UseVisualStyleBackColor = true;
            // this.btnCaptureKey.Click += new System.EventHandler(this.btnCaptureKey_Click); // Đại ca cần tạo handler này trong RuleEditorControl.cs
            // 
            // grpTargetAction
            // 
            this.grpTargetAction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTargetAction.Controls.Add(this.lblActionParameterSecondaryPrompt);
            this.grpTargetAction.Controls.Add(this.txtActionParameterSecondary);
            this.grpTargetAction.Controls.Add(this.lblActionParameterPrompt);
            this.grpTargetAction.Controls.Add(this.txtActionParameter);
            this.grpTargetAction.Controls.Add(this.cmbActionType);
            this.grpTargetAction.Controls.Add(this.lblActionType);
            this.grpTargetAction.Location = new System.Drawing.Point(18, 140);
            this.grpTargetAction.Name = "grpTargetAction";
            this.grpTargetAction.Size = new System.Drawing.Size(409, 130);
            this.grpTargetAction.TabIndex = 4;
            this.grpTargetAction.TabStop = false;
            this.grpTargetAction.Text = "Target Action";
            // 
            // lblActionParameterSecondaryPrompt
            // 
            this.lblActionParameterSecondaryPrompt.AutoSize = true;
            this.lblActionParameterSecondaryPrompt.Location = new System.Drawing.Point(6, 94);
            this.lblActionParameterSecondaryPrompt.Name = "lblActionParameterSecondaryPrompt";
            this.lblActionParameterSecondaryPrompt.Size = new System.Drawing.Size(115, 13);
            this.lblActionParameterSecondaryPrompt.TabIndex = 5;
            this.lblActionParameterSecondaryPrompt.Text = "Secondary Parameter:";
            this.lblActionParameterSecondaryPrompt.Visible = false; // Sẽ được ConfigureActionParameterControls quản lý
            // 
            // txtActionParameterSecondary
            // 
            this.txtActionParameterSecondary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtActionParameterSecondary.Location = new System.Drawing.Point(127, 91);
            this.txtActionParameterSecondary.Name = "txtActionParameterSecondary";
            this.txtActionParameterSecondary.Size = new System.Drawing.Size(276, 20);
            this.txtActionParameterSecondary.TabIndex = 4;
            this.txtActionParameterSecondary.Visible = false; // Sẽ được ConfigureActionParameterControls quản lý
            // 
            // lblActionParameterPrompt
            // 
            this.lblActionParameterPrompt.AutoSize = true;
            this.lblActionParameterPrompt.Location = new System.Drawing.Point(6, 60);
            this.lblActionParameterPrompt.Name = "lblActionParameterPrompt";
            this.lblActionParameterPrompt.Size = new System.Drawing.Size(95, 13);
            this.lblActionParameterPrompt.TabIndex = 3;
            this.lblActionParameterPrompt.Text = "Action Parameter:";
            // 
            // txtActionParameter
            // 
            this.txtActionParameter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtActionParameter.Location = new System.Drawing.Point(127, 57);
            this.txtActionParameter.Name = "txtActionParameter";
            this.txtActionParameter.Size = new System.Drawing.Size(276, 20);
            this.txtActionParameter.TabIndex = 2;
            // 
            // cmbActionType
            // 
            this.cmbActionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbActionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbActionType.FormattingEnabled = true;
            this.cmbActionType.Location = new System.Drawing.Point(127, 23);
            this.cmbActionType.Name = "cmbActionType";
            this.cmbActionType.Size = new System.Drawing.Size(276, 21);
            this.cmbActionType.TabIndex = 1;
            // this.cmbActionType.SelectedIndexChanged += new System.EventHandler(this.CmbActionType_SelectedIndexChanged); // Đã đăng ký trong code-behind
            // 
            // lblActionType
            // 
            this.lblActionType.AutoSize = true;
            this.lblActionType.Location = new System.Drawing.Point(6, 26);
            this.lblActionType.Name = "lblActionType";
            this.lblActionType.Size = new System.Drawing.Size(68, 13);
            this.lblActionType.TabIndex = 0;
            this.lblActionType.Text = "Action Type:";
            // 
            // lblOrder
            // 
            this.lblOrder.AutoSize = true;
            this.lblOrder.Location = new System.Drawing.Point(15, 282);
            this.lblOrder.Name = "lblOrder";
            this.lblOrder.Size = new System.Drawing.Size(62, 13);
            this.lblOrder.TabIndex = 5;
            this.lblOrder.Text = "Rule Order:";
            // 
            // numOrder
            // 
            this.numOrder.Location = new System.Drawing.Point(100, 280);
            this.numOrder.Name = "numOrder";
            this.numOrder.Size = new System.Drawing.Size(120, 20);
            this.numOrder.TabIndex = 6;
            // 
            // btnSaveRule
            // 
            this.btnSaveRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveRule.Location = new System.Drawing.Point(271, 315);
            this.btnSaveRule.Name = "btnSaveRule";
            this.btnSaveRule.Size = new System.Drawing.Size(75, 23);
            this.btnSaveRule.TabIndex = 7;
            this.btnSaveRule.Text = "Save";
            this.btnSaveRule.UseVisualStyleBackColor = true;
            // this.btnSaveRule.Click += new System.EventHandler(this.btnSaveRule_Click); // Đã đăng ký trong code-behind
            // 
            // btnCancelRule
            // 
            this.btnCancelRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelRule.Location = new System.Drawing.Point(352, 315);
            this.btnCancelRule.Name = "btnCancelRule";
            this.btnCancelRule.Size = new System.Drawing.Size(75, 23);
            this.btnCancelRule.TabIndex = 8;
            this.btnCancelRule.Text = "Cancel";
            this.btnCancelRule.UseVisualStyleBackColor = true;
            // this.btnCancelRule.Click += new System.EventHandler(this.btnCancelRule_Click); // Đã đăng ký trong code-behind
            // 
            // RuleEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCancelRule);
            this.Controls.Add(this.btnSaveRule);
            this.Controls.Add(this.numOrder);
            this.Controls.Add(this.lblOrder);
            this.Controls.Add(this.grpTargetAction);
            this.Controls.Add(this.grpOriginalKey);
            this.Controls.Add(this.chkRuleEnabled);
            this.Controls.Add(this.txtRuleName);
            this.Controls.Add(this.lblRuleName);
            this.Name = "RuleEditorControl";
            this.Size = new System.Drawing.Size(445, 350); // Kích thước ví dụ
            // this.Load += new System.EventHandler(this.RuleEditorControl_Load); // Đã đăng ký trong code-behind
            this.grpOriginalKey.ResumeLayout(false);
            this.grpOriginalKey.PerformLayout();
            this.grpTargetAction.ResumeLayout(false);
            this.grpTargetAction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOrder)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
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