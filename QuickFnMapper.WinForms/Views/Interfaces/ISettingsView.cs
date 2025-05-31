#region Using Directives
using QuickFnMapper.Core.Models;
using System;
using System.Collections.Generic; // For IEnumerable 
using System.Windows.Forms;
#endregion

namespace QuickFnMapper.WinForms.Views.Interfaces
{
    /// <summary>
    /// <para>Defines the contract for the application settings view (SettingsControl).</para>
    /// <para>Định nghĩa hợp đồng cho view cài đặt ứng dụng (SettingsControl).</para>
    /// </summary>
    public interface ISettingsView
    {
        #region Properties to Get/Set Settings Data from UI
        /// <summary>
        /// <para>Gets or sets a value indicating whether the application should start with Windows.</para>
        /// <para>Lấy hoặc đặt một giá trị cho biết ứng dụng có nên khởi động cùng Windows hay không.</para>
        /// </summary>
        bool StartWithWindows { get; set; } // 

        /// <summary>
        /// <para>Gets or sets a value indicating whether the global key hook is enabled (as presented in the UI).</para>
        /// <para>Lấy hoặc đặt một giá trị cho biết liệu hook phím toàn cục có được kích hoạt hay không (như được hiển thị trong UI).</para>
        /// </summary>
        bool IsGlobalHookEnabledView { get; set; } // 

        /// <summary>
        /// <para>Gets or sets a value indicating whether the application should minimize to the system tray on close.</para>
        /// <para>Lấy hoặc đặt một giá trị cho biết ứng dụng có nên thu nhỏ xuống khay hệ thống khi đóng hay không.</para>
        /// </summary>
        bool MinimizeToTrayOnClose { get; set; } // 

        /// <summary>
        /// <para>Gets or sets a value indicating whether to show notifications.</para>
        /// <para>Lấy hoặc đặt một giá trị cho biết có hiển thị thông báo hay không.</para>
        /// </summary>
        bool ShowNotifications { get; set; } // 

        /// <summary>
        /// <para>Gets or sets the file path for storing key mapping rules (as presented in the UI).</para>
        /// <para>Lấy hoặc đặt đường dẫn tệp để lưu trữ các quy tắc ánh xạ phím (như được hiển thị trong UI).</para>
        /// </summary>
        string RulesFilePathView { get; set; } // 

        /// <summary>
        /// <para>Gets or sets the selected application theme (as presented in the UI).</para>
        /// <para>Lấy hoặc đặt chủ đề ứng dụng được chọn (như được hiển thị trong UI).</para>
        /// </summary>
        string ApplicationThemeView { get; set; } // 
        #endregion

        #region Methods to Update UI
        /// <summary>
        /// <para>Populates the UI with the available application themes.</para>
        /// <para>Điền vào UI các chủ đề ứng dụng có sẵn.</para>
        /// </summary>
        void PopulateApplicationThemes(IEnumerable<string> themes); // 
        /// <summary>
        /// <para>Displays a message to the user.</para>
        /// <para>Hiển thị một thông báo cho người dùng.</para>
        /// </summary>
        void ShowUserNotification(string message, string caption, MessageBoxIcon icon); // 
        /// <summary>
        /// <para>Closes the settings view.</para>
        /// <para>Đóng view cài đặt.</para>
        /// </summary>
        void CloseView(bool success); // 
        #endregion

        #region Events triggered by UI actions
        // Event ViewInitialized không còn cần thiết nếu controller chủ động populate view
        // event EventHandler? ViewInitialized; // 

        /// <summary>
        /// <para>Occurs when the user clicks the save (or apply) button for settings.</para>
        /// <para>Xảy ra khi người dùng nhấp vào nút lưu (hoặc áp dụng) cho cài đặt.</para>
        /// </summary>
        event EventHandler? SaveSettingsClicked; // 

        /// <summary>
        /// <para>Occurs when the user clicks the cancel (or close) button without saving.</para>
        /// <para>Xảy ra khi người dùng nhấp vào nút hủy (hoặc đóng) mà không lưu.</para>
        /// </summary>
        event EventHandler? CancelSettingsClicked; // 

        /// <summary>
        /// <para>Occurs when the user requests to browse for a new rules file path.</para>
        /// <para>Xảy ra khi người dùng yêu cầu duyệt tìm một đường dẫn tệp quy tắc mới.</para>
        /// </summary>
        event EventHandler? BrowseRulesFilePathClicked; // 
        #endregion
    }
}