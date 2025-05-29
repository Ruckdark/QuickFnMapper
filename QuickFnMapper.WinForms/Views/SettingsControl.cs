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
    public partial class SettingsControl : UserControl, ISettingsView
    {
        public SettingsControl()
        {
            InitializeComponent(); // Gọi phương thức từ file .Designer.cs

            // Đăng ký các sự kiện từ control UI
            // Giả sử các control đã được tạo trong Designer
            if (this.btnSaveSettings != null) // btnSaveSettings là Button "Lưu Cài Đặt"
                this.btnSaveSettings.Click += (s, e) => SaveSettingsClicked?.Invoke(this, EventArgs.Empty);

            if (this.btnCancelSettings != null) // btnCancelSettings là Button "Hủy"
                this.btnCancelSettings.Click += (s, e) => CancelSettingsClicked?.Invoke(this, EventArgs.Empty);

            if (this.btnBrowseRulesPath != null) // btnBrowseRulesPath là Button "Duyệt..."
                this.btnBrowseRulesPath.Click += (s, e) => BrowseRulesFilePathClicked?.Invoke(this, EventArgs.Empty);

            this.Load += SettingsControl_Load;
        }

        private void SettingsControl_Load(object? sender, EventArgs e)
        {
            ViewInitialized?.Invoke(this, EventArgs.Empty);
        }

        #region ISettingsView Implementation
        public bool StartWithWindows
        {
            get { return this.chkStartWithWindows?.Checked ?? false; }
            set { if (this.chkStartWithWindows != null) this.chkStartWithWindows.Checked = value; }
        }
        public bool IsGlobalHookEnabledView
        {
            get { return this.chkEnableGlobalHook?.Checked ?? false; }
            set { if (this.chkEnableGlobalHook != null) this.chkEnableGlobalHook.Checked = value; }
        }
        public bool MinimizeToTrayOnClose
        {
            get { return this.chkMinimizeToTray?.Checked ?? false; }
            set { if (this.chkMinimizeToTray != null) this.chkMinimizeToTray.Checked = value; }
        }
        public bool ShowNotifications
        {
            get { return this.chkShowNotifications?.Checked ?? false; }
            set { if (this.chkShowNotifications != null) this.chkShowNotifications.Checked = value; }
        }
        public string RulesFilePathView
        {
            get { return this.txtRulesFilePath?.Text ?? string.Empty; }
            set { if (this.txtRulesFilePath != null) this.txtRulesFilePath.Text = value; }
        }
        public string ApplicationThemeView
        {
            get { return this.cmbAppTheme?.SelectedItem?.ToString() ?? "SystemDefault"; }
            set { if (this.cmbAppTheme != null) this.cmbAppTheme.SelectedItem = value; }
        }

        public void PopulateApplicationThemes(IEnumerable<string> themes)
        {
            if (this.cmbAppTheme != null)
            {
                this.cmbAppTheme.Items.Clear();
                foreach (var theme in themes)
                {
                    this.cmbAppTheme.Items.Add(theme);
                }
                if (this.cmbAppTheme.Items.Count > 0 && string.IsNullOrEmpty(this.ApplicationThemeView))
                {
                    this.cmbAppTheme.SelectedIndex = 0;
                }
                else
                {
                    this.cmbAppTheme.SelectedItem = this.ApplicationThemeView;
                }
            }
        }

        public void ShowUserNotification(string message, string caption, MessageBoxIcon icon)
        {
            MessageBox.Show(this, message, caption, MessageBoxButtons.OK, icon);
        }

        public void CloseView(bool success)
        {
            var parentForm = this.FindForm();
            if (parentForm != null)
            {
                if (!(parentForm is MainForm))
                {
                    parentForm.Close();
                }
                else
                {
                    this.Visible = false;
                }
            }
        }

        public event EventHandler? ViewInitialized;
        public event EventHandler? SaveSettingsClicked;
        public event EventHandler? CancelSettingsClicked;
        public event EventHandler? BrowseRulesFilePathClicked;
        #endregion

        // Các control cần được Đại ca tạo trong Designer:
        // private System.Windows.Forms.CheckBox chkStartWithWindows;
        // private System.Windows.Forms.CheckBox chkEnableGlobalHook;
        // private System.Windows.Forms.CheckBox chkMinimizeToTray;
        // private System.Windows.Forms.CheckBox chkShowNotifications;
        // private System.Windows.Forms.TextBox txtRulesFilePath;
        // private System.Windows.Forms.Button btnBrowseRulesPath;
        // private System.Windows.Forms.ComboBox cmbAppTheme;
        // private System.Windows.Forms.Button btnSaveSettings;
        // private System.Windows.Forms.Button btnCancelSettings;
    }
}