// MINH HỌA - ĐẠI CA NÊN DÙNG VISUAL STUDIO DESIGNER ĐỂ TẠO FILE NÀY
namespace QuickFnMapper.WinForms.Views
{
    partial class SettingsControl
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
            this.chkStartWithWindows = new System.Windows.Forms.CheckBox();
            this.chkEnableGlobalHook = new System.Windows.Forms.CheckBox();
            this.chkMinimizeToTray = new System.Windows.Forms.CheckBox();
            this.chkShowNotifications = new System.Windows.Forms.CheckBox();
            this.lblRulesFilePath = new System.Windows.Forms.Label();
            this.txtRulesFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowseRulesPath = new System.Windows.Forms.Button();
            this.lblAppTheme = new System.Windows.Forms.Label();
            this.cmbAppTheme = new System.Windows.Forms.ComboBox();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnCancelSettings = new System.Windows.Forms.Button();
            this.grpGeneral = new System.Windows.Forms.GroupBox();
            this.grpAdvanced = new System.Windows.Forms.GroupBox();
            this.grpGeneral.SuspendLayout();
            this.grpAdvanced.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpGeneral
            // 
            this.grpGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpGeneral.Controls.Add(this.chkStartWithWindows);
            this.grpGeneral.Controls.Add(this.chkEnableGlobalHook);
            this.grpGeneral.Controls.Add(this.chkMinimizeToTray);
            this.grpGeneral.Controls.Add(this.chkShowNotifications);
            this.grpGeneral.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grpGeneral.Location = new System.Drawing.Point(15, 15);
            this.grpGeneral.Name = "grpGeneral";
            this.grpGeneral.Padding = new System.Windows.Forms.Padding(10, 5, 10, 10);
            this.grpGeneral.Size = new System.Drawing.Size(410, 135); // Tăng chiều cao
            this.grpGeneral.TabIndex = 0;
            this.grpGeneral.TabStop = false;
            this.grpGeneral.Text = "General Settings";
            // 
            // chkStartWithWindows
            // 
            this.chkStartWithWindows.AutoSize = true;
            this.chkStartWithWindows.Location = new System.Drawing.Point(15, 25);
            this.chkStartWithWindows.Name = "chkStartWithWindows";
            this.chkStartWithWindows.Size = new System.Drawing.Size(128, 19);
            this.chkStartWithWindows.TabIndex = 0;
            this.chkStartWithWindows.Text = "Start with Windows";
            this.chkStartWithWindows.UseVisualStyleBackColor = true;
            // 
            // chkEnableGlobalHook
            // 
            this.chkEnableGlobalHook.AutoSize = true;
            this.chkEnableGlobalHook.Location = new System.Drawing.Point(15, 50);
            this.chkEnableGlobalHook.Name = "chkEnableGlobalHook";
            this.chkEnableGlobalHook.Size = new System.Drawing.Size(193, 19);
            this.chkEnableGlobalHook.TabIndex = 1;
            this.chkEnableGlobalHook.Text = "Enable Key Mapping Service";
            this.chkEnableGlobalHook.UseVisualStyleBackColor = true;
            // 
            // chkMinimizeToTray
            // 
            this.chkMinimizeToTray.AutoSize = true;
            this.chkMinimizeToTray.Location = new System.Drawing.Point(15, 75);
            this.chkMinimizeToTray.Name = "chkMinimizeToTray";
            this.chkMinimizeToTray.Size = new System.Drawing.Size(198, 19);
            this.chkMinimizeToTray.TabIndex = 2;
            this.chkMinimizeToTray.Text = "Minimize to System Tray on Close";
            this.chkMinimizeToTray.UseVisualStyleBackColor = true;
            // 
            // chkShowNotifications
            // 
            this.chkShowNotifications.AutoSize = true;
            this.chkShowNotifications.Location = new System.Drawing.Point(15, 100);
            this.chkShowNotifications.Name = "chkShowNotifications";
            this.chkShowNotifications.Size = new System.Drawing.Size(238, 19);
            this.chkShowNotifications.TabIndex = 3;
            this.chkShowNotifications.Text = "Show Notifications for Triggered Actions";
            this.chkShowNotifications.UseVisualStyleBackColor = true;
            // 
            // grpAdvanced
            // 
            this.grpAdvanced.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAdvanced.Controls.Add(this.lblRulesFilePath);
            this.grpAdvanced.Controls.Add(this.txtRulesFilePath);
            this.grpAdvanced.Controls.Add(this.btnBrowseRulesPath);
            this.grpAdvanced.Controls.Add(this.lblAppTheme);
            this.grpAdvanced.Controls.Add(this.cmbAppTheme);
            this.grpAdvanced.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grpAdvanced.Location = new System.Drawing.Point(15, 160); // Cách grpGeneral
            this.grpAdvanced.Name = "grpAdvanced";
            this.grpAdvanced.Padding = new System.Windows.Forms.Padding(10, 5, 10, 10);
            this.grpAdvanced.Size = new System.Drawing.Size(410, 100);
            this.grpAdvanced.TabIndex = 1;
            this.grpAdvanced.TabStop = false;
            this.grpAdvanced.Text = "Advanced Settings";
            // 
            // lblRulesFilePath
            // 
            this.lblRulesFilePath.AutoSize = true;
            this.lblRulesFilePath.Location = new System.Drawing.Point(12, 28);
            this.lblRulesFilePath.Name = "lblRulesFilePath";
            this.lblRulesFilePath.Size = new System.Drawing.Size(90, 15);
            this.lblRulesFilePath.TabIndex = 0;
            this.lblRulesFilePath.Text = "Rules File Path:";
            // 
            // txtRulesFilePath
            // 
            this.txtRulesFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRulesFilePath.Location = new System.Drawing.Point(110, 25);
            this.txtRulesFilePath.Name = "txtRulesFilePath";
            this.txtRulesFilePath.Size = new System.Drawing.Size(203, 23);
            this.txtRulesFilePath.TabIndex = 1;
            // 
            // btnBrowseRulesPath
            // 
            this.btnBrowseRulesPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseRulesPath.Location = new System.Drawing.Point(319, 24);
            this.btnBrowseRulesPath.Name = "btnBrowseRulesPath";
            this.btnBrowseRulesPath.Size = new System.Drawing.Size(78, 25);
            this.btnBrowseRulesPath.TabIndex = 2;
            this.btnBrowseRulesPath.Text = "&Browse...";
            this.btnBrowseRulesPath.UseVisualStyleBackColor = true;
            this.btnBrowseRulesPath.Click += new System.EventHandler(this.btnBrowseRulesPath_Click); // Handler trong .cs
            // 
            // lblAppTheme
            // 
            this.lblAppTheme.AutoSize = true;
            this.lblAppTheme.Location = new System.Drawing.Point(12, 60);
            this.lblAppTheme.Name = "lblAppTheme";
            this.lblAppTheme.Size = new System.Drawing.Size(92, 15);
            this.lblAppTheme.TabIndex = 3;
            this.lblAppTheme.Text = "App Theme:";
            // 
            // cmbAppTheme
            // 
            this.cmbAppTheme.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAppTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAppTheme.FormattingEnabled = true;
            this.cmbAppTheme.Location = new System.Drawing.Point(110, 57);
            this.cmbAppTheme.Name = "cmbAppTheme";
            this.cmbAppTheme.Size = new System.Drawing.Size(287, 23);
            this.cmbAppTheme.TabIndex = 4;
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnSaveSettings.Location = new System.Drawing.Point(249, 275); // Điều chỉnh vị trí
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(85, 28);
            this.btnSaveSettings.TabIndex = 2;
            this.btnSaveSettings.Text = "&Save";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click); // Handler trong .cs
            // 
            // btnCancelSettings
            // 
            this.btnCancelSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelSettings.DialogResult = System.Windows.Forms.DialogResult.Cancel; // Có thể hữu ích nếu dùng trong dialog
            this.btnCancelSettings.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCancelSettings.Location = new System.Drawing.Point(340, 275); // Điều chỉnh vị trí
            this.btnCancelSettings.Name = "btnCancelSettings";
            this.btnCancelSettings.Size = new System.Drawing.Size(85, 28);
            this.btnCancelSettings.TabIndex = 3;
            this.btnCancelSettings.Text = "&Cancel";
            this.btnCancelSettings.UseVisualStyleBackColor = true;
            this.btnCancelSettings.Click += new System.EventHandler(this.btnCancelSettings_Click); // Handler trong .cs
            // 
            // SettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.btnCancelSettings);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.grpAdvanced);
            this.Controls.Add(this.grpGeneral);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SettingsControl";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(440, 318); // Kích thước ví dụ
            this.Load += new System.EventHandler(this.SettingsControl_Load); // Handler trong .cs
            this.grpGeneral.ResumeLayout(false);
            this.grpGeneral.PerformLayout();
            this.grpAdvanced.ResumeLayout(false);
            this.grpAdvanced.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox grpGeneral;
        private System.Windows.Forms.CheckBox chkStartWithWindows;
        private System.Windows.Forms.CheckBox chkEnableGlobalHook;
        private System.Windows.Forms.CheckBox chkMinimizeToTray;
        private System.Windows.Forms.CheckBox chkShowNotifications;
        private System.Windows.Forms.GroupBox grpAdvanced;
        private System.Windows.Forms.Label lblRulesFilePath;
        private System.Windows.Forms.TextBox txtRulesFilePath;
        private System.Windows.Forms.Button btnBrowseRulesPath;
        private System.Windows.Forms.Label lblAppTheme;
        private System.Windows.Forms.ComboBox cmbAppTheme;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnCancelSettings;
    }
}