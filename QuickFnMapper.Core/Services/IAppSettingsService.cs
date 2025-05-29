#region Using Directives
using QuickFnMapper.Core.Models; // Để sử dụng AppSettings
#endregion

namespace QuickFnMapper.Core.Services
{
    /// <summary>
    /// <para>Defines the contract for a service that manages application settings.</para>
    /// <para>Định nghĩa hợp đồng cho một dịch vụ quản lý cài đặt ứng dụng.</para>
    /// </summary>
    public interface IAppSettingsService
    {
        /// <summary>
        /// <para>Loads the application settings from a persistent storage.</para>
        /// <para>Tải cài đặt ứng dụng từ một bộ lưu trữ bền vững.</para>
        /// </summary>
        /// <returns>
        /// <para>An <see cref="AppSettings"/> object containing the loaded settings. Returns default settings if loading fails or no settings are found.</para>
        /// <para>Một đối tượng <see cref="AppSettings"/> (luôn non-null) chứa các cài đặt đã tải. Trả về cài đặt mặc định nếu việc tải thất bại hoặc không tìm thấy cài đặt nào.</para>
        /// </returns>
        AppSettings LoadSettings();

        /// <summary>
        /// <para>Saves the provided application settings to a persistent storage.</para>
        /// <para>Lưu các cài đặt ứng dụng được cung cấp vào một bộ lưu trữ bền vững.</para>
        /// </summary>
        /// <param name="settings">
        /// <para>The <see cref="AppSettings"/> object to save. This parameter is expected to be non-null.</para>
        /// <para>Đối tượng <see cref="AppSettings"/> cần lưu. Tham số này được mong đợi là không null.</para>
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para>Thrown by implementations if <paramref name="settings"/> is null.</para>
        /// <para>Có thể được ném bởi các lớp triển khai nếu <paramref name="settings"/> là null.</para>
        /// </exception>
        void SaveSettings(AppSettings settings);

        /// <summary>
        /// <para>Gets the default application settings.</para>
        /// <para>Lấy các cài đặt ứng dụng mặc định.</para>
        /// </summary>
        /// <returns>
        /// <para>A new instance of <see cref="AppSettings"/> (non-null) with default values.</para>
        /// <para>Một đối tượng mới của <see cref="AppSettings"/> (không null) với các giá trị mặc định.</para>
        /// </returns>
        AppSettings GetDefaultSettings();
    }
}