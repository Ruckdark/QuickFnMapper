// File: QuickFnMapper.WinForms/Views/Interfaces/IMainView.cs
#region Using Directives
using QuickFnMapper.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel; // For CancelEventHandler
using System.Windows.Forms; // For MessageBoxIcon
#endregion

namespace QuickFnMapper.WinForms.Views.Interfaces
{
    /// <summary>
    /// <para>Defines the contract for the main view (MainForm) of the application.</para>
    /// <para>Định nghĩa hợp đồng cho view chính (MainForm) của ứng dụng.</para>
    /// </summary>
    public interface IMainView
    {
        #region Properties for UI State
        /// <summary>
        /// <para>Gets or sets a value indicating whether the mapping service is currently presented as active in the UI.</para>
        /// <para>Lấy hoặc đặt một giá trị cho biết liệu dịch vụ ánh xạ có đang được hiển thị là đang hoạt động trong UI hay không.</para>
        /// </summary>
        bool IsServiceEnabledUI { get; set; }

        /// <summary>
        /// <para>Gets the unique identifier of the currently selected rule in the rule list, if any.</para>
        /// <para>Lấy mã định danh duy nhất của quy tắc hiện đang được chọn trong danh sách quy tắc, nếu có.</para>
        /// <para>Returns <c>null</c> if no rule is selected.</para>
        /// <para>Trả về <c>null</c> nếu không có quy tắc nào được chọn.</para>
        /// </summary>
        Guid? SelectedRuleId { get; } // Giữ nguyên là nullable Guid
        #endregion

        #region Methods to Update UI
        /// <summary>
        /// <para>Displays the list of key mapping rules in the UI.</para>
        /// <para>Hiển thị danh sách các quy tắc ánh xạ phím trong UI.</para>
        /// </summary>
        /// <param name="rules">
        /// <para>The collection of rules to display. Expected to be non-null, can be empty.</para>
        /// <para>Bộ sưu tập các quy tắc cần hiển thị. Được mong đợi là không null, có thể rỗng.</para>
        /// </param>
        void DisplayRules(IEnumerable<KeyMappingRule> rules);

        /// <summary>
        /// <para>Shows the rule editor view/control, optionally pre-filled with a rule for editing.</para>
        /// <para>Hiển thị view/control soạn thảo quy tắc, có thể được điền sẵn một quy tắc để chỉnh sửa.</para>
        /// </summary>
        /// <param name="ruleToEdit">
        /// <para>The rule to edit. If <c>null</c>, the editor is shown for adding a new rule.</para>
        /// <para>Quy tắc cần sửa. Nếu <c>null</c>, trình soạn thảo được hiển thị để thêm quy tắc mới.</para>
        /// </param>
        void ShowRuleEditor(KeyMappingRule? ruleToEdit); // Cho phép ruleToEdit là null

        /// <summary>
        /// <para>Shows the application settings view/control.</para>
        /// <para>Hiển thị view/control cài đặt ứng dụng.</para>
        /// </summary>
        /// <param name="currentSettings">
        /// <para>The current application settings to display and edit. Expected to be non-null.</para>
        /// <para>Cài đặt ứng dụng hiện tại để hiển thị và chỉnh sửa. Được mong đợi là không null.</para>
        /// </param>
        void ShowSettingsEditor(AppSettings currentSettings);

        /// <summary>
        /// <para>Displays a message to the user.</para>
        /// <para>Hiển thị một thông báo cho người dùng.</para>
        /// </summary>
        /// <param name="message"><para>The message text.</para><para>Nội dung thông báo.</para></param>
        /// <param name="caption"><para>The caption for the message box.</para><para>Tiêu đề cho hộp thoại thông báo.</para></param>
        /// <param name="icon"><para>The icon to display in the message box.</para><para>Biểu tượng hiển thị trong hộp thoại thông báo.</para></param>
        void ShowUserNotification(string message, string caption, MessageBoxIcon icon);

        /// <summary>
        /// <para>Clears any existing rule selection in the UI.</para>
        /// <para>Xóa lựa chọn quy tắc hiện có trong UI.</para>
        /// </summary>
        void ClearRuleSelection();
        #endregion

        #region Events triggered by UI actions
        /// <summary>
        /// <para>Occurs when the main form is loaded and ready for the controller to initialize data.</para>
        /// <para>Xảy ra khi form chính được tải và sẵn sàng để controller khởi tạo dữ liệu.</para>
        /// </summary>
        event EventHandler? ViewLoaded; // Sửa: Thêm '?'

        /// <summary>
        /// <para>Occurs when the user requests to toggle the enabled state of the key mapping service.</para>
        /// <para>Xảy ra khi người dùng yêu cầu bật/tắt trạng thái của dịch vụ ánh xạ phím.</para>
        /// </summary>
        event EventHandler? ToggleServiceStatusClicked; // Sửa: Thêm '?'

        /// <summary>
        /// <para>Occurs when the user requests to add a new key mapping rule.</para>
        /// <para>Xảy ra khi người dùng yêu cầu thêm một quy tắc ánh xạ phím mới.</para>
        /// </summary>
        event EventHandler? AddRuleClicked; // Sửa: Thêm '?'

        /// <summary>
        /// <para>Occurs when the user requests to edit the currently selected key mapping rule.</para>
        /// <para>Xảy ra khi người dùng yêu cầu chỉnh sửa quy tắc ánh xạ phím hiện đang được chọn.</para>
        /// </summary>
        event EventHandler? EditRuleClicked; // Sửa: Thêm '?'

        /// <summary>
        /// <para>Occurs when the user requests to delete the currently selected key mapping rule.</para>
        /// <para>Xảy ra khi người dùng yêu cầu xóa quy tắc ánh xạ phím hiện đang được chọn.</para>
        /// </summary>
        event EventHandler? DeleteRuleClicked; // Sửa: Thêm '?'

        /// <summary>
        /// <para>Occurs when the user requests to open the application settings view.</para>
        /// <para>Xảy ra khi người dùng yêu cầu mở view cài đặt ứng dụng.</para>
        /// </summary>
        event EventHandler? OpenSettingsClicked; // Sửa: Thêm '?'

        /// <summary>
        /// <para>Occurs when the main form is about to be closed.</para>
        /// <para>Xảy ra khi form chính sắp được đóng.</para>
        /// <para>Allows the controller to perform cleanup or confirmation. The <see cref="CancelEventArgs.Cancel"/> property can be set to true to prevent closing.</para>
        /// <para>Cho phép controller thực hiện dọn dẹp hoặc xác nhận. Thuộc tính <see cref="CancelEventArgs.Cancel"/> có thể được đặt thành true để ngăn việc đóng.</para>
        /// </summary>
        event CancelEventHandler? ViewClosing; // Sửa: Thêm '?' (CancelEventHandler là delegate chuẩn)
        #endregion
    }
}