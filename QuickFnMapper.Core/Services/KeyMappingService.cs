#region Using Directives
using System.Management;
using QuickFnMapper.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json; // Đảm bảo using này có
using System.Windows.Forms; // For Keys enum
using WindowsInput;
using WindowsInput.Native;
// using QuickFnMapper.Core.Services; // Không cần nếu NotificationEventArgs cùng namespace
#endregion

namespace QuickFnMapper.Core.Services
{
    public class KeyMappingService : IKeyMappingService
    {
        #region P/Invoke for SendInput (Dùng cho SendMediaKeyAction)
        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT { public uint type; public InputUnion U; public static int Size => Marshal.SizeOf(typeof(INPUT)); }
        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion { [FieldOffset(0)] public MOUSEINPUT mi; [FieldOffset(0)] public KEYBDINPUT ki; [FieldOffset(0)] public HARDWAREINPUT hi; }
        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT { public ushort wVk; public ushort wScan; public uint dwFlags; public uint time; public IntPtr dwExtraInfo; }
        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT { public int dx; public int dy; public uint mouseData; public uint dwFlags; public uint time; public IntPtr dwExtraInfo; }
        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT { public uint uMsg; public ushort wParamL; public ushort wParamH; }
        private const uint INPUT_KEYBOARD = 1;
        private const uint KEYEVENTF_KEYUP = 0x0002;
        private const ushort VK_VOLUME_MUTE = 0xAD; private const ushort VK_VOLUME_DOWN = 0xAE; private const ushort VK_VOLUME_UP = 0xAF; private const ushort VK_MEDIA_NEXT_TRACK = 0xB0; private const ushort VK_MEDIA_PREV_TRACK = 0xB1; private const ushort VK_MEDIA_STOP = 0xB2; private const ushort VK_MEDIA_PLAY_PAUSE = 0xB3;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();
        #endregion

        #region Fields
        private readonly IGlobalHookService _globalHookService;
        private readonly IAppSettingsService _appSettingsService;
        private List<KeyMappingRule> _rules;
        private bool _isServiceCurrentlyEnabled;
        private readonly string _rulesFilePath;
        private readonly IInputSimulator _inputSimulator;

        // ĐẢM BẢO FIELD NÀY CÓ VÀ ĐÚNG
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };
        #endregion

        #region Events
        // Event để thông báo ra ngoài khi cần hiển thị notification
        public event EventHandler<NotificationEventArgs>? NotificationRequested;
        #endregion

        public KeyMappingService(IGlobalHookService globalHookService, IAppSettingsService appSettingsService)
        {
            _globalHookService = globalHookService ?? throw new ArgumentNullException(nameof(globalHookService));
            _appSettingsService = appSettingsService ?? throw new ArgumentNullException(nameof(appSettingsService));
            _inputSimulator = new InputSimulator();

            AppSettings settings = _appSettingsService.LoadSettings();
            _rulesFilePath = settings.RulesFilePath;
            if (string.IsNullOrWhiteSpace(_rulesFilePath))
            {
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                _rulesFilePath = Path.Combine(appDataFolder, "QuickFnMapper", "DefaultKeyMappings.json");
                Debug.WriteLine($"[WARN] KeyMappingService: RulesFilePath from AppSettings was empty. Using default: {_rulesFilePath}");
            }

            _rules = new List<KeyMappingRule>();
            _isServiceCurrentlyEnabled = false;
            LoadRules();
        }

        #region Properties
        public bool IsServiceEnabled => _isServiceCurrentlyEnabled;
        #endregion

        #region IKeyMappingService Implementation
        // ... (EnableService, DisableService, LoadRules, SaveRules, GetAllRules, GetRuleById, AddRule, UpdateRule, DeleteRule giữ nguyên như lần trước)
        public void EnableService() { if (_isServiceCurrentlyEnabled) return; try { _globalHookService.KeyDownEvent += OnGlobalKeyDown; _globalHookService.StartHook(); _isServiceCurrentlyEnabled = true; Debug.WriteLineIf(_isServiceCurrentlyEnabled, "[INFO] KeyMappingService: Enabled."); } catch (Exception ex) { Debug.WriteLine($"[ERROR] KS: Enable failed. {ex.Message}"); DisableService(); } }
        public void DisableService() { if (!_isServiceCurrentlyEnabled && !_globalHookService.IsHookRunning) return; try { _globalHookService.StopHook(); _globalHookService.KeyDownEvent -= OnGlobalKeyDown; _isServiceCurrentlyEnabled = false; Debug.WriteLineIf(!_isServiceCurrentlyEnabled, "[INFO] KeyMappingService: Disabled."); } catch (Exception ex) { Debug.WriteLine($"[ERROR] KS: Disable failed. {ex.Message}"); } }
        public void LoadRules() { try { if (File.Exists(_rulesFilePath)) { string json = File.ReadAllText(_rulesFilePath); if (!string.IsNullOrWhiteSpace(json)) { _rules = JsonSerializer.Deserialize<List<KeyMappingRule>>(json, _jsonSerializerOptions) ?? new List<KeyMappingRule>(); Debug.WriteLine($"[INFO] KS: Loaded {_rules.Count} rules from '{_rulesFilePath}'."); return; } else Debug.WriteLine($"[INFO] KS: Rules file '{_rulesFilePath}' empty."); } else Debug.WriteLine($"[INFO] KS: Rules file '{_rulesFilePath}' not found."); } catch (Exception ex) { Debug.WriteLine($"[ERROR] KS: Load rules error. {ex.Message}."); } _rules = new List<KeyMappingRule>(); }
        public void SaveRules() { try { string? dir = Path.GetDirectoryName(_rulesFilePath); if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir); string json = JsonSerializer.Serialize(_rules, _jsonSerializerOptions); File.WriteAllText(_rulesFilePath, json); Debug.WriteLine($"[INFO] KS: Saved {_rules.Count} rules to '{_rulesFilePath}'."); } catch (Exception ex) { Debug.WriteLine($"[ERROR] KS: Save rules error. {ex.Message}"); } }
        public IEnumerable<KeyMappingRule> GetAllRules() => _rules.ToList();
        public KeyMappingRule? GetRuleById(Guid id) => _rules.FirstOrDefault(r => r.Id == id);
        public void AddRule(KeyMappingRule rule) { if (rule == null) throw new ArgumentNullException(nameof(rule)); if (_rules.Any(r => r.Id == rule.Id)) throw new InvalidOperationException($"Rule ID {rule.Id} exists."); _rules.Add(rule); SaveRules(); }
        public void UpdateRule(KeyMappingRule rule) { if (rule == null) throw new ArgumentNullException(nameof(rule)); int i = _rules.FindIndex(r => r.Id == rule.Id); if (i == -1) throw new InvalidOperationException($"Rule ID {rule.Id} not found."); rule.OriginalKey ??= new OriginalKeyData(Keys.None); rule.TargetActionDetails ??= new TargetAction(); _rules[i] = rule; rule.LastModifiedDate = DateTime.UtcNow; SaveRules(); }
        public void DeleteRule(Guid id) { if (_rules.RemoveAll(r => r.Id == id) == 0) Debug.WriteLine($"[WARN] KS: Rule ID {id} not found for deletion."); else SaveRules(); }

        #endregion

        #region Private Helper Methods
        private void OnGlobalKeyDown(object? sender, KeyEventArgsProcessed e)
        {
            if (!_isServiceCurrentlyEnabled || e == null || e.KeyData == null) return;

            var activeRules = _rules.Where(r => r.IsEnabled && r.OriginalKey != null) // Đảm bảo OriginalKey không null
                                    .OrderBy(r => r.Order)
                                    .ThenByDescending(r => r.LastModifiedDate);

            foreach (var rule in activeRules)
            {
                // OriginalKey đã được kiểm tra không null ở trên
                if (DoesKeyMatchRule(e.KeyData, rule.OriginalKey!))
                {
                    string ruleNameForNotification = rule.Name ?? "Unnamed Rule";
                    Debug.WriteLine($"[INFO] KeyMappingService: Matched rule '{ruleNameForNotification}' for key {e.KeyData.Key}");
                    if (rule.TargetActionDetails != null)
                    {
                        ExecuteAction(rule.TargetActionDetails, ruleNameForNotification);
                    }
                    e.Handled = true;
                    break;
                }
            }
        }

        private bool DoesKeyMatchRule(OriginalKeyData pressedKey, OriginalKeyData ruleKey)
        {
            return pressedKey.Key == ruleKey.Key &&
                   pressedKey.Ctrl == ruleKey.Ctrl &&
                   pressedKey.Shift == ruleKey.Shift &&
                   pressedKey.Alt == ruleKey.Alt &&
                   pressedKey.Win == ruleKey.Win;
        }

        // ExecuteAction nhận thêm ruleName để dùng trong notification
        private void ExecuteAction(TargetAction actionDetails, string ruleName)
        {
            AppSettings settings = _appSettingsService.LoadSettings();
            bool ruleExecutedSuccessfully = false;
            string? errorMessage = null;
            string actionTypeString = actionDetails.Type.ToString();
            string actionParamString = actionDetails.ActionParameter ?? string.Empty;

            try
            {
                Debug.WriteLine($"[INFO] Executing action: {actionTypeString}, Param: '{actionParamString}' for rule '{ruleName}'");
                switch (actionDetails.Type)
                {
                    case ActionType.SendMediaKey: SendMediaKeyAction(actionParamString); ruleExecutedSuccessfully = true; break;
                    case ActionType.RunApplication: RunApplicationAction(actionParamString); ruleExecutedSuccessfully = true; break;
                    case ActionType.OpenUrl: OpenUrlAction(actionParamString); ruleExecutedSuccessfully = true; break;
                    case ActionType.SendText: SendTextAction(actionParamString); ruleExecutedSuccessfully = true; break;
                    case ActionType.TriggerHotkeyOrCommand: TriggerHotkeyOrCommandAction(actionParamString, actionDetails.ActionParameterSecondary); ruleExecutedSuccessfully = true; break;
                    case ActionType.SetScreenBrightness:
                        AdjustScreenBrightness(actionParamString);
                        // Giả sử AdjustScreenBrightness sẽ tự xử lý thông báo lỗi nếu cần
                        // Hoặc bạn có thể kiểm tra kết quả trả về từ AdjustScreenBrightness nếu có
                        ruleExecutedSuccessfully = true; // Giả định thành công nếu không có exception
                        break;
                    case ActionType.None: Debug.WriteLine($"[INFO] ActionType is None for rule '{ruleName}'."); break; // Không thông báo thành công cho None
                    default: Debug.WriteLine($"[WARN] Unhandled ActionType: {actionDetails.Type} for rule '{ruleName}'."); break;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                Debug.WriteLine($"[ERROR] KeyMappingService: Error executing action '{actionTypeString}' for rule '{ruleName}'. {errorMessage}");
            }

            // Raise event nếu setting ShowNotifications được bật
            if (settings.ShowNotifications)
            {
                NotificationEventArgs.NotificationInfo? notificationInfo = null;
                if (ruleExecutedSuccessfully && actionDetails.Type != ActionType.None) // Chỉ thông báo thành công nếu không phải ActionType.None
                {
                    notificationInfo = new NotificationEventArgs.NotificationInfo(
                        $"Rule '{Truncate(ruleName, 30)}' Triggered",
                        $"Action '{actionTypeString}' executed." +
                        (!string.IsNullOrEmpty(actionParamString) ? $" Param: '{Truncate(actionParamString, 50)}'" : ""),
                        NotificationType.Info // Enum từ NotificationEventArgs.cs
                    );
                }
                else if (!string.IsNullOrEmpty(errorMessage))
                {
                    notificationInfo = new NotificationEventArgs.NotificationInfo(
                        $"Error in Rule '{Truncate(ruleName, 30)}'",
                        $"Failed to execute '{actionTypeString}'. Error: {Truncate(errorMessage, 100)}",
                        NotificationType.Error // Enum từ NotificationEventArgs.cs
                    );
                }

                if (notificationInfo != null)
                {
                    OnNotificationRequested(notificationInfo);
                }
            }
        }

        // Phương thức để raise event
        protected virtual void OnNotificationRequested(NotificationEventArgs.NotificationInfo info)
        {
            NotificationRequested?.Invoke(this, new NotificationEventArgs(info));
        }

        private string Truncate(string? value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        }

        // ... (Các phương thức SendMediaKeyAction, SendTextAction, RunApplicationAction, OpenUrlAction, TriggerHotkeyOrCommandAction giữ nguyên như lần cập nhật trước)
        private void SendMediaKeyAction(string? mediaKeyName) { if (string.IsNullOrWhiteSpace(mediaKeyName)) { Debug.WriteLine("[WARN] SendMediaKeyAction: null/empty."); return; } ushort vk = 0; string norm = mediaKeyName.Replace(" ", "").ToUpperInvariant(); switch (norm) { case "VOLUMEMUTE": case "MUTE": vk = VK_VOLUME_MUTE; break; case "VOLUMEDOWN": vk = VK_VOLUME_DOWN; break; case "VOLUMEUP": vk = VK_VOLUME_UP; break; case "MEDIANEXTTRACK": case "NEXT": vk = VK_MEDIA_NEXT_TRACK; break; case "MEDIAPREVIOUSTRACK": case "PREVIOUS": case "PREV": vk = VK_MEDIA_PREV_TRACK; break; case "MEDIASTOP": case "STOP": vk = VK_MEDIA_STOP; break; case "MEDIAPLAYPAUSE": case "PLAYPAUSE": case "PLAY": case "PAUSE": vk = VK_MEDIA_PLAY_PAUSE; break; default: if (!ushort.TryParse(mediaKeyName, out vk)) { Debug.WriteLine($"[WARN] SendMediaKey: Unknown '{mediaKeyName}'"); return; } break; } INPUT[] ins = new INPUT[2]; ins[0].type = INPUT_KEYBOARD; ins[0].U.ki = new KEYBDINPUT { wVk = vk, dwFlags = 0 }; ins[1].type = INPUT_KEYBOARD; ins[1].U.ki = new KEYBDINPUT { wVk = vk, dwFlags = KEYEVENTF_KEYUP }; if (SendInput(2, ins, INPUT.Size) != 2) Debug.WriteLine($"[ERROR] SendMediaKey: SendInput failed: {Marshal.GetLastWin32Error()}"); }
        private void SendTextAction(string text) { if (string.IsNullOrEmpty(text)) return; Debug.WriteLine($"[INFO] SendText: '{text}'"); try { _inputSimulator.Keyboard.TextEntry(text); } catch (Exception ex) { Debug.WriteLine($"[ERROR] SendText: {ex.Message}"); } }
        private void RunApplicationAction(string? path) { if (string.IsNullOrWhiteSpace(path)) { Debug.WriteLine("[WARN] RunApp: null/empty path."); return; } try { Process.Start(path); } catch (Exception ex) { Debug.WriteLine($"[ERROR] RunApp '{path}': {ex.Message}"); } }
        private void OpenUrlAction(string? url) { if (string.IsNullOrWhiteSpace(url)) { Debug.WriteLine("[WARN] OpenUrl: null/empty URL."); return; } try { string pUrl = url; if (!pUrl.Contains("://")) pUrl = "http://" + pUrl; Process.Start(new ProcessStartInfo(pUrl) { UseShellExecute = true }); } catch (Exception ex) { Debug.WriteLine($"[ERROR] OpenUrl '{url}': {ex.Message}"); } }
        private void TriggerHotkeyOrCommandAction(string? param, string? secParam) { if (string.IsNullOrWhiteSpace(param)) { Debug.WriteLine("[WARN] TriggerHotkey: null/empty param."); return; } string type = secParam?.ToUpperInvariant() ?? "COMMAND"; if (type == "HOTKEY") { Debug.WriteLine($"[INFO] TriggerHotkey: '{param}'"); try { List<VirtualKeyCode> mods = new List<VirtualKeyCode>(), keys = new List<VirtualKeyCode>(); foreach (string p in param.Split('+')) { string tp = p.Trim().ToUpperInvariant(); switch (tp) { case "CTRL": case "CONTROL": mods.Add(VirtualKeyCode.CONTROL); break; case "SHIFT": mods.Add(VirtualKeyCode.SHIFT); break; case "ALT": case "MENU": mods.Add(VirtualKeyCode.MENU); break; case "WIN": case "WINDOWS": mods.Add(VirtualKeyCode.LWIN); break; default: if (Enum.TryParse<VirtualKeyCode>(tp, true, out var vk)) keys.Add(vk); else if (tp.Length == 1 && Enum.TryParse<VirtualKeyCode>("VK_" + tp[0], true, out var cvk)) keys.Add(cvk); else Debug.WriteLine($"[WARN] Hotkey parse: Unknown '{tp}'."); break; } } if (keys.Any()) { if (mods.Any()) _inputSimulator.Keyboard.ModifiedKeyStroke(mods, keys); else _inputSimulator.Keyboard.KeyPress(keys.ToArray()); } else if (mods.Any()) _inputSimulator.Keyboard.KeyPress(mods.ToArray()); else Debug.WriteLine($"[WARN] Hotkey parse: No valid keys/mods in '{param}'."); } catch (Exception ex) { Debug.WriteLine($"[ERROR] TriggerHotkey: {ex.Message}"); } } else { Debug.WriteLine($"[INFO] Executing command: '{param}'"); try { Process.Start(new ProcessStartInfo("cmd.exe", $"/c \"{param}\"") { WindowStyle = ProcessWindowStyle.Hidden, CreateNoWindow = true, UseShellExecute = false }); } catch (Exception ex) { Debug.WriteLine($"[ERROR] Executing command '{param}': {ex.Message}"); } } }

        #endregion

        #region Brightness Control Methods (WMI)

        /// <summary>
        /// Gets the current screen brightness.
        /// </summary>
        /// <returns>Current brightness percentage (0-100), or -1 if an error occurs.</returns>
        private int GetCurrentBrightness()
        {
            try
            {
                ManagementScope scope = new ManagementScope("root\\WMI"); // [cite: 344]
                SelectQuery query = new SelectQuery("WmiMonitorBrightness"); // [cite: 345]

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
                {
                    using (ManagementObjectCollection objectCollection = searcher.Get())
                    {
                        foreach (ManagementObject mObj in objectCollection) // [cite: 346]
                        {
                            object brightnessObj = mObj["CurrentBrightness"]; // Lấy giá trị dưới dạng object trước
                            if (brightnessObj != null) // [cite: 347]
                            {
                                Debug.WriteLine($"[DEBUG] GetCurrentBrightness: Raw WMI CurrentBrightness value: '{brightnessObj}', Type: '{brightnessObj.GetType().FullName}'");
                                try
                                {
                                    // WMI CurrentBrightness thường là byte (0-100).
                                    // Thử chuyển đổi sang byte trước, sau đó sang int.
                                    byte byteBrightness = Convert.ToByte(brightnessObj);
                                    return (int)byteBrightness; // An toàn hơn là Convert.ToInt32 trực tiếp
                                }
                                catch (FormatException fe)
                                {
                                    Debug.WriteLine($"[ERROR] GetCurrentBrightness: FormatException converting brightness value '{brightnessObj}'. {fe.Message}");
                                }
                                catch (InvalidCastException ice) // Bắt lỗi ép kiểu cụ thể
                                {
                                    Debug.WriteLine($"[ERROR] GetCurrentBrightness: InvalidCastException converting brightness value '{brightnessObj}'. {ice.Message}");
                                    // Có thể thử một số cách chuyển đổi khác nếu cần, ví dụ nếu nó là string
                                    if (brightnessObj is string strVal && byte.TryParse(strVal, out byte parsedByte))
                                    {
                                        Debug.WriteLine($"[DEBUG] GetCurrentBrightness: Successfully parsed brightness string '{strVal}' to byte '{parsedByte}'.");
                                        return (int)parsedByte;
                                    }
                                }
                                catch (OverflowException oe)
                                {
                                    Debug.WriteLine($"[ERROR] GetCurrentBrightness: OverflowException converting brightness value '{brightnessObj}'. {oe.Message}");
                                }
                            }
                            // Chỉ cần lấy độ sáng của màn hình đầu tiên tìm thấy
                            break;
                        }
                    }
                }
                Debug.WriteLine("[WARN] GetCurrentBrightness: WmiMonitorBrightness instance not found or CurrentBrightness property is null/unconvertible."); // [cite: 349]
            }
            catch (ManagementException mex) // Bắt lỗi cụ thể từ WMI
            {
                Debug.WriteLine($"[ERROR] GetCurrentBrightness (ManagementException): {mex.Message}. ErrorCode: {mex.ErrorCode}. WMI may not be available or supported.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] GetCurrentBrightness (General Exception): {ex.Message}"); // [cite: 350]
            }
            return -1; // [cite: 351]
        }

        /// <summary>
        /// Sets the screen brightness.
        /// </summary>
        /// <param name="targetBrightness">Target brightness percentage (0-100).</param>
        private void SetTargetBrightness(byte targetBrightness)
        {
            // Đảm bảo giá trị nằm trong khoảng 0-100 (byte đã là không âm)
            if (targetBrightness > 100) targetBrightness = 100; // [cite: 353]

            try
            {
                ManagementScope scope = new ManagementScope("root\\WMI"); // [cite: 354]
                SelectQuery query = new SelectQuery("WmiMonitorBrightnessMethods"); // [cite: 355]
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
                {
                    using (ManagementObjectCollection objectCollection = searcher.Get())
                    {
                        if (objectCollection.Count == 0)
                        {
                            Debug.WriteLine("[WARN] SetTargetBrightness: WmiMonitorBrightnessMethods instance not found. Cannot set brightness.");
                            OnNotificationRequested(new NotificationEventArgs.NotificationInfo(
                                "Brightness Error", "Could not find a WMI object to control screen brightness.", NotificationType.Error));
                            return;
                        }

                        foreach (ManagementObject mObj in objectCollection) // [cite: 356]
                        {
                            // Tham số đầu tiên của WmiSetBrightness là timeout (không dùng ở đây), thứ hai là độ sáng
                            mObj.InvokeMethod("WmiSetBrightness", new object[] { (uint)0, targetBrightness }); // [cite: 357] // Đặt timeout là 0
                            Debug.WriteLine($"[INFO] SetTargetBrightness: Attempted to set brightness to {targetBrightness}");
                            // Thông báo cho người dùng rằng lệnh đã được gửi, không nhất thiết phải thành công ngay
                            // OnNotificationRequested(new NotificationEventArgs.NotificationInfo(
                            //    "Brightness Control", $"Brightness set to {targetBrightness}%", NotificationType.Info));
                            return; // Áp dụng cho màn hình đầu tiên tìm thấy
                        }
                    }
                }
            }
            catch (ManagementException mex)
            {
                Debug.WriteLine($"[ERROR] SetTargetBrightness (ManagementException): {mex.Message}. ErrorCode: {mex.ErrorCode}. WMI may not be available or brightness control not supported.");
                OnNotificationRequested(new NotificationEventArgs.NotificationInfo(
                   "Brightness Error", $"Failed to set brightness via WMI: {mex.Message}", NotificationType.Error));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] SetTargetBrightness (General Exception): {ex.Message}"); // [cite: 359]
                OnNotificationRequested(new NotificationEventArgs.NotificationInfo(
                    "Brightness Error", $"Failed to set brightness: {ex.Message}", NotificationType.Error));
            }
        }

        /// <summary>
        /// Adjusts the screen brightness based on a relative change string (e.g., "+10", "-10")
        /// or an absolute value string (e.g., "50").
        /// </summary>
        /// <param name="brightnessParameter">The brightness adjustment string.</param>
        private void AdjustScreenBrightness(string? brightnessParameter)
        {
            if (string.IsNullOrWhiteSpace(brightnessParameter)) // [cite: 361]
            {
                Debug.WriteLine("[WARN] AdjustScreenBrightness: brightnessParameter is null or empty.");
                OnNotificationRequested(new NotificationEventArgs.NotificationInfo(
                    "Brightness Error", "Brightness parameter not provided.", NotificationType.Warning));
                return;
            }

            int currentBrightness = GetCurrentBrightness(); // [cite: 362]
            if (currentBrightness == -1) // [cite: 363]
            {
                Debug.WriteLine("[ERROR] AdjustScreenBrightness: Could not retrieve current screen brightness. Cannot adjust.");
                // GetCurrentBrightness đã có thể bắn ra thông báo lỗi rồi, hoặc thông báo ở đây nếu muốn cụ thể hơn
                // OnNotificationRequested(new NotificationEventArgs.NotificationInfo(
                //    "Brightness Error", "Could not retrieve current screen brightness to perform adjustment.", NotificationType.Error)); // [cite: 364]
                return;
            }

            int newBrightness = currentBrightness; // [cite: 365]
            bool isRelative = brightnessParameter.StartsWith("+") || brightnessParameter.StartsWith("-");

            if (int.TryParse(brightnessParameter.TrimStart('+', '-'), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int value))
            {
                if (isRelative)
                {
                    newBrightness = currentBrightness + value * (brightnessParameter.StartsWith("-") ? -1 : 1); // [cite: 366]
                }
                else
                {
                    newBrightness = value; // [cite: 367]
                }

                newBrightness = Math.Max(0, Math.Min(100, newBrightness)); // [cite: 368]
                Debug.WriteLine($"[INFO] AdjustScreenBrightness: Current: {currentBrightness}, Param: '{brightnessParameter}', Target Calculated: {newBrightness}");
                SetTargetBrightness((byte)newBrightness);
            }
            else
            {
                Debug.WriteLine($"[ERROR] AdjustScreenBrightness: Invalid brightness parameter format: '{brightnessParameter}'. Expected format like '+10', '-10', or '50'."); // [cite: 369]
                OnNotificationRequested(new NotificationEventArgs.NotificationInfo( // [cite: 370]
                     "Brightness Error", $"Invalid brightness parameter: '{brightnessParameter}'. Use numbers e.g. '+10', '-10', or '50'.", NotificationType.Error));
            }
        }
        #endregion
    }
}