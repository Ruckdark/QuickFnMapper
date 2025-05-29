// File: QuickFnMapper.WinForms/Views/Interfaces/IHomeView.cs
#region Using Directives
using System;
using System.Windows.Forms; // For MessageBoxIcon
#endregion

namespace QuickFnMapper.WinForms.Views.Interfaces
{
    /// <summary>
    /// <para>Defines the contract for the Home view (HomeControl).</para>
    /// <para>Định nghĩa hợp đồng cho view Trang chủ (HomeControl).</para>
    /// </summary>
    public interface IHomeView
    {
        /// <summary>
        /// <para>Sets the welcome message displayed on the Home view.</para>
        /// <para>Đặt thông điệp chào mừng hiển thị trên view Trang chủ.</para>
        /// </summary>
        string WelcomeMessage { set; }

        /// <summary>
        /// <para>Sets the current status of the key mapping service displayed on the Home view.</para>
        /// <para>Đặt trạng thái hiện tại của dịch vụ ánh xạ phím được hiển thị trên view Trang chủ.</para>
        /// </summary>
        string ServiceStatusText { set; }

        /// <summary>
        /// <para>Sets the color of the service status text.</para>
        /// <para>Đặt màu cho dòng chữ trạng thái dịch vụ.</para>
        /// </summary>
        System.Drawing.Color ServiceStatusColor { set; }

        /// <summary>
        /// <para>Displays a notification to the user.</para>
        /// <para>Hiển thị một thông báo cho người dùng.</para>
        /// </summary>
        void ShowUserNotification(string message, string caption, MessageBoxIcon icon);

        /// <summary>
        /// <para>Occurs when the view is loaded and ready for initialization.</para>
        /// <para>Xảy ra khi view được tải và sẵn sàng để khởi tạo.</para>
        /// </summary>
        event EventHandler ViewLoaded;

        /// <summary>
        /// <para>Occurs when the user requests to navigate to the rule management screen.</para>
        /// <para>Xảy ra khi người dùng yêu cầu điều hướng đến màn hình quản lý quy tắc.</para>
        /// </summary>
        event EventHandler ManageRulesRequested;

        /// <summary>
        /// <para>Occurs when the user requests to toggle the service status (if a button for this exists on HomeControl).</para>
        /// <para>Xảy ra khi người dùng yêu cầu bật/tắt trạng thái dịch vụ (nếu có nút cho việc này trên HomeControl).</para>
        /// </summary>
        event EventHandler ToggleServiceStatusRequested;
    }
}