// File: QuickFnMapper.WinForms/Views/HomeControl.cs
#region Using Directives
using QuickFnMapper.WinForms.Views.Interfaces; // Để sử dụng IHomeView
using System;
using System.Drawing;
using System.Windows.Forms;
#endregion

namespace QuickFnMapper.WinForms.Views
{
    /// <summary>
    /// <para>UserControl representing the Home screen of the application.</para>
    /// <para>UserControl đại diện cho màn hình Trang chủ của ứng dụng.</para>
    /// </summary>
    public partial class HomeControl : UserControl, IHomeView // Implement IHomeView
    {
        #region Constructors
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="HomeControl"/> class.</para>
        /// <para>Khởi tạo một đối tượng mới của lớp <see cref="HomeControl"/>.</para>
        /// </summary>
        public HomeControl()
        {
            InitializeComponent();
            // Sự kiện Load đã được đăng ký trong HomeControl.Designer.cs (this.Load += ...)
            // Sự kiện Click cho các button cũng đã được đăng ký trong HomeControl.Designer.cs
        }
        #endregion

        #region IHomeView Implementation

        // --- Properties ---
        /// <summary>
        /// <inheritdoc cref="IHomeView.WelcomeMessage"/>
        /// </summary>
        public string WelcomeMessage
        {
            // Đảm bảo control đã được khởi tạo trước khi truy cập
            set { if (this.lblWelcomeMessage != null) this.lblWelcomeMessage.Text = value; }
        }

        /// <summary>
        /// <inheritdoc cref="IHomeView.ServiceStatusText"/>
        /// </summary>
        public string ServiceStatusText
        {
            set { if (this.lblServiceStatus != null) this.lblServiceStatus.Text = value; }
        }

        /// <summary>
        /// <inheritdoc cref="IHomeView.ServiceStatusColor"/>
        /// </summary>
        public Color ServiceStatusColor
        {
            set { if (this.lblServiceStatus != null) this.lblServiceStatus.ForeColor = value; }
        }

        // --- Methods ---
        /// <summary>
        /// <inheritdoc cref="IHomeView.ShowUserNotification"/>
        /// </summary>
        public void ShowUserNotification(string message, string caption, MessageBoxIcon icon)
        {
            MessageBox.Show(this, message, caption, MessageBoxButtons.OK, icon);
        }

        // --- Events ---
        /// <summary>
        /// <inheritdoc cref="IHomeView.ViewLoaded"/>
        /// </summary>
        public event EventHandler? ViewLoaded; // Sửa: Thêm '?' cho nullable

        /// <summary>
        /// <inheritdoc cref="IHomeView.ManageRulesRequested"/>
        /// </summary>
        public event EventHandler? ManageRulesRequested; // Sửa: Thêm '?' cho nullable

        /// <summary>
        /// <inheritdoc cref="IHomeView.ToggleServiceStatusRequested"/>
        /// </summary>
        public event EventHandler? ToggleServiceStatusRequested; // Sửa: Thêm '?' cho nullable

        #endregion

        #region Control Event Handlers (Methods for Designer)

        // Phương thức này sẽ được gọi bởi Designer khi sự kiện Load của UserControl được đăng ký
        private void HomeControl_Load(object? sender, EventArgs e) // Sửa: object? sender
        {
            ViewLoaded?.Invoke(this, EventArgs.Empty); // Kích hoạt event của IHomeView
        }

        // Phương thức này sẽ được gọi bởi Designer khi sự kiện Click của btnManageRules được đăng ký
        private void btnManageRules_Click(object? sender, EventArgs e) // Sửa: object? sender
        {
            ManageRulesRequested?.Invoke(this, EventArgs.Empty); // Kích hoạt event của IHomeView
        }

        // Phương thức này sẽ được gọi bởi Designer khi sự kiện Click của btnToggleService được đăng ký
        private void btnToggleService_Click(object? sender, EventArgs e) // Sửa: object? sender
        {
            ToggleServiceStatusRequested?.Invoke(this, EventArgs.Empty); // Kích hoạt event của IHomeView
        }

        #endregion
    }
}