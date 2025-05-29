#region Using Directives
using System;
using System.Collections.Generic;
using QuickFnMapper.Core.Models; // Để sử dụng KeyMappingRule
#endregion

namespace QuickFnMapper.Core.Services
{
    /// <summary>
    /// <para>Defines the contract for a service that manages key mapping rules.</para>
    /// <para>Định nghĩa hợp đồng cho một dịch vụ quản lý các quy tắc ánh xạ phím.</para>
    /// </summary>
    public interface IKeyMappingService // Cân nhắc implement IDisposable nếu service này cần giải phóng tài nguyên trực tiếp (hiện tại nó phụ thuộc IGlobalHookService đã IDisposable)
    {
        /// <summary>
        /// <para>Gets a value indicating whether the key mapping service is currently enabled and processing key events.</para>
        /// <para>Lấy một giá trị cho biết liệu dịch vụ ánh xạ phím có đang được kích hoạt và xử lý các sự kiện phím hay không.</para>
        /// </summary>
        bool IsServiceEnabled { get; }

        /// <summary>
        /// <para>Enables the key mapping service to start listening for global key events and processing rules.</para>
        /// <para>Kích hoạt dịch vụ ánh xạ phím để bắt đầu lắng nghe các sự kiện phím toàn cục và xử lý quy tắc.</para>
        /// </summary>
        void EnableService();

        /// <summary>
        /// <para>Disables the key mapping service, stopping it from listening to global key events.</para>
        /// <para>Vô hiệu hóa dịch vụ ánh xạ phím, dừng việc lắng nghe các sự kiện phím toàn cục.</para>
        /// </summary>
        void DisableService();

        /// <summary>
        /// <para>Loads all key mapping rules from persistent storage.</para>
        /// <para>Tải tất cả các quy tắc ánh xạ phím từ bộ lưu trữ bền vững.</para>
        /// <para>This should be called, for example, at application startup or when settings indicate a change in rules file path.</para>
        /// <para>Phương thức này nên được gọi, ví dụ, khi ứng dụng khởi động hoặc khi cài đặt chỉ ra sự thay đổi đường dẫn tệp quy tắc.</para>
        /// </summary>
        void LoadRules();

        /// <summary>
        /// <para>Saves all current key mapping rules to persistent storage.</para>
        /// <para>Lưu tất cả các quy tắc ánh xạ phím hiện tại vào bộ lưu trữ bền vững.</para>
        /// </summary>
        void SaveRules();

        /// <summary>
        /// <para>Gets all currently loaded key mapping rules.</para>
        /// <para>Lấy tất cả các quy tắc ánh xạ phím hiện đã được tải.</para>
        /// </summary>
        /// <returns>
        /// <para>An enumerable collection of <see cref="KeyMappingRule"/> (non-null, but can be empty).</para>
        /// <para>Một bộ sưu tập có thể liệt kê của <see cref="KeyMappingRule"/> (không null, nhưng có thể rỗng).</para>
        /// </returns>
        IEnumerable<KeyMappingRule> GetAllRules();

        /// <summary>
        /// <para>Gets a specific key mapping rule by its unique identifier.</para>
        /// <para>Lấy một quy tắc ánh xạ phím cụ thể bằng mã định danh duy nhất của nó.</para>
        /// </summary>
        /// <param name="id">
        /// <para>The unique identifier of the rule.</para>
        /// <para>Mã định danh duy nhất của quy tắc.</para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="KeyMappingRule"/> if found; otherwise, <c>null</c>.</para>
        /// <para><see cref="KeyMappingRule"/> nếu tìm thấy; ngược lại, <c>null</c>.</para>
        /// </returns>
        KeyMappingRule? GetRuleById(Guid id); // Sửa: Kiểu trả về là nullable 'KeyMappingRule?'

        /// <summary>
        /// <para>Adds a new key mapping rule to the collection.</para>
        /// <para>Thêm một quy tắc ánh xạ phím mới vào bộ sưu tập.</para>
        /// <para>Implementations should typically call <see cref="SaveRules"/> afterwards or provide a mechanism for batch saving.</para>
        /// <para>Các lớp triển khai thường nên gọi <see cref="SaveRules"/> sau đó hoặc cung cấp một cơ chế để lưu hàng loạt.</para>
        /// </summary>
        /// <param name="rule">
        /// <para>The <see cref="KeyMappingRule"/> to add. This parameter is expected to be non-null.</para>
        /// <para>Quy tắc <see cref="KeyMappingRule"/> cần thêm. Tham số này được mong đợi là không null.</para>
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para>Thrown by implementations if <paramref name="rule"/> is null.</para>
        /// <para>Có thể được ném bởi các lớp triển khai nếu <paramref name="rule"/> là null.</para>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <para>Thrown by implementations if a rule with the same ID already exists.</para>
        /// <para>Có thể được ném bởi các lớp triển khai nếu một quy tắc với cùng ID đã tồn tại.</para>
        /// </exception>
        void AddRule(KeyMappingRule rule);

        /// <summary>
        /// <para>Updates an existing key mapping rule in the collection.</para>
        /// <para>Cập nhật một quy tắc ánh xạ phím hiện có trong bộ sưu tập.</para>
        /// <para>Implementations should typically call <see cref="SaveRules"/> afterwards.</para>
        /// <para>Các lớp triển khai thường nên gọi <see cref="SaveRules"/> sau đó.</para>
        /// </summary>
        /// <param name="rule">
        /// <para>The <see cref="KeyMappingRule"/> to update, identified by its Id. This parameter is expected to be non-null.</para>
        /// <para>Quy tắc <see cref="KeyMappingRule"/> cần cập nhật, được xác định bằng Id của nó. Tham số này được mong đợi là không null.</para>
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para>Thrown by implementations if <paramref name="rule"/> is null.</para>
        /// <para>Có thể được ném bởi các lớp triển khai nếu <paramref name="rule"/> là null.</para>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <para>Thrown by implementations if the rule to update is not found.</para>
        /// <para>Có thể được ném bởi các lớp triển khai nếu quy tắc cần cập nhật không được tìm thấy.</para>
        /// </exception>
        void UpdateRule(KeyMappingRule rule);

        /// <summary>
        /// <para>Deletes a key mapping rule from the collection by its unique identifier.</para>
        /// <para>Xóa một quy tắc ánh xạ phím khỏi bộ sưu tập bằng mã định danh duy nhất của nó.</para>
        /// <para>Implementations should typically call <see cref="SaveRules"/> afterwards.</para>
        /// <para>Các lớp triển khai thường nên gọi <see cref="SaveRules"/> sau đó.</para>
        /// </summary>
        /// <param name="id">
        /// <para>The unique identifier of the rule to delete.</para>
        /// <para>Mã định danh duy nhất của quy tắc cần xóa.</para>
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// <para>Thrown by implementations if the rule to delete is not found.</para>
        /// <para>Có thể được ném bởi các lớp triển khai nếu quy tắc cần xóa không được tìm thấy.</para>
        /// </exception>
        void DeleteRule(Guid id);
    }
}