#region Using Directives
using QuickFnMapper.Core.Models; // Dùng cho AppSettings, KeyMappingRule
using System;
using System.Collections.Generic;
using System.ComponentModel;      // Dùng cho CancelEventHandler
using System.Windows.Forms;      // Dùng cho MessageBoxIcon
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
        /// </summary>
        Guid? SelectedRuleId { get; }
        #endregion

        #region Methods to Update UI
        /// <summary>
        /// <para>Displays the list of key mapping rules in the UI.</para>
        /// <para>Hiển thị danh sách các quy tắc ánh xạ phím trong UI.</para>
        /// </summary>
        /// <param name="rules">The collection of rules to display. Expected to be non-null, can be empty.</param>
        void DisplayRules(IEnumerable<KeyMappingRule> rules);

        /// <summary>
        /// <para>Shows the rule editor view/control, optionally pre-filled with a rule for editing.</para>
        /// <para>Hiển thị view/control soạn thảo quy tắc, có thể được điền sẵn một quy tắc để chỉnh sửa.</para>
        /// </summary>
        /// <param name="ruleToEdit">The rule to edit. If <c>null</c>, the editor is shown for adding a new rule.</param>
        void ShowRuleEditor(KeyMappingRule? ruleToEdit);

        /// <summary>
        /// <para>Shows the application settings view/control.</para>
        /// <para>Hiển thị view/control cài đặt ứng dụng.</para>
        /// </summary>
        /// <param name="currentSettings">The current application settings to display and edit. Expected to be non-null.</param>
        void ShowSettingsEditor(AppSettings currentSettings);

        /// <summary>
        /// <para>Displays a message to the user.</para>
        /// <para>Hiển thị một thông báo cho người dùng.</para>
        /// </summary>
        /// <param name="message">The message text.</param>
        /// <param name="caption">The caption for the message box.</param>
        /// <param name="icon">The icon to display in the message box.</param>
        void ShowUserNotification(string message, string caption, MessageBoxIcon icon);

        /// <summary>
        /// <para>Clears any existing rule selection in the UI.</para>
        /// <para>Xóa lựa chọn quy tắc hiện có trong UI.</para>
        /// </summary>
        void ClearRuleSelection();

        /// <summary>
        /// Shows the home control/view.
        /// </summary>
        void ShowHomeControl(); // Giữ lại phương thức này để MainController có thể gọi nếu cần

        void ShowRuleListView();
        #endregion

        #region Events triggered by UI actions
        /// <summary>
        /// <para>Occurs when the main form is loaded and ready for the controller to initialize data.</para>
        /// <para>Xảy ra khi form chính được tải và sẵn sàng để controller khởi tạo dữ liệu.</para>
        /// </summary>
        event EventHandler? ViewLoaded;

        /// <summary>
        /// <para>Occurs when the user requests to toggle the enabled state of the key mapping service.</para>
        /// <para>Xảy ra khi người dùng yêu cầu bật/tắt trạng thái của dịch vụ ánh xạ phím.</para>
        /// </summary>
        event EventHandler? ToggleServiceStatusClicked;

        /// <summary>
        /// <para>Occurs when the user requests to add a new key mapping rule.</para>
        /// <para>Xảy ra khi người dùng yêu cầu thêm một quy tắc ánh xạ phím mới.</para>
        /// </summary>
        event EventHandler? AddRuleClicked;

        /// <summary>
        /// <para>Occurs when the user requests to edit the currently selected key mapping rule.</para>
        /// <para>Xảy ra khi người dùng yêu cầu chỉnh sửa quy tắc ánh xạ phím hiện đang được chọn.</para>
        /// </summary>
        event EventHandler? EditRuleClicked;

        /// <summary>
        /// <para>Occurs when the user requests to delete the currently selected key mapping rule.</para>
        /// <para>Xảy ra khi người dùng yêu cầu xóa quy tắc ánh xạ phím hiện đang được chọn.</para>
        /// </summary>
        event EventHandler? DeleteRuleClicked;

        /// <summary>
        /// <para>Occurs when the user requests to open the application settings view.</para>
        /// <para>Xảy ra khi người dùng yêu cầu mở view cài đặt ứng dụng.</para>
        /// </summary>
        event EventHandler? OpenSettingsClicked;

        /// <summary>
        /// <para>Occurs when the main form is about to be closed.</para>
        /// <para>Xảy ra khi form chính sắp được đóng.</para>
        /// </summary>
        event CancelEventHandler? ViewClosing;
        #endregion
    }
}