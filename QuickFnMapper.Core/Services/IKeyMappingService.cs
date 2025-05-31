#region Using Directives
using System;
using System.Collections.Generic;
using QuickFnMapper.Core.Models;
// Không cần using QuickFnMapper.Core.Services; ở đây nếu NotificationEventArgs cùng namespace (đã là Core.Services)
#endregion

namespace QuickFnMapper.Core.Services
{
    /// <summary>
    /// <para>Defines the contract for a service that manages key mapping rules.</para>
    /// <para>Định nghĩa hợp đồng cho một dịch vụ quản lý các quy tắc ánh xạ phím.</para>
    /// </summary>
    public interface IKeyMappingService
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
        /// <param name="id">The unique identifier of the rule.</param>
        /// <returns>The <see cref="KeyMappingRule"/> if found; otherwise, <c>null</c>.</returns>
        KeyMappingRule? GetRuleById(Guid id);

        /// <summary>
        /// <para>Adds a new key mapping rule to the collection.</para>
        /// <para>Thêm một quy tắc ánh xạ phím mới vào bộ sưu tập.</para>
        /// </summary>
        /// <param name="rule">The <see cref="KeyMappingRule"/> to add. Expected to be non-null.</param>
        void AddRule(KeyMappingRule rule);

        /// <summary>
        /// <para>Updates an existing key mapping rule in the collection.</para>
        /// <para>Cập nhật một quy tắc ánh xạ phím hiện có trong bộ sưu tập.</para>
        /// </summary>
        /// <param name="rule">The <see cref="KeyMappingRule"/> to update. Expected to be non-null.</param>
        void UpdateRule(KeyMappingRule rule);

        /// <summary>
        /// <para>Deletes a key mapping rule from the collection by its unique identifier.</para>
        /// <para>Xóa một quy tắc ánh xạ phím khỏi bộ sưu tập bằng mã định danh duy nhất của nó.</para>
        /// </summary>
        /// <param name="id">The unique identifier of the rule to delete.</param>
        void DeleteRule(Guid id);

        /// <summary>
        /// Occurs when the service needs to display a notification to the user
        /// (e.g., after a rule is executed or an error occurs during execution).
        /// </summary>
        event EventHandler<NotificationEventArgs>? NotificationRequested;
    }
}