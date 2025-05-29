#region Using Directives
using System;
using System.Collections.Generic;
using System.Windows.Forms; // Để sử dụng enum Keys
using System.Runtime.InteropServices; // Để sử dụng MapVirtualKey
#endregion

namespace QuickFnMapper.Core
{
    /// <summary>
    /// <para>Provides utility methods for working with key codes and key names.</para>
    /// <para>Cung cấp các phương thức tiện ích để làm việc với mã phím và tên phím.</para>
    /// </summary>
    public static class KeyCodeHelper
    {
        #region WinAPI Declarations for ToUnicode
        // Sử dụng ToUnicode để lấy ký tự từ mã phím, có thể chính xác hơn trong một số trường hợp
        // và xử lý được layout bàn phím hiện tại.
        [DllImport("user32.dll")]
        private static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState,
                                            [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)]
                                            System.Text.StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);

        [DllImport("user32.dll")]
        private static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, IntPtr dwhkl);

        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private const uint MAPVK_VK_TO_VSC = 0x00;
        private const uint MAPVK_VSC_TO_VK = 0x01;
        private const uint MAPVK_VK_TO_CHAR = 0x02;
        private const uint MAPVK_VSC_TO_VK_EX = 0x03;
        private const uint MAPVK_VK_TO_VSC_EX = 0x04;
        #endregion

        // Dictionary để lưu các tên phím đặc biệt đã được tùy chỉnh
        private static readonly Dictionary<Keys, string> _specialKeyNames = new Dictionary<Keys, string>
        {
            { Keys.None, "None" },
            { Keys.Cancel, "Cancel" }, // Ctrl+Break
            { Keys.Back, "Backspace" },
            { Keys.Tab, "Tab" },
            { Keys.LineFeed, "Line Feed" },
            { Keys.Clear, "Clear" },
            { Keys.Return, "Enter" }, // Hoặc "Return"
            { Keys.Enter, "Enter" },
            { Keys.ShiftKey, "Shift" }, // Phím Shift chung
            { Keys.ControlKey, "Ctrl" }, // Phím Control chung
            { Keys.Menu, "Alt" }, // Phím Alt (Menu) chung
            { Keys.Pause, "Pause" },
            { Keys.Capital, "Caps Lock" }, // Hoặc "CapsLock"
            { Keys.CapsLock, "Caps Lock" },
            { Keys.KanaMode, "Kana" }, // IME related
            { Keys.JunjaMode, "Junja" }, // IME related
            { Keys.FinalMode, "Final" }, // IME related
            { Keys.KanjiMode, "Kanji" }, // IME related
            { Keys.Escape, "Esc" },
            { Keys.IMEConvert, "IME Convert" },
            { Keys.IMENonconvert, "IME NonConvert" },
            { Keys.IMEAccept, "IME Accept" },
            { Keys.IMEModeChange, "IME Mode Change" },
            { Keys.Space, "Space" },
            { Keys.Prior, "Page Up" }, // Hoặc "PgUp"
            { Keys.PageUp, "Page Up" },
            { Keys.Next, "Page Down" }, // Hoặc "PgDn"
            { Keys.PageDown, "Page Down" },
            { Keys.End, "End" },
            { Keys.Home, "Home" },
            { Keys.Left, "Left Arrow" },
            { Keys.Up, "Up Arrow" },
            { Keys.Right, "Right Arrow" },
            { Keys.Down, "Down Arrow" },
            { Keys.Select, "Select" },
            { Keys.Print, "Print" }, // Thường là một phần của PrintScreen
            { Keys.Execute, "Execute" },
            { Keys.PrintScreen, "Print Screen" }, // Hoặc "PrtSc"
            { Keys.Snapshot, "Print Screen" },
            { Keys.Insert, "Insert" }, // Hoặc "Ins"
            { Keys.Delete, "Delete" }, // Hoặc "Del"
            { Keys.Help, "Help" },
            { Keys.D0, "0" }, { Keys.D1, "1" }, { Keys.D2, "2" }, { Keys.D3, "3" }, { Keys.D4, "4" },
            { Keys.D5, "5" }, { Keys.D6, "6" }, { Keys.D7, "7" }, { Keys.D8, "8" }, { Keys.D9, "9" },
            // Keys A-Z được xử lý riêng để trả về ký tự
            { Keys.LWin, "Left Windows" },
            { Keys.RWin, "Right Windows" },
            { Keys.Apps, "Applications" }, // Phím menu ngữ cảnh
            { Keys.Sleep, "Sleep" },
            { Keys.NumPad0, "Numpad 0" }, { Keys.NumPad1, "Numpad 1" }, { Keys.NumPad2, "Numpad 2" },
            { Keys.NumPad3, "Numpad 3" }, { Keys.NumPad4, "Numpad 4" }, { Keys.NumPad5, "Numpad 5" },
            { Keys.NumPad6, "Numpad 6" }, { Keys.NumPad7, "Numpad 7" }, { Keys.NumPad8, "Numpad 8" },
            { Keys.NumPad9, "Numpad 9" },
            { Keys.Multiply, "Numpad *" },
            { Keys.Add, "Numpad +" },
            { Keys.Separator, "Numpad Separator" }, // Thường là phím Enter trên Numpad ở một số layout
            { Keys.Subtract, "Numpad -" },
            { Keys.Decimal, "Numpad ." },
            { Keys.Divide, "Numpad /" },
            { Keys.F1, "F1" }, { Keys.F2, "F2" }, { Keys.F3, "F3" }, { Keys.F4, "F4" },
            { Keys.F5, "F5" }, { Keys.F6, "F6" }, { Keys.F7, "F7" }, { Keys.F8, "F8" },
            { Keys.F9, "F9" }, { Keys.F10, "F10" }, { Keys.F11, "F11" }, { Keys.F12, "F12" },
            { Keys.F13, "F13" }, { Keys.F14, "F14" }, { Keys.F15, "F15" }, { Keys.F16, "F16" },
            { Keys.F17, "F17" }, { Keys.F18, "F18" }, { Keys.F19, "F19" }, { Keys.F20, "F20" },
            { Keys.F21, "F21" }, { Keys.F22, "F22" }, { Keys.F23, "F23" }, { Keys.F24, "F24" },
            { Keys.NumLock, "Num Lock" },
            { Keys.Scroll, "Scroll Lock" },
            { Keys.LShiftKey, "Left Shift" },
            { Keys.RShiftKey, "Right Shift" },
            { Keys.LControlKey, "Left Ctrl" },
            { Keys.RControlKey, "Right Ctrl" },
            { Keys.LMenu, "Left Alt" },
            { Keys.RMenu, "Right Alt" },
            { Keys.BrowserBack, "Browser Back" },
            { Keys.BrowserForward, "Browser Forward" },
            { Keys.BrowserRefresh, "Browser Refresh" },
            { Keys.BrowserStop, "Browser Stop" },
            { Keys.BrowserSearch, "Browser Search" },
            { Keys.BrowserFavorites, "Browser Favorites" },
            { Keys.BrowserHome, "Browser Home" },
            { Keys.VolumeMute, "Volume Mute" }, // Hoặc "Mute"
            { Keys.VolumeDown, "Volume Down" },
            { Keys.VolumeUp, "Volume Up" },
            { Keys.MediaNextTrack, "Media Next Track" },
            { Keys.MediaPreviousTrack, "Media Previous Track" },
            { Keys.MediaStop, "Media Stop" },
            { Keys.MediaPlayPause, "Media Play/Pause" },
            { Keys.LaunchMail, "Launch Mail" },
            { Keys.SelectMedia, "Launch Media Select" },
            { Keys.LaunchApplication1, "Launch Application1" }, // Thường là "My Computer"
            { Keys.LaunchApplication2, "Launch Application2" }, // Thường là "Calculator"
            { Keys.OemSemicolon, ";" }, // Hoặc OEM1 trên một số hệ thống
            { Keys.Oem1, ";" },
            { Keys.Oemplus, "=" }, // Hoặc "+" không kèm Shift
            { Keys.Oemcomma, "," },
            { Keys.OemMinus, "-" },
            { Keys.OemPeriod, "." },
            { Keys.OemQuestion, "/" }, // Hoặc OEM2
            { Keys.Oem2, "/" },
            { Keys.Oemtilde, "`" }, // Hoặc OEM3
            { Keys.Oem3, "`" },
            { Keys.OemOpenBrackets, "[" }, // Hoặc OEM4
            { Keys.Oem4, "[" },
            { Keys.OemPipe, "\\" }, // Hoặc OEM5
            { Keys.Oem5, "\\" },
            { Keys.OemCloseBrackets, "]" }, // Hoặc OEM6
            { Keys.Oem6, "]" },
            { Keys.OemQuotes, "'" }, // Hoặc OEM7
            { Keys.Oem7, "'" },
            { Keys.Oem8, "OEM 8" }, // Thường không được dùng cụ thể
            { Keys.OemBackslash, "\\" }, // Hoặc OEM102, phím phụ trên một số bàn phím non-US
            { Keys.Oem102, "\\" },
            { Keys.ProcessKey, "Process Key" }, // IME Process key
            { Keys.Packet, "Packet" }, // Dùng để truyền ký tự Unicode như thể chúng là phím bấm
            { Keys.Attn, "Attn" },
            { Keys.Crsel, "CrSel" },
            { Keys.Exsel, "ExSel" },
            { Keys.EraseEof, "Erase EOF" },
            { Keys.Play, "Play" },
            { Keys.Zoom, "Zoom" },
            { Keys.NoName, "NoName" }, // Reserved
            { Keys.Pa1, "PA1" }, // PA1 key
            { Keys.OemClear, "Clear" } // Clear key
        };


        /// <summary>
        /// <para>Gets a user-friendly name for the specified key.</para>
        /// <para>Lấy tên thân thiện với người dùng cho phím được chỉ định.</para>
        /// </summary>
        /// <param name="key">
        /// <para>The key value from <see cref="System.Windows.Forms.Keys"/> enum.</para>
        /// <para>Giá trị phím từ enum <see cref="System.Windows.Forms.Keys"/>.</para>
        /// </param>
        /// <returns>
        /// <para>A string representing the user-friendly name of the key.</para>
        /// <para>Một chuỗi đại diện cho tên thân thiện của phím.</para>
        /// </returns>
        public static string GetKeyName(Keys key)
        {
            // Xử lý các phím bổ trợ đặc biệt nếu chúng được truyền dưới dạng "KeyData"
            // Tuy nhiên, trong ngữ cảnh của OriginalKeyData, chúng ta chỉ quan tâm đến phím chính
            // vì các phím bổ trợ đã được lưu riêng.
            Keys keyCode = key & Keys.KeyCode; // Chỉ lấy phần mã phím, loại bỏ các modifier nếu có (ví dụ: Keys.Control | Keys.A)

            if (_specialKeyNames.TryGetValue(keyCode, out string specialName))
            {
                return specialName;
            }

            // Đối với các phím chữ A-Z
            if (keyCode >= Keys.A && keyCode <= Keys.Z)
            {
                return keyCode.ToString(); // Sẽ trả về "A", "B", ...
            }

            // Thử sử dụng ToUnicodeEx để lấy ký tự theo layout bàn phím hiện tại cho các phím khác
            // Điều này có thể giúp hiển thị đúng các ký tự như ';', '=', '[', ']', v.v.
            // mà không cần hardcode tất cả các trường hợp Oem
            string charFromLayout = GetCharFromKey(keyCode);
            if (!string.IsNullOrEmpty(charFromLayout) && charFromLayout.Length == 1)
            {
                // Chỉ chấp nhận nếu là một ký tự đơn và không phải là ký tự điều khiển
                if (!char.IsControl(charFromLayout[0]))
                {
                    return charFromLayout.ToUpperInvariant(); // Hiển thị ký tự, ví dụ ";", "="
                }
            }

            // Nếu không tìm thấy trong dictionary và không phải A-Z, trả về tên mặc định từ enum
            // Có thể cần thêm các xử lý cho các trường hợp khác nếu cần
            return keyCode.ToString();
        }


        /// <summary>
        /// <para>Tries to get the character representation of a key, considering the current keyboard layout.</para>
        /// <para>Cố gắng lấy ký tự đại diện của một phím, xem xét layout bàn phím hiện tại.</para>
        /// </summary>
        /// <param name="key">
        /// <para>The key code.</para>
        /// <para>Mã phím.</para>
        /// </param>
        /// <returns>
        /// <para>The character, or an empty string if not applicable or not a single character.</para>
        /// <para>Ký tự, hoặc một chuỗi rỗng nếu không áp dụng được hoặc không phải là một ký tự đơn.</para>
        /// </returns>
        private static string GetCharFromKey(Keys key)
        {
            try
            {
                uint virtualKey = (uint)key;
                // Lấy layout bàn phím cho thread của cửa sổ đang active
                IntPtr foregroundWindow = GetForegroundWindow();
                uint threadId = GetWindowThreadProcessId(foregroundWindow, out _);
                IntPtr hkl = GetKeyboardLayout(threadId);

                uint scanCode = MapVirtualKeyEx(virtualKey, MAPVK_VK_TO_VSC_EX, hkl);
                if (scanCode == 0) // Không map được scan code
                {
                    scanCode = MapVirtualKeyEx(virtualKey, MAPVK_VK_TO_VSC, hkl); // Thử lại với kiểu map cũ hơn
                }

                if (scanCode == 0) return string.Empty; // Vẫn không được thì bỏ qua

                byte[] keyboardState = new byte[256];
                GetKeyboardState(keyboardState); // Lấy trạng thái bàn phím hiện tại (Shift, CapsLock,...)

                System.Text.StringBuilder sb = new System.Text.StringBuilder(64);
                int result = ToUnicodeEx(virtualKey, scanCode, keyboardState, sb, sb.Capacity, 0, hkl);

                if (result > 0)
                {
                    return sb.ToString().Substring(0, result);
                }
            }
            catch (Exception) // Bỏ qua lỗi nếu có vấn đề với P/Invoke
            {
                // Log exception if needed
            }
            return string.Empty;
        }

        // Đại ca có thể thêm các phương thức tiện ích khác ở đây nếu cần, ví dụ:
        // - IsModifierKey(Keys key)
        // - GetKeyFromString(string keyName)
        // - etc.
    }
}