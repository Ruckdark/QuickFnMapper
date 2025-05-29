// File: QuickFnMapper.Core/Models/KeyEventArgsProcessed.cs
#region Using Directives
using System;
// OriginalKeyData nằm trong cùng namespace Models, nếu khác thì cần using.
// using QuickFnMapper.Core.Models; 
#endregion

namespace QuickFnMapper.Core.Models
{
    /// <summary>
    /// <para>Provides data for keyboard events that have been processed by a hook, allowing indication of whether the event was handled.</para>
    /// <para>Cung cấp dữ liệu cho các sự kiện bàn phím đã được xử lý bởi một hook, cho phép chỉ định liệu sự kiện có được xử lý hay không.</para>
    /// </summary>
    public class KeyEventArgsProcessed : EventArgs
    {
        /// <summary>
        /// <para>Gets the data for the original key combination that was pressed.</para>
        /// <para>Lấy dữ liệu cho tổ hợp phím gốc đã được nhấn.</para>
        /// </summary>
        public OriginalKeyData KeyData { get; } // OriginalKeyData được giả định là non-nullable.

        /// <summary>
        /// <para>Gets or sets a value indicating whether the key event has been handled by a subscriber.</para>
        /// <para>If set to <c>true</c>, the hook procedure will attempt to prevent the key event from being processed by other applications.</para>
        /// <para>Lấy hoặc đặt một giá trị cho biết liệu sự kiện phím có được xử lý bởi một người đăng ký hay không.</para>
        /// <para>Nếu đặt thành <c>true</c>, thủ tục hook sẽ cố gắng ngăn chặn sự kiện phím được xử lý bởi các ứng dụng khác.</para>
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="KeyEventArgsProcessed"/> class.</para>
        /// <para>Khởi tạo một đối tượng mới của lớp <see cref="KeyEventArgsProcessed"/>.</para>
        /// </summary>
        /// <param name="keyData">
        /// <para>The data of the pressed key combination. This parameter is non-nullable.</para>
        /// <para>Dữ liệu của tổ hợp phím đã nhấn. Tham số này không được null.</para>
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para>Thrown if <paramref name="keyData"/> is null.</para>
        /// <para>Ném ra nếu <paramref name="keyData"/> là null.</para>
        /// </exception>
        public KeyEventArgsProcessed(OriginalKeyData keyData)
        {
            // KeyData được yêu cầu là non-null. OriginalKeyData cũng nên được thiết kế để các thành phần cốt lõi của nó (như Key) không dễ bị null.
            KeyData = keyData ?? throw new ArgumentNullException(nameof(keyData));
            Handled = false;
        }
    }
}