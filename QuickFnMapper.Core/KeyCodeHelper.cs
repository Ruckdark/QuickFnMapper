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
        // private const uint MAPVK_VSC_TO_VK = 0x01; // Không dùng đến
        // private const uint MAPVK_VK_TO_CHAR = 0x02; // Không dùng đến
        // private const uint MAPVK_VSC_TO_VK_EX = 0x03; // Không dùng đến
        private const uint MAPVK_VK_TO_VSC_EX = 0x04;
        #endregion

        // Dictionary để lưu các tên phím đặc biệt đã được tùy chỉnh
        private static readonly Dictionary<Keys, string> _specialKeyNames = new Dictionary<Keys, string>
        {
            { Keys.None, "None" },
            { Keys.Cancel, "Cancel" },
            { Keys.Back, "Backspace" },
            { Keys.Tab, "Tab" },
            { Keys.LineFeed, "Line Feed" },
            { Keys.Clear, "Clear" },
            { Keys.Enter, "Enter" }, // Giữ lại Keys.Enter (hoặc Keys.Return)
            { Keys.ShiftKey, "Shift" },
            { Keys.ControlKey, "Ctrl" },
            { Keys.Menu, "Alt" }, // Phím Alt (Menu) chung
            { Keys.Pause, "Pause" },
            { Keys.CapsLock, "Caps Lock" }, // Giữ lại Keys.CapsLock (hoặc Keys.Capital)
            { Keys.KanaMode, "Kana" },
            { Keys.JunjaMode, "Junja" },
            { Keys.FinalMode, "Final" },
            { Keys.KanjiMode, "Kanji" },
            { Keys.Escape, "Esc" },
            { Keys.IMEConvert, "IME Convert" },
            { Keys.IMENonconvert, "IME NonConvert" },
            { Keys.IMEAccept, "IME Accept" },
            { Keys.IMEModeChange, "IME Mode Change" },
            { Keys.Space, "Space" },
            { Keys.PageUp, "Page Up" }, // Keys.Prior và Keys.PageUp thường có cùng giá trị (33)
            { Keys.PageDown, "Page Down" }, // Keys.Next và Keys.PageDown thường có cùng giá trị (34)
            { Keys.End, "End" },
            { Keys.Home, "Home" },
            { Keys.Left, "Left Arrow" },
            { Keys.Up, "Up Arrow" },
            { Keys.Right, "Right Arrow" },
            { Keys.Down, "Down Arrow" },
            { Keys.Select, "Select" },
            { Keys.Print, "Print" },
            { Keys.Execute, "Execute" },
            { Keys.PrintScreen, "Print Screen" }, // Keys.Snapshot và Keys.PrintScreen thường có cùng giá trị (44)
            { Keys.Insert, "Insert" },
            { Keys.Delete, "Delete" },
            { Keys.Help, "Help" },
            { Keys.D0, "0" }, { Keys.D1, "1" }, { Keys.D2, "2" }, { Keys.D3, "3" }, { Keys.D4, "4" },
            { Keys.D5, "5" }, { Keys.D6, "6" }, { Keys.D7, "7" }, { Keys.D8, "8" }, { Keys.D9, "9" },
            { Keys.LWin, "Left Windows" },
            { Keys.RWin, "Right Windows" },
            { Keys.Apps, "Applications" },
            { Keys.Sleep, "Sleep" },
            { Keys.NumPad0, "Numpad 0" }, { Keys.NumPad1, "Numpad 1" }, { Keys.NumPad2, "Numpad 2" },
            { Keys.NumPad3, "Numpad 3" }, { Keys.NumPad4, "Numpad 4" }, { Keys.NumPad5, "Numpad 5" },
            { Keys.NumPad6, "Numpad 6" }, { Keys.NumPad7, "Numpad 7" }, { Keys.NumPad8, "Numpad 8" },
            { Keys.NumPad9, "Numpad 9" },
            { Keys.Multiply, "Numpad *" },
            { Keys.Add, "Numpad +" },
            { Keys.Separator, "Numpad Separator" },
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
            { Keys.VolumeMute, "Volume Mute" },
            { Keys.VolumeDown, "Volume Down" },
            { Keys.VolumeUp, "Volume Up" },
            { Keys.MediaNextTrack, "Media Next Track" },
            { Keys.MediaPreviousTrack, "Media Previous Track" },
            { Keys.MediaStop, "Media Stop" },
            { Keys.MediaPlayPause, "Media Play/Pause" },
            { Keys.LaunchMail, "Launch Mail" },
            { Keys.SelectMedia, "Launch Media Select" },
            { Keys.LaunchApplication1, "Launch Application1" },
            { Keys.LaunchApplication2, "Launch Application2" },
            { Keys.Oem1, ";" }, // Keys.OemSemicolon và Keys.Oem1 thường có cùng giá trị (186)
            { Keys.Oemplus, "=" },
            { Keys.Oemcomma, "," },
            { Keys.OemMinus, "-" },
            { Keys.OemPeriod, "." },
            { Keys.Oem2, "/" }, // Keys.OemQuestion và Keys.Oem2 thường có cùng giá trị (191)
            { Keys.Oem3, "`" }, // Keys.Oemtilde và Keys.Oem3 thường có cùng giá trị (192)
            { Keys.Oem4, "[" }, // Keys.OemOpenBrackets và Keys.Oem4 thường có cùng giá trị (219)
            { Keys.Oem5, "\\" }, // Keys.OemPipe và Keys.Oem5 thường có cùng giá trị (220)
            { Keys.Oem6, "]" }, // Keys.OemCloseBrackets và Keys.Oem6 thường có cùng giá trị (221)
            { Keys.Oem7, "'" }, // Keys.OemQuotes và Keys.Oem7 thường có cùng giá trị (222)
            { Keys.Oem8, "OEM 8" },
            { Keys.Oem102, "\\" }, // Keys.OemBackslash và Keys.Oem102 thường có cùng giá trị (226)
            { Keys.ProcessKey, "Process Key" },
            { Keys.Packet, "Packet" },
            { Keys.Attn, "Attn" },
            { Keys.Crsel, "CrSel" },
            { Keys.Exsel, "ExSel" },
            { Keys.EraseEof, "Erase EOF" },
            { Keys.Play, "Play" },
            { Keys.Zoom, "Zoom" },
            { Keys.NoName, "NoName" },
            { Keys.Pa1, "PA1" },
            { Keys.OemClear, "Clear" }
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
            Keys keyCode = key & Keys.KeyCode;

            if (_specialKeyNames.TryGetValue(keyCode, out string? specialName)) // Sửa: out string?
            {
                return specialName ?? keyCode.ToString(); // Trả về keyCode.ToString() nếu specialName là null (không nên xảy ra với dictionary hiện tại) 
            }

            if (keyCode >= Keys.A && keyCode <= Keys.Z)
            {
                return keyCode.ToString();
            }

            string charFromLayout = GetCharFromKey(keyCode);
            if (!string.IsNullOrEmpty(charFromLayout) && charFromLayout.Length == 1)
            {
                if (!char.IsControl(charFromLayout[0]))
                {
                    return charFromLayout.ToUpperInvariant();
                }
            }
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
                IntPtr foregroundWindow = GetForegroundWindow();
                uint threadId = GetWindowThreadProcessId(foregroundWindow, out _);
                IntPtr hkl = GetKeyboardLayout(threadId);

                uint scanCode = MapVirtualKeyEx(virtualKey, MAPVK_VK_TO_VSC_EX, hkl);
                if (scanCode == 0)
                {
                    scanCode = MapVirtualKeyEx(virtualKey, MAPVK_VK_TO_VSC, hkl);
                }

                if (scanCode == 0) return string.Empty;

                byte[] keyboardState = new byte[256];
                GetKeyboardState(keyboardState);

                System.Text.StringBuilder sb = new System.Text.StringBuilder(64);
                int result = ToUnicodeEx(virtualKey, scanCode, keyboardState, sb, sb.Capacity, 0, hkl);
                if (result > 0)
                {
                    return sb.ToString().Substring(0, result);
                }
            }
            catch (Exception)
            {
                // Log exception if needed
            }
            return string.Empty;
        }
    }
}