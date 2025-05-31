#region Using Directives
using QuickFnMapper.Core.Models; // Cho AppSettings
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
using QuickFnMapper.WinForms.Utils; // 
using QuickFnMapper.WinForms.Themes; // 
using System.Diagnostics;
#endregion

namespace QuickFnMapper.WinForms.Views
{
    public partial class SettingsControl : UserControl, ISettingsView
    {
        public SettingsControl()
        {
            InitializeComponent(); // 
            Debug.WriteLine($"[INFO] SettingsControl '{this.Name}': Constructor called, InitializeComponent finished.");
            if (Program.AppThemeManager != null)
            {
                Program.AppThemeManager.ThemeChanged += OnAppThemeChanged; // 
                Debug.WriteLine($"[INFO] SettingsControl '{this.Name}': Subscribed to ThemeManager.ThemeChanged.");
                Program.AppThemeManager.ApplyThemeToFormOrUserControl(this); // 
                Debug.WriteLine($"[INFO] SettingsControl '{this.Name}': Initial theme applied.");
            }
            this.Disposed += SettingsControl_Disposed; // 

            // Sự kiện Load vẫn được đăng ký trong Designer.cs, nhưng không cần raise ViewInitialized nữa
            // this.Load += SettingsControl_Load; // Dòng này đã có trong Designer.cs 
        }

        #region Theme Handling
        private void OnAppThemeChanged(object? sender, EventArgs e)
        {
            Debug.WriteLine($"[INFO] SettingsControl '{this.Name}': OnAppThemeChanged called.");
            if (Program.AppThemeManager != null)
            {
                Program.AppThemeManager.ApplyThemeToFormOrUserControl(this); // 
                Debug.WriteLine($"[INFO] SettingsControl '{this.Name}': Theme re-applied due to ThemeChanged event.");
            }
        }
        #endregion

        #region Dispose Handling
        private void SettingsControl_Disposed(object? sender, EventArgs e)
        {
            Debug.WriteLine($"[INFO] SettingsControl '{this.Name}': Disposed event called.");
            if (Program.AppThemeManager != null)
            {
                Program.AppThemeManager.ThemeChanged -= OnAppThemeChanged; // 
                Debug.WriteLine($"[INFO] SettingsControl '{this.Name}': Unsubscribed from AppThemeManager.ThemeChanged."); // 
            }
        }
        #endregion

        #region Control Event Handlers (Đăng ký trong Designer.cs)
        private void SettingsControl_Load(object? sender, EventArgs e) // 
        {
            Debug.WriteLine($"[INFO] SettingsControl '{this.Name}': Load event fired. Visible: {this.Visible}, Size: {this.Size}, Parent: {this.Parent?.Name}");
            // ViewInitialized?.Invoke(this, EventArgs.Empty); // KHÔNG CẦN NỮA 
        }

        private void btnSaveSettings_Click(object? sender, EventArgs e) // 
        {
            Debug.WriteLine($"[INFO] SettingsControl '{this.Name}': btnSaveSettings_Click fired.");
            SaveSettingsClicked?.Invoke(this, EventArgs.Empty); // 
        }

        private void btnCancelSettings_Click(object? sender, EventArgs e) // 
        {
            Debug.WriteLine($"[INFO] SettingsControl '{this.Name}': btnCancelSettings_Click fired.");
            CancelSettingsClicked?.Invoke(this, EventArgs.Empty); // 
        }

        private void btnBrowseRulesPath_Click(object? sender, EventArgs e) // 
        {
            Debug.WriteLine($"[INFO] SettingsControl '{this.Name}': btnBrowseRulesPath_Click fired.");
            BrowseRulesFilePathClicked?.Invoke(this, EventArgs.Empty); // 
        }
        #endregion

        #region ISettingsView Implementation

        public bool StartWithWindows
        {
            get
            {
                bool val = chkStartWithWindows?.Checked ?? false; // 
                // Debug.WriteLine($"SettingsControl.StartWithWindows GET: chkStartWithWindows is {(chkStartWithWindows == null ? "NULL" : "NOT NULL")}, returning {val}");
                return val;
            }
            set
            {
                Debug.WriteLine($"SettingsControl.StartWithWindows SET: Value = {value}. Control 'chkStartWithWindows' is {(chkStartWithWindows == null ? "NULL" : "NOT NULL")}");
                if (chkStartWithWindows != null)
                {
                    chkStartWithWindows.Checked = value; // 
                    Debug.WriteLine($"SettingsControl.StartWithWindows: chkStartWithWindows.Checked set to {value}. Visible: {chkStartWithWindows.Visible}, Size: {chkStartWithWindows.Size}, Location: {chkStartWithWindows.Location}");
                }
                else
                {
                    Debug.WriteLine($"[ERROR] SettingsControl.StartWithWindows: Control 'chkStartWithWindows' is NULL!");
                }
            }
        }

        public bool IsGlobalHookEnabledView
        {
            get
            {
                bool val = chkEnableGlobalHook?.Checked ?? false; // 
                // Debug.WriteLine($"SettingsControl.IsGlobalHookEnabledView GET: chkEnableGlobalHook is {(chkEnableGlobalHook == null ? "NULL" : "NOT NULL")}, returning {val}");
                return val;
            }
            set
            {
                Debug.WriteLine($"SettingsControl.IsGlobalHookEnabledView SET: Value = {value}. Control 'chkEnableGlobalHook' is {(chkEnableGlobalHook == null ? "NULL" : "NOT NULL")}");
                if (chkEnableGlobalHook != null)
                {
                    chkEnableGlobalHook.Checked = value; // 
                    Debug.WriteLine($"SettingsControl.IsGlobalHookEnabledView: chkEnableGlobalHook.Checked set to {value}. Visible: {chkEnableGlobalHook.Visible}, Size: {chkEnableGlobalHook.Size}, Location: {chkEnableGlobalHook.Location}");
                }
                else
                {
                    Debug.WriteLine($"[ERROR] SettingsControl.IsGlobalHookEnabledView: Control 'chkEnableGlobalHook' is NULL!");
                }
            }
        }

        public bool MinimizeToTrayOnClose
        {
            get
            {
                bool val = chkMinimizeToTray?.Checked ?? false; // 
                // Debug.WriteLine($"SettingsControl.MinimizeToTrayOnClose GET: chkMinimizeToTray is {(chkMinimizeToTray == null ? "NULL" : "NOT NULL")}, returning {val}");
                return val;
            }
            set
            {
                Debug.WriteLine($"SettingsControl.MinimizeToTrayOnClose SET: Value = {value}. Control 'chkMinimizeToTray' is {(chkMinimizeToTray == null ? "NULL" : "NOT NULL")}");
                if (chkMinimizeToTray != null)
                {
                    chkMinimizeToTray.Checked = value; // 
                    Debug.WriteLine($"SettingsControl.MinimizeToTrayOnClose: chkMinimizeToTray.Checked set to {value}. Visible: {chkMinimizeToTray.Visible}, Size: {chkMinimizeToTray.Size}, Location: {chkMinimizeToTray.Location}");
                }
                else
                {
                    Debug.WriteLine($"[ERROR] SettingsControl.MinimizeToTrayOnClose: Control 'chkMinimizeToTray' is NULL!");
                }
            }
        }

        public bool ShowNotifications
        {
            get
            {
                bool val = chkShowNotifications?.Checked ?? false; // 
                // Debug.WriteLine($"SettingsControl.ShowNotifications GET: chkShowNotifications is {(chkShowNotifications == null ? "NULL" : "NOT NULL")}, returning {val}");
                return val;
            }
            set
            {
                Debug.WriteLine($"SettingsControl.ShowNotifications SET: Value = {value}. Control 'chkShowNotifications' is {(chkShowNotifications == null ? "NULL" : "NOT NULL")}");
                if (chkShowNotifications != null)
                {
                    chkShowNotifications.Checked = value; // 
                    Debug.WriteLine($"SettingsControl.ShowNotifications: chkShowNotifications.Checked set to {value}. Visible: {chkShowNotifications.Visible}, Size: {chkShowNotifications.Size}, Location: {chkShowNotifications.Location}");
                }
                else
                {
                    Debug.WriteLine($"[ERROR] SettingsControl.ShowNotifications: Control 'chkShowNotifications' is NULL!");
                }
            }
        }

        public string RulesFilePathView
        {
            get
            {
                string val = txtRulesFilePath?.Text ?? string.Empty; // 
                // Debug.WriteLine($"SettingsControl.RulesFilePathView GET: txtRulesFilePath is {(txtRulesFilePath == null ? "NULL" : "NOT NULL")}, returning '{val}'");
                return val;
            }
            set
            {
                Debug.WriteLine($"SettingsControl.RulesFilePathView SET: Value = '{value}'. Control 'txtRulesFilePath' is {(txtRulesFilePath == null ? "NULL" : "NOT NULL")}");
                if (txtRulesFilePath != null)
                {
                    txtRulesFilePath.Text = value; // 
                    Debug.WriteLine($"SettingsControl.RulesFilePathView: txtRulesFilePath.Text set to '{value}'. Visible: {txtRulesFilePath.Visible}, Size: {txtRulesFilePath.Size}, Location: {txtRulesFilePath.Location}");
                }
                else
                {
                    Debug.WriteLine($"[ERROR] SettingsControl.RulesFilePathView: Control 'txtRulesFilePath' is NULL!");
                }
            }
        }

        public string ApplicationThemeView
        {
            get
            {
                string val = cmbAppTheme?.SelectedItem?.ToString() ?? "SystemDefault"; // 
                // Debug.WriteLine($"SettingsControl.ApplicationThemeView GET: cmbAppTheme is {(cmbAppTheme == null ? "NULL" : "NOT NULL")}, returning '{val}'");
                return val;
            }
            set
            {
                Debug.WriteLine($"SettingsControl.ApplicationThemeView SET: Value = '{value}'. Control 'cmbAppTheme' is {(cmbAppTheme == null ? "NULL" : "NOT NULL")}");
                if (cmbAppTheme != null) // 
                {
                    if (!string.IsNullOrEmpty(value) && cmbAppTheme.Items.Contains(value))
                    {
                        cmbAppTheme.SelectedItem = value; // 
                    }
                    else if (cmbAppTheme.Items.Contains("SystemDefault")) // 
                    {
                        cmbAppTheme.SelectedItem = "SystemDefault"; // 
                    }
                    else if (cmbAppTheme.Items.Count > 0) // 
                    {
                        cmbAppTheme.SelectedIndex = 0; // 
                    }
                    Debug.WriteLine($"SettingsControl.ApplicationThemeView: cmbAppTheme.SelectedItem set to '{cmbAppTheme.SelectedItem}'. Visible: {cmbAppTheme.Visible}, Size: {cmbAppTheme.Size}, Location: {cmbAppTheme.Location}");
                }
                else
                {
                    Debug.WriteLine($"[ERROR] SettingsControl.ApplicationThemeView: Control 'cmbAppTheme' is NULL!");
                }
            }
        }

        public void PopulateApplicationThemes(IEnumerable<string> themes) // 
        {
            Debug.WriteLine($"[INFO] SettingsControl '{this.Name}': PopulateApplicationThemes called. Control 'cmbAppTheme' is {(cmbAppTheme == null ? "NULL" : "NOT NULL")}");
            if (this.cmbAppTheme != null)
            {
                string? currentTheme = this.cmbAppTheme.SelectedItem?.ToString(); // 
                Debug.WriteLine($"[INFO] SettingsControl.PopulateApplicationThemes: Current theme in ComboBox (before clear): '{currentTheme}'");
                this.cmbAppTheme.Items.Clear(); // 
                foreach (var theme in themes)
                {
                    this.cmbAppTheme.Items.Add(theme); // 
                }
                Debug.WriteLine($"[INFO] SettingsControl.PopulateApplicationThemes: ComboBox items repopulated with {this.cmbAppTheme.Items.Count} themes.");

                if (!string.IsNullOrEmpty(currentTheme) && this.cmbAppTheme.Items.Contains(currentTheme)) // 
                {
                    this.cmbAppTheme.SelectedItem = currentTheme; // 
                }
                else if (this.cmbAppTheme.Items.Contains("SystemDefault")) // 
                {
                    this.cmbAppTheme.SelectedItem = "SystemDefault"; // 
                }
                else if (this.cmbAppTheme.Items.Count > 0) // 
                {
                    this.cmbAppTheme.SelectedIndex = 0; // 
                }
                Debug.WriteLine($"[INFO] SettingsControl.PopulateApplicationThemes: Final selected theme in ComboBox: '{this.cmbAppTheme.SelectedItem}'");
            }
        }

        public void ShowUserNotification(string message, string caption, MessageBoxIcon icon) // 
        {
            Debug.WriteLine($"[INFO] SettingsControl '{this.Name}': ShowUserNotification called. Title: '{caption}', Message: '{message}'");
            MessageBox.Show(this, message, caption, MessageBoxButtons.OK, icon); // 
        }

        public void CloseView(bool success) // 
        {
            Debug.WriteLine($"[INFO] SettingsControl '{this.Name}': CloseView called with success = {success}.");
            var parentForm = this.FindForm(); // 
            if (parentForm != null)
            {
                if (!(parentForm is MainForm mainFrm))
                {
                    parentForm.DialogResult = success ? DialogResult.OK : DialogResult.Cancel; // 
                    parentForm.Close(); // 
                }
                else
                {
                    this.Visible = false; // 
                    Debug.WriteLine($"[INFO] SettingsControl '{this.Name}': Set Visible = false. Calling mainFrm.ShowHomeControl().");
                    mainFrm.ShowHomeControl(); // 
                }
            }
            else
            {
                Debug.WriteLine($"[WARN] SettingsControl '{this.Name}': CloseView called, but parentForm is null.");
            }
        }

        // Event ViewInitialized không còn cần thiết trong ISettingsView và không cần khai báo ở đây
        // public event EventHandler? ViewInitialized; // 

        public event EventHandler? SaveSettingsClicked; // 
        public event EventHandler? CancelSettingsClicked; // 
        public event EventHandler? BrowseRulesFilePathClicked; // 
        #endregion
    }
}