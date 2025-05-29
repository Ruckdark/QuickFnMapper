#region Using Directives
// Đại ca kiểm tra lại dòng using này nhé, có thể là:
// using QuickFnMapper.Core; // Nếu KeyCodeHelper ở namespace QuickFnMapper.Core
// Hoặc không cần nếu KeyCodeHelper cùng namespace Models, hoặc sẽ using cụ thể khi dùng.
using QuickFnMapper.QuickFnMapper.Core; // Giữ nguyên theo code Đại ca gửi, Đại ca xem lại sau ạ
using System;
using System.Windows.Forms; // Để sử dụng enum Keys cho OriginalKeyData
#endregion

namespace QuickFnMapper.Core.Models
{
    #region Enums

    /// <summary>
    /// <para>Defines the types of actions that can be performed by a key mapping rule.</para>
    /// <para>Định nghĩa các loại hành động có thể được thực hiện bởi một quy tắc ánh xạ phím.</para>
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// <para>No action defined.</para>
        /// <para>Không có hành động nào được định nghĩa.</para>
        /// </summary>
        None,

        /// <summary>
        /// <para>Sends a media key command (e.g., Volume Up/Down, Mute, Play/Pause).</para>
        /// <para>Gửi một lệnh media (VD: Tăng/Giảm âm lượng, Tắt tiếng, Phát/Tạm dừng).</para>
        /// </summary>
        SendMediaKey,

        /// <summary>
        /// <para>Runs an application.</para>
        /// <para>Chạy một ứng dụng.</para>
        /// </summary>
        RunApplication,

        /// <summary>
        /// <para>Opens a URL in the default web browser.</para>
        /// <para>Mở một địa chỉ web trong trình duyệt mặc định.</para>
        /// </summary>
        OpenUrl,

        /// <summary>
        /// <para>Sends a predefined string of text.</para>
        /// <para>Gửi một đoạn văn bản được định nghĩa trước.</para>
        /// </summary>
        SendText,

        /// <summary>
        /// <para>Triggers another defined hotkey or a system command.</para>
        /// <para>Kích hoạt một phím nóng đã định nghĩa khác hoặc một lệnh hệ thống.</para>
        /// </summary>
        TriggerHotkeyOrCommand
        // Future action types can be added here / Các loại hành động trong tương lai có thể được thêm vào đây
    }

    #endregion

    #region Data Structures for KeyMappingRule

    /// <summary>
    /// <para>Represents the original key combination that triggers a mapping rule.</para>
    /// <para>Đại diện cho tổ hợp phím gốc kích hoạt một quy tắc ánh xạ.</para>
    /// </summary>
    public class OriginalKeyData
    {
        #region Properties
        /// <summary>
        /// <para>Gets or sets the primary key that was pressed.</para>
        /// <para>Lấy hoặc đặt phím chính được nhấn.</para>
        /// </summary>
        public Keys Key { get; set; }

        /// <summary>
        /// <para>Gets or sets a value indicating whether the Control key was pressed as part of the combination.</para>
        /// <para>Lấy hoặc đặt một giá trị cho biết phím Control có được nhấn cùng hay không.</para>
        /// </summary>
        public bool Ctrl { get; set; }

        /// <summary>
        /// <para>Gets or sets a value indicating whether the Shift key was pressed as part of the combination.</para>
        /// <para>Lấy hoặc đặt một giá trị cho biết phím Shift có được nhấn cùng hay không.</para>
        /// </summary>
        public bool Shift { get; set; }

        /// <summary>
        /// <para>Gets or sets a value indicating whether the Alt key was pressed as part of the combination.</para>
        /// <para>Lấy hoặc đặt một giá trị cho biết phím Alt có được nhấn cùng hay không.</para>
        /// </summary>
        public bool Alt { get; set; }

        /// <summary>
        /// <para>Gets or sets a value indicating whether the Windows key was pressed as part of the combination.</para>
        /// <para>Lấy hoặc đặt một giá trị cho biết phím Windows có được nhấn cùng hay không.</para>
        /// </summary>
        public bool Win { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="OriginalKeyData"/> class.</para>
        /// <para>Khởi tạo một đối tượng mới của lớp <see cref="OriginalKeyData"/>.</para>
        /// </summary>
        public OriginalKeyData()
        {
            Key = Keys.None; // Default to no key / Mặc định không có phím nào
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="OriginalKeyData"/> class with specified key and modifiers.</para>
        /// <para>Khởi tạo một đối tượng mới của lớp <see cref="OriginalKeyData"/> với phím và các phím bổ trợ được chỉ định.</para>
        /// </summary>
        /// <param name="key">
        /// <para>The primary key.</para>
        /// <para>Phím chính.</para>
        /// </param>
        /// <param name="ctrl">
        /// <para>Control key pressed.</para>
        /// <para>Phím Control được nhấn.</para>
        /// </param>
        /// <param name="shift">
        /// <para>Shift key pressed.</para>
        /// <para>Phím Shift được nhấn.</para>
        /// </param>
        /// <param name="alt">
        /// <para>Alt key pressed.</para>
        /// <para>Phím Alt được nhấn.</para>
        /// </param>
        /// <param name="win">
        /// <para>Windows key pressed.</para>
        /// <para>Phím Windows được nhấn.</para>
        /// </param>
        public OriginalKeyData(Keys key, bool ctrl = false, bool shift = false, bool alt = false, bool win = false)
        {
            Key = key;
            Ctrl = ctrl;
            Shift = shift;
            Alt = alt;
            Win = win;
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// <para>Returns a string representation of the original key combination.</para>
        /// <para>Trả về một chuỗi đại diện cho tổ hợp phím gốc.</para>
        /// </summary>
        /// <returns>
        /// <para>A string representing the key combination.</para>
        /// <para>Một chuỗi đại diện cho tổ hợp phím.</para>
        /// </returns>
        public override string ToString()
        {
            string representation = "";
            if (Ctrl) representation += "Ctrl + ";
            if (Alt) representation += "Alt + ";
            if (Shift) representation += "Shift + ";
            if (Win) representation += "Win + ";
            representation += KeyCodeHelper.GetKeyName(Key); // Assuming KeyCodeHelper.GetKeyName exists / Giả sử KeyCodeHelper.GetKeyName tồn tại
            return representation;
        }
        #endregion
    }

    /// <summary>
    /// <para>Represents the target action to be performed when a key mapping rule is triggered.</para>
    /// <para>Đại diện cho hành động đích sẽ được thực hiện khi một quy tắc ánh xạ phím được kích hoạt.</para>
    /// </summary>
    public class TargetAction
    {
        #region Properties
        /// <summary>
        /// <para>Gets or sets the type of action to perform.</para>
        /// <para>Lấy hoặc đặt loại hành động sẽ thực hiện.</para>
        /// </summary>
        public ActionType Type { get; set; }

        /// <summary>
        /// <para>Gets or sets the parameter associated with the action.</para>
        /// <para>The meaning of this parameter depends on the <see cref="ActionType"/>.</para>
        /// <para>Lấy hoặc đặt tham số liên quan đến hành động.</para>
        /// <para>Ý nghĩa của tham số này phụ thuộc vào <see cref="ActionType"/>.</para>
        /// </summary>
        /// <example>
        /// <para>If Type is SendMediaKey, ActionParameter could be "VolumeDown", "VK_VOLUME_MUTE", etc.</para>
        /// <para>If Type is RunApplication, ActionParameter is the path to the executable.</para>
        /// <para>If Type is OpenUrl, ActionParameter is the URL string.</para>
        /// </example>
        public string ActionParameter { get; set; }

        /// <summary>
        /// <para>Gets or sets an optional secondary parameter for more complex actions.</para>
        /// <para>Lấy hoặc đặt một tham số phụ tùy chọn cho các hành động phức tạp hơn.</para>
        /// </summary>
        public string ActionParameterSecondary { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="TargetAction"/> class.</para>
        /// <para>Khởi tạo một đối tượng mới của lớp <see cref="TargetAction"/>.</para>
        /// </summary>
        public TargetAction()
        {
            Type = ActionType.None;
            ActionParameter = string.Empty;
            ActionParameterSecondary = string.Empty;
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="TargetAction"/> class with specified type and parameter.</para>
        /// <para>Khởi tạo một đối tượng mới của lớp <see cref="TargetAction"/> với loại và tham số được chỉ định.</para>
        /// </summary>
        /// <param name="type">
        /// <para>The type of action.</para>
        /// <para>Loại hành động.</para>
        /// </param>
        /// <param name="parameter">
        /// <para>The action parameter.</para>
        /// <para>Tham số hành động.</para>
        /// </param>
        /// <param name="secondaryParameter">
        /// <para>The optional secondary action parameter.</para>
        /// <para>Tham số hành động phụ tùy chọn.</para>
        /// </param>
        public TargetAction(ActionType type, string parameter, string secondaryParameter = "")
        {
            Type = type;
            ActionParameter = parameter;
            ActionParameterSecondary = secondaryParameter ?? string.Empty;
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// <para>Returns a string representation of the target action.</para>
        /// <para>Trả về một chuỗi đại diện cho hành động đích.</para>
        /// </summary>
        /// <returns>
        /// <para>A string representing the action.</para>
        /// <para>Một chuỗi đại diện cho hành động.</para>
        /// </returns>
        public override string ToString()
        {
            if (Type == ActionType.None)
                return "No Action";
            return $"{Type}: {ActionParameter}" + (!string.IsNullOrEmpty(ActionParameterSecondary) ? $" ({ActionParameterSecondary})" : "");
        }
        #endregion
    }

    #endregion

    #region Main KeyMappingRule Class

    /// <summary>
    /// <para>Represents a single key mapping rule that defines an original key combination</para>
    /// <para>and a target action to be performed when that combination is detected.</para>
    /// <para>Đại diện cho một quy tắc ánh xạ phím đơn lẻ, định nghĩa một tổ hợp phím gốc</para>
    /// <para>và một hành động đích sẽ được thực hiện khi tổ hợp đó được phát hiện.</para>
    /// </summary>
    public class KeyMappingRule
    {
        #region Properties

        /// <summary>
        /// <para>Gets or sets the unique identifier for this rule.</para>
        /// <para>Lấy hoặc đặt mã định danh duy nhất cho quy tắc này.</para>
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// <para>Gets or sets the user-friendly name or description for this rule.</para>
        /// <para>Lấy hoặc đặt tên hoặc mô tả thân thiện với người dùng cho quy tắc này.</para>
        /// </summary>
        /// <example>
        /// <para>"F2 to Volume Down", "Ctrl+Shift+N for Notepad"</para>
        /// </example>
        public string Name { get; set; }

        /// <summary>
        /// <para>Gets or sets a value indicating whether this rule is currently active and should be processed.</para>
        /// <para>Lấy hoặc đặt một giá trị cho biết quy tắc này có đang hoạt động và nên được xử lý hay không.</para>
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// <para>Gets or sets the original key combination that triggers this rule.</para>
        /// <para>Lấy hoặc đặt tổ hợp phím gốc kích hoạt quy tắc này.</para>
        /// </summary>
        public OriginalKeyData OriginalKey { get; set; }

        /// <summary>
        /// <para>Gets or sets the target action to be performed when this rule is triggered.</para>
        /// <para>Lấy hoặc đặt hành động đích sẽ được thực hiện khi quy tắc này được kích hoạt.</para>
        /// </summary>
        public TargetAction TargetActionDetails { get; set; }

        /// <summary>
        /// <para>Gets or sets an optional order this rule should be considered if multiple rules match. Lower numbers processed first.</para>
        /// <para>(Advanced) Lấy hoặc đặt thứ tự tùy chọn mà quy tắc này nên được xem xét nếu nhiều quy tắc khớp. Số thấp hơn được xử lý trước.</para>
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// <para>Gets or sets the date and time when this rule was created.</para>
        /// <para>Lấy hoặc đặt ngày và giờ quy tắc này được tạo.</para>
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// <para>Gets or sets the date and time when this rule was last modified.</para>
        /// <para>Lấy hoặc đặt ngày và giờ quy tắc này được sửa đổi lần cuối.</para>
        /// </summary>
        public DateTime LastModifiedDate { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="KeyMappingRule"/> class with default values.</para>
        /// <para>Khởi tạo một đối tượng mới của lớp <see cref="KeyMappingRule"/> với các giá trị mặc định.</para>
        /// </summary>
        public KeyMappingRule()
        {
            Id = Guid.NewGuid();
            Name = "New Rule";
            IsEnabled = true;
            OriginalKey = new OriginalKeyData();
            TargetActionDetails = new TargetAction();
            Order = 0;
            CreatedDate = DateTime.UtcNow;
            LastModifiedDate = DateTime.UtcNow;
        }

        #endregion

        #region Overridden Methods
        /// <summary>
        /// <para>Returns a string representation of the key mapping rule, typically its name.</para>
        /// <para>Trả về một chuỗi đại diện cho quy tắc ánh xạ phím, thường là tên của nó.</para>
        /// </summary>
        /// <returns>
        /// <para>The name of the rule.</para>
        /// <para>Tên của quy tắc.</para>
        /// </returns>
        public override string ToString()
        {
            return Name ?? $"Rule ({Id})";
        }
        #endregion
    }

    #endregion
}