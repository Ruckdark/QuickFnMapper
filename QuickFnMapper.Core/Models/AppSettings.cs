#region Using Directives
using System;
#endregion

namespace QuickFnMapper.Core.Models
{
    /// <summary>
    /// <para>Represents the application-wide settings for QuickFnMapper.</para>
    /// <para>Đại diện cho các cài đặt toàn cục của ứng dụng QuickFnMapper.</para>
    /// </summary>
    public class AppSettings
    {
        #region Properties

        /// <summary>
        /// <para>Gets or sets a value indicating whether the application should start automatically when Windows starts.</para>
        /// <para>Lấy hoặc đặt một giá trị cho biết ứng dụng có nên tự động khởi động cùng Windows hay không.</para>
        /// </summary>
        public bool StartWithWindows { get; set; }

        /// <summary>
        /// <para>Gets or sets a value indicating whether the global key hook (and thus all key mappings) is currently enabled.</para>
        /// <para>Lấy hoặc đặt một giá trị cho biết chức năng hook phím toàn cục (và do đó tất cả các ánh xạ phím) có đang được kích hoạt hay không.</para>
        /// <para>Khi False, không có quy tắc nào được xử lý.</para>
        /// </summary>
        public bool IsGlobalHookEnabled { get; set; }

        /// <summary>
        /// <para>Gets or sets a value indicating whether the application should minimize to the system tray when the close button (X) is clicked, instead of exiting the application.</para>
        /// <para>Lấy hoặc đặt một giá trị cho biết ứng dụng có nên thu nhỏ xuống khay hệ thống khi nút đóng (X) được nhấp, thay vì thoát hoàn toàn ứng dụng.</para>
        /// </summary>
        public bool MinimizeToTrayOnClose { get; set; }

        /// <summary>
        /// <para>Gets or sets a value indicating whether to show notifications (e.g., toast notifications) when a key mapping rule is triggered and an action is performed.</para>
        /// <para>Lấy hoặc đặt một giá trị cho biết có hiển thị thông báo (ví dụ: thông báo nhanh) khi một quy tắc ánh xạ phím được kích hoạt và một hành động được thực hiện hay không.</para>
        /// </summary>
        public bool ShowNotifications { get; set; }

        /// <summary>
        /// <para>Gets or sets the path to the file where key mapping rules are stored.</para>
        /// <para>(Consider if this should be configurable or a fixed location)</para>
        /// <para>Lấy hoặc đặt đường dẫn đến tệp nơi các quy tắc ánh xạ phím được lưu trữ.</para>
        /// <para>(Cân nhắc xem điều này có nên được cấu hình hay là một vị trí cố định)</para>
        /// </summary>
        public string RulesFilePath { get; set; }

        /// <summary>
        /// <para>Gets or sets the theme for the application UI (e.g., "Light", "Dark", "SystemDefault").</para>
        /// <para>(Optional - for future UI enhancements)</para>
        /// <para>Lấy hoặc đặt chủ đề cho giao diện người dùng của ứng dụng (ví dụ: "Sáng", "Tối", "Mặc định hệ thống").</para>
        /// <para>(Tùy chọn - cho các cải tiến UI trong tương lai)</para>
        /// </summary>
        public string ApplicationTheme { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="AppSettings"/> class with default values.</para>
        /// <para>Khởi tạo một đối tượng mới của lớp <see cref="AppSettings"/> với các giá trị mặc định.</para>
        /// </summary>
        public AppSettings()
        {
            // Set default values / Đặt các giá trị mặc định
            StartWithWindows = false;
            IsGlobalHookEnabled = true;      // Mặc định là bật để người dùng thấy hiệu quả ngay
            MinimizeToTrayOnClose = true;  // Hành vi phổ biến cho ứng dụng tiện ích chạy ngầm
            ShowNotifications = true;        // Thông báo cho người dùng biết khi quy tắc được kích hoạt

            // Default path for rules file - consider placing in AppData
            // Đường dẫn mặc định cho tệp quy tắc - cân nhắc đặt trong AppData
            RulesFilePath = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), // Thư mục AppData\Roaming của người dùng
                "QuickFnMapper",                                                      // Tên thư mục ứng dụng
                "KeyMappings.json"                                                    // Tên tệp cấu hình (ví dụ: JSON)
            );

            ApplicationTheme = "SystemDefault"; // Mặc định theo hệ thống
        }

        #endregion

        #region Methods
        // <summary>
        // <para>Placeholder for any methods related to AppSettings, if needed in the future.</para>
        // <para>Ví dụ: ValidateSettings(), ResetToDefaults(), etc.</para>
        // <para>Chỗ dành sẵn cho bất kỳ phương thức nào liên quan đến AppSettings, nếu cần trong tương lai.</para>
        // <para>Ví dụ: ValidateSettings(), ResetToDefaults(), v.v...</para>
        // </summary>
        #endregion
    }
}