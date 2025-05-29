#region Using Directives
using System;
using QuickFnMapper.Core.Models; // Để sử dụng KeyEventArgsProcessed
#endregion

namespace QuickFnMapper.Core.Services
{
    /// <summary>
    /// <para>Defines the contract for a service that provides global keyboard hooking functionality.</para>
    /// <para>Định nghĩa hợp đồng cho một dịch vụ cung cấp chức năng hook bàn phím toàn cục.</para>
    /// </summary>
    public interface IGlobalHookService : IDisposable
    {
        /// <summary>
        /// <para>Occurs when a key is pressed down anywhere in the system and the hook is active.</para>
        /// <para>Xảy ra khi một phím được nhấn xuống ở bất kỳ đâu trong hệ thống và hook đang hoạt động.</para>
        /// <para>Subscribers can set the <see cref="KeyEventArgsProcessed.Handled"/> property to <c>true</c> to indicate that the key press has been processed and should not be passed to other applications.</para>
        /// <para>Người đăng ký có thể đặt thuộc tính <see cref="KeyEventArgsProcessed.Handled"/> thành <c>true</c> để chỉ ra rằng việc nhấn phím đã được xử lý và không nên được chuyển cho các ứng dụng khác.</para>
        /// </summary>
        event EventHandler<KeyEventArgsProcessed>? KeyDownEvent; // Sửa: Thêm '?' để cho phép event là nullable

        /// <summary>
        /// <para>Gets a value indicating whether the global keyboard hook is currently running.</para>
        /// <para>Lấy một giá trị cho biết liệu hook bàn phím toàn cục có đang chạy hay không.</para>
        /// </summary>
        bool IsHookRunning { get; }

        /// <summary>
        /// <para>Starts the global keyboard hook.</para>
        /// <para>Bắt đầu hook bàn phím toàn cục.</para>
        /// <para>Throws an exception if the hook cannot be started.</para>
        /// <para>Ném một ngoại lệ nếu hook không thể bắt đầu.</para>
        /// </summary>
        void StartHook();

        /// <summary>
        /// <para>Stops the global keyboard hook.</para>
        /// <para>Dừng hook bàn phím toàn cục.</para>
        /// </summary>
        void StopHook();
    }
}