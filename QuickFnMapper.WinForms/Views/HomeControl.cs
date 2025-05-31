#region Using Directives
using QuickFnMapper.WinForms.Views.Interfaces;
using System;
using System.Drawing;
using System.Windows.Forms;
using QuickFnMapper.WinForms.Utils;   // Cho Program.AppThemeManager
using QuickFnMapper.WinForms.Themes; // Cho ColorPalette
using System.Diagnostics;
#endregion

namespace QuickFnMapper.WinForms.Views
{
    public partial class HomeControl : UserControl, IHomeView
    {
        // components được khai báo và quản lý bởi HomeControl.Designer.cs
        // private System.ComponentModel.IContainer components = null;

        #region Constructors
        public HomeControl()
        {
            InitializeComponent(); // Gọi để khởi tạo các control con từ Designer.cs

            if (Program.AppThemeManager != null)
            {
                // Đăng ký lắng nghe sự kiện thay đổi theme
                Program.AppThemeManager.ThemeChanged += OnAppThemeChanged;
                // Áp dụng theme hiện tại ngay khi control được khởi tạo
                // Phương thức này sẽ gọi ThemeManager để áp dụng cho UserControl và các control con của nó
                Program.AppThemeManager.ApplyThemeToFormOrUserControl(this);
            }

            // Đăng ký vào sự kiện Disposed của UserControl này để hủy đăng ký ThemeChanged
            this.Disposed += HomeControl_Disposed;
        }
        #endregion

        #region Theme Handling
        private void OnAppThemeChanged(object? sender, EventArgs e)
        {
            if (Program.AppThemeManager != null)
            {
                // Khi theme thay đổi, yêu cầu ThemeManager áp dụng lại theme cho UserControl này và các con của nó
                Program.AppThemeManager.ApplyThemeToFormOrUserControl(this);
            }
        }

        
        // private void ApplyCurrentTheme(ColorPalette palette)
        // {
        //     this.BackColor = palette.FormBackground; 
        //     if (Program.AppThemeManager != null)
        //     {
        //         // Gọi phương thức chung của ThemeManager để áp dụng cho các control con
        //         // mà không cần lặp lại logic trong mỗi UserControl
        //         Program.AppThemeManager.ApplyThemeToNestedControls(this.Controls, palette);
        //     }
        //     // Hoặc tự áp dụng riêng cho từng control trong HomeControl nếu muốn khác biệt:
        //     // if (lblWelcomeMessage != null) lblWelcomeMessage.ForeColor = palette.TextColor;
        //     // ...
        // }
        #endregion

        #region Dispose Handling
        private void HomeControl_Disposed(object? sender, EventArgs e)
        {
            // Hủy đăng ký sự kiện ThemeChanged khi control này bị dispose
            if (Program.AppThemeManager != null)
            {
                Program.AppThemeManager.ThemeChanged -= OnAppThemeChanged;
                Debug.WriteLine("[INFO] HomeControl_Disposed: Unsubscribed from AppThemeManager.ThemeChanged.");
            }
        }
        // Không override Dispose(bool disposing) ở đây, file Designer.cs đã xử lý.
        #endregion

        #region IHomeView Implementation
        public string WelcomeMessage
        {
            set { if (this.lblWelcomeMessage != null) this.lblWelcomeMessage.Text = value; }
        }
        public string ServiceStatusText
        {
            set { if (this.lblServiceStatus != null) this.lblServiceStatus.Text = value; }
        }
        public Color ServiceStatusColor
        {
            set { if (this.lblServiceStatus != null) this.lblServiceStatus.ForeColor = value; }
        }

        public void ShowUserNotification(string message, string caption, MessageBoxIcon icon)
        {
            // Có thể cân nhắc dùng UIMessageHelper ở đây nếu muốn chuẩn hóa
            MessageBox.Show(this, message, caption, MessageBoxButtons.OK, icon);
        }

        public event EventHandler? ViewLoaded;
        public event EventHandler? ManageRulesRequested;
        public event EventHandler? ToggleServiceStatusRequested;
        #endregion

        #region Control Event Handlers (Các handler này được đăng ký trong HomeControl.Designer.cs)
        private void HomeControl_Load(object? sender, EventArgs e)
        {
            ViewLoaded?.Invoke(this, EventArgs.Empty);
        }

        private void btnManageRules_Click(object? sender, EventArgs e)
        {
            ManageRulesRequested?.Invoke(this, EventArgs.Empty);
        }

        private void btnToggleServiceOnHome_Click(object? sender, EventArgs e)
        {
            ToggleServiceStatusRequested?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}