#region Using Directives
using QuickFnMapper.Core.Models; // Để sử dụng OriginalKeyData, KeyEventArgsProcessed
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms; // Để sử dụng Keys enum và GetKeyState
using System.ComponentModel; // Để sử dụng Win32Exception
#endregion

namespace QuickFnMapper.Core.Services
{
    /// <summary>
    /// <para>Provides global keyboard hooking functionality using Windows Low-Level Keyboard Hook.</para>
    /// <para>Cung cấp chức năng hook bàn phím toàn cục sử dụng Windows Low-Level Keyboard Hook.</para>
    /// </summary>
    public class GlobalHookService : IGlobalHookService // Đã implement IDisposable từ interface
    {
        #region WinAPI Constants and Declarations

        private const int WH_KEYBOARD_LL = 13; // Low-Level Keyboard Hook
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104; // Khi Alt được nhấn
        private const int HC_ACTION = 0; // wParam và lParam chứa thông tin về keystroke message

        // Delegate cho hàm callback của hook
        // Signature của LowLevelKeyboardProc là chuẩn, không thay đổi nullability của các tham số IntPtr
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string? lpModuleName); // lpModuleName có thể là null

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetKeyState(int keyCode); // Virtual KeyCode

        // Struct để chứa thông tin từ hook callback
        // Các field là value type hoặc IntPtr, không có vấn đề nullability reference type trực tiếp
        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public uint vkCode;      // Virtual-key code
            public uint scanCode;    // Hardware scan code
            public uint flags;       // Flags
            public uint time;        // Time stamp
            public IntPtr dwExtraInfo; // Extra info
        }

        #endregion

        #region Fields
        private IntPtr _hookID = IntPtr.Zero;
        // _keyboardProcDelegate phải là một field để ngăn GC thu hồi nó. Nó không thể null khi hook đang chạy.
        private readonly LowLevelKeyboardProc _keyboardProcDelegate;
        private bool _isDisposed = false;
        #endregion

        #region Properties

        /// <summary>
        /// <para>Gets a value indicating whether the global keyboard hook is currently running.</para>
        /// <para>Lấy một giá trị cho biết liệu hook bàn phím toàn cục có đang chạy hay không.</para>
        /// </summary>
        public bool IsHookRunning => _hookID != IntPtr.Zero;

        #endregion

        #region Events

        /// <summary>
        /// <para>Occurs when a key is pressed down anywhere in the system and the hook is active.</para>
        /// <para>Xảy ra khi một phím được nhấn xuống ở bất kỳ đâu trong hệ thống và hook đang hoạt động.</para>
        /// </summary>
        public event EventHandler<KeyEventArgsProcessed>? KeyDownEvent; // Event có thể nullable

        #endregion

        #region Constructors
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="GlobalHookService"/> class.</para>
        /// <para>Khởi tạo một đối tượng mới của lớp <see cref="GlobalHookService"/>.</para>
        /// </summary>
        public GlobalHookService()
        {
            // Gán delegate vào một trường để nó không bị thu gom bởi Garbage Collector
            _keyboardProcDelegate = HookCallback;
        }
        #endregion

        #region IGlobalHookService Implementation

        /// <summary>
        /// <para>Starts the global keyboard hook.</para>
        /// <para>Bắt đầu hook bàn phím toàn cục.</para>
        /// </summary>
        /// <exception cref="System.ComponentModel.Win32Exception">
        /// <para>Thrown if setting the hook fails.</para>
        /// <para>Ném ra nếu việc đặt hook thất bại.</para>
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <para>Thrown if the service has been disposed.</para>
        /// <para>Ném ra nếu dịch vụ đã được giải phóng.</para>
        /// </exception>
        public void StartHook()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(GlobalHookService), "Cannot start hook on a disposed service.");
            }

            if (IsHookRunning) // Hook đã chạy rồi
            {
                return;
            }

            // Sử dụng GetModuleHandle(null) để lấy handle của process hiện tại.
            // Điều này an toàn hơn là dựa vào MainModule.ModuleName có thể null trong một số trường hợp.
            IntPtr hMod = GetModuleHandle(null);
            if (hMod == IntPtr.Zero) // Kiểm tra lỗi GetModuleHandle
            {
                int errorCode = Marshal.GetLastWin32Error();
                Console.WriteLine($"Failed to get module handle. Error code: {errorCode}");
                throw new Win32Exception(errorCode, "Failed to get module handle for setting global keyboard hook.");
            }

            _hookID = SetWindowsHookEx(WH_KEYBOARD_LL, _keyboardProcDelegate, hMod, 0);

            if (_hookID == IntPtr.Zero)
            {
                int errorCode = Marshal.GetLastWin32Error();
                Console.WriteLine($"Failed to set keyboard hook. Error code: {errorCode}");
                throw new Win32Exception(errorCode, "Failed to set global keyboard hook.");
            }
            // Console.WriteLine("GlobalHookService: Hook started."); // For debugging
        }

        /// <summary>
        /// <para>Stops the global keyboard hook.</para>
        /// <para>Dừng hook bàn phím toàn cục.</para>
        /// </summary>
        public void StopHook()
        {
            if (IsHookRunning) // Chỉ Unhook nếu hook đang chạy
            {
                if (!UnhookWindowsHookEx(_hookID))
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    // Log lỗi nhưng vẫn tiếp tục reset _hookID
                    Console.WriteLine($"Failed to unhook keyboard hook. Error code: {errorCode}");
                }
                _hookID = IntPtr.Zero;
                // Console.WriteLine("GlobalHookService: Hook stopped."); // For debugging
            }
        }

        #endregion

        #region Hook Callback Method

        /// <summary>
        /// <para>The callback method for the low-level keyboard hook.</para>
        /// <para>Phương thức callback cho hook bàn phím mức thấp.</para>
        /// </summary>
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            bool handledBySubscribers = false;
            if (nCode == HC_ACTION) // Hoặc nCode >= 0 đều được cho WH_KEYBOARD_LL
            {
                // Chỉ xử lý sự kiện nhấn phím xuống
                // wParam là message identifier (WM_KEYDOWN, WM_SYSKEYDOWN, etc.)
                if (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
                {
                    // Marshal dữ liệu từ lParam (con trỏ) sang cấu trúc KBDLLHOOKSTRUCT
                    // Trong trường hợp này, lParam không nên là null. Nếu nó là null, PtrToStructure sẽ ném ArgumentNullException.
                    KBDLLHOOKSTRUCT kbdStruct = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT))!;

                    Keys currentKey = (Keys)kbdStruct.vkCode;

                    // Kiểm tra trạng thái các phím bổ trợ
                    // Bit cao nhất (0x8000) cho biết phím đang được nhấn.
                    bool ctrlPressed = (GetKeyState((int)Keys.LControlKey) & 0x8000) != 0 ||
                                       (GetKeyState((int)Keys.RControlKey) & 0x8000) != 0 ||
                                       (GetKeyState((int)Keys.ControlKey) & 0x8000) != 0; // Bao gồm cả ControlKey chung

                    bool shiftPressed = (GetKeyState((int)Keys.LShiftKey) & 0x8000) != 0 ||
                                        (GetKeyState((int)Keys.RShiftKey) & 0x8000) != 0 ||
                                        (GetKeyState((int)Keys.ShiftKey) & 0x8000) != 0; // Bao gồm cả ShiftKey chung

                    bool altPressed = (GetKeyState((int)Keys.LMenu) & 0x8000) != 0 ||
                                      (GetKeyState((int)Keys.RMenu) & 0x8000) != 0 ||
                                      (GetKeyState((int)Keys.Menu) & 0x8000) != 0; // Bao gồm cả Menu (Alt) chung

                    bool winPressed = (GetKeyState((int)Keys.LWin) & 0x8000) != 0 ||
                                      (GetKeyState((int)Keys.RWin) & 0x8000) != 0;

                    // OriginalKeyData constructor đã có giá trị mặc định, nên keyData sẽ không null.
                    OriginalKeyData keyData = new OriginalKeyData(currentKey, ctrlPressed, shiftPressed, altPressed, winPressed);

                    // KeyEventArgsProcessed constructor yêu cầu keyData non-null, đã đảm bảo ở trên.
                    KeyEventArgsProcessed args = new KeyEventArgsProcessed(keyData);

                    // Sử dụng 'this' (object?) cho sender là an toàn.
                    KeyDownEvent?.Invoke(this, args);

                    handledBySubscribers = args.Handled;
                }
            }

            // Nếu đã xử lý (handled = true), trả về 1 để chặn phím. Ngược lại, gọi CallNextHookEx.
            // _hookID có thể đã bị set về Zero nếu StopHook được gọi từ thread khác trong lúc HookCallback đang chạy.
            // Tuy nhiên, CallNextHookEx với hhk là IntPtr.Zero có thể gây lỗi.
            // Cần đảm bảo _hookID vẫn hợp lệ hoặc xử lý tình huống này.
            // Trong thực tế, việc StopHook() bất đồng bộ trong khi callback đang chạy là tình huống phức tạp.
            // Thông thường, UnhookWindowsHookEx sẽ đợi callback hoàn thành.
            return handledBySubscribers ? (IntPtr)1 : CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// <para>Releases all resources used by the <see cref="GlobalHookService"/>.</para>
        /// <para>Giải phóng tất cả tài nguyên được sử dụng bởi <see cref="GlobalHookService"/>.</para>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <para>Protected method to release resources.</para>
        /// <para>Phương thức được bảo vệ để giải phóng tài nguyên.</para>
        /// </summary>
        /// <param name="disposing">
        /// <para>True if called from Dispose(), false if called from the finalizer.</para>
        /// <para>True nếu được gọi từ Dispose(), false nếu được gọi từ finalizer.</para>
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                // Giải phóng tài nguyên managed (nếu có)
                // Hiện tại không có tài nguyên managed nào cần giải phóng đặc biệt ở đây
                // Event KeyDownEvent sẽ tự được dọn dẹp khi không còn ai đăng ký hoặc khi instance bị GC.
            }

            // Luôn giải phóng tài nguyên unmanaged (hook handle)
            StopHook(); // Đảm bảo hook được gỡ bỏ
            _isDisposed = true;
        }

        /// <summary>
        /// <para>Finalizer for the <see cref="GlobalHookService"/> class.</para>
        /// <para>Hàm hủy cho lớp <see cref="GlobalHookService"/>.</para>
        /// <para>Ensures the hook is unhooked if Dispose was not called explicitly.</para>
        /// <para>Đảm bảo hook được gỡ bỏ nếu Dispose không được gọi một cách tường minh.</para>
        /// </summary>
        ~GlobalHookService()
        {
            Dispose(false);
        }
        #endregion
    }
}