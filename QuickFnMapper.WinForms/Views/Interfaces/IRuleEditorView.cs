// File: QuickFnMapper.WinForms/Views/Interfaces/IRuleEditorView.cs
#region Using Directives
using QuickFnMapper.Core.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
#endregion

namespace QuickFnMapper.WinForms.Views.Interfaces
{
    /// <summary>
    /// <para>Defines the contract for the rule editor view (RuleEditorControl).</para>
    /// <para>Định nghĩa hợp đồng cho view soạn thảo quy tắc (RuleEditorControl).</para>
    /// </summary>
    public interface IRuleEditorView
    {
        #region Properties to Get/Set Rule Data from UI
        /// <summary>
        /// <para>Gets or sets the name of the rule.</para>
        /// <para>Lấy hoặc đặt tên của quy tắc.</para>
        /// </summary>
        string RuleName { get; set; }

        /// <summary>
        /// <para>Gets or sets a value indicating whether the rule is enabled.</para>
        /// <para>Lấy hoặc đặt một giá trị cho biết quy tắc có được kích hoạt hay không.</para>
        /// </summary>
        bool IsRuleEnabled { get; set; }

        /// <summary>
        /// <para>Gets or sets the original key combination selected/entered by the user.</para>
        /// <para>Lấy hoặc đặt tổ hợp phím gốc được người dùng chọn/nhập.</para>
        /// </summary>
        OriginalKeyData SelectedOriginalKey { get; set; }

        /// <summary>
        /// <para>Gets or sets the selected action type for the rule.</para>
        /// <para>Lấy hoặc đặt loại hành động được chọn cho quy tắc.</para>
        /// </summary>
        ActionType SelectedActionType { get; set; }

        /// <summary>
        /// <para>Gets or sets the primary parameter for the selected action.</para>
        /// <para>Lấy hoặc đặt tham số chính cho hành động được chọn.</para>
        /// </summary>
        string ActionParameterValue { get; set; }

        /// <summary>
        /// <para>Gets or sets the secondary parameter for the selected action (if applicable).</para>
        /// <para>Lấy hoặc đặt tham số phụ cho hành động được chọn (nếu có).</para>
        /// </summary>
        string? ActionParameterSecondaryValue { get; set; } // Cho phép nullable

        /// <summary>
        /// <para>Gets or sets the order of the rule.</para>
        /// <para>Lấy hoặc đặt thứ tự của quy tắc.</para>
        /// </summary>
        int RuleOrder { get; set; }
        #endregion

        #region Methods to Update UI
        /// <summary>
        /// <para>Populates the UI with the available action types.</para>
        /// <para>Điền vào UI các loại hành động có sẵn.</para>
        /// </summary>
        void PopulateActionTypes(IEnumerable<ActionType> actionTypes);

        /// <summary>
        /// <para>Populates UI controls specific to selecting a media key parameter.</para>
        /// <para>Điền vào các control UI cụ thể để chọn tham số phím media.</para>
        /// </summary>
        void PopulateMediaKeyParameters(IEnumerable<string> mediaKeyNames);

        /// <summary>
        /// <para>Adjusts the visibility and state of UI controls used for action parameters based on the selected action type.</para>
        /// <para>Điều chỉnh khả năng hiển thị và trạng thái của các control UI dùng cho tham số hành động dựa trên loại hành động được chọn.</para>
        /// </summary>
        void ConfigureActionParameterControls(ActionType actionType);

        /// <summary>
        /// <para>Displays an error message to the user.</para>
        /// <para>Hiển thị một thông báo lỗi cho người dùng.</para>
        /// </summary>
        void ShowErrorMessage(string message, string caption);

        /// <summary>
        /// <para>Closes the rule editor view.</para>
        /// <para>Đóng view soạn thảo quy tắc.</para>
        /// </summary>
        void CloseView(bool success);
        #endregion

        #region Events triggered by UI actions
        /// <summary>
        /// <para>Occurs when the view is loaded and ready for initialization.</para>
        /// <para>Xảy ra khi view được tải và sẵn sàng để khởi tạo.</para>
        /// </summary>
        event EventHandler? ViewInitialized; // Sửa: Thêm '?'

        /// <summary>
        /// <para>Occurs when the user clicks the save button.</para>
        /// <para>Xảy ra khi người dùng nhấp vào nút lưu.</para>
        /// </summary>
        event EventHandler? SaveRuleClicked; // Sửa: Thêm '?'

        /// <summary>
        /// <para>Occurs when the user clicks the cancel button.</para>
        /// <para>Xảy ra khi người dùng nhấp vào nút hủy.</para>
        /// </summary>
        event EventHandler? CancelClicked; // Sửa: Thêm '?'

        /// <summary>
        /// <para>Occurs when the selected action type changes in the UI.</para>
        /// <para>Xảy ra khi loại hành động được chọn thay đổi trong UI.</para>
        /// </summary>
        event EventHandler<ActionType>? SelectedActionTypeChanged; // Sửa: Thêm '?'

        // CaptureOriginalKeyRequested không nhất thiết phải là event nếu View tự xử lý và cập nhật SelectedOriginalKey
        // Nếu Controller cần biết khi nào việc bắt phím xảy ra, thì mới cần event này.
        // event EventHandler? CaptureOriginalKeyRequested; 
        #endregion
    }
}