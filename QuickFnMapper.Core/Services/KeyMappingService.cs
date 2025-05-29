#region Using Directives
using QuickFnMapper.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics; // For Debug.WriteLine
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows.Forms; // For Keys enum and SendInput related constants
#endregion

namespace QuickFnMapper.Core.Services
{
    /// <summary>
    /// <para>Manages key mapping rules, including loading, saving, and processing them in conjunction with the global keyboard hook.</para>
    /// <para>Quản lý các quy tắc ánh xạ phím, bao gồm việc tải, lưu và xử lý chúng kết hợp với hook bàn phím toàn cục.</para>
    /// </summary>
    public class KeyMappingService : IKeyMappingService
    {
        #region P/Invoke for SendInput (Action Execution)

        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public uint type;
            public InputUnion U;
            public static int Size => Marshal.SizeOf(typeof(INPUT));
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        private const uint INPUT_KEYBOARD = 1;
        private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const uint KEYEVENTF_KEYUP = 0x0002;
        // private const uint KEYEVENTF_UNICODE = 0x0004; 
        // private const uint KEYEVENTF_SCANCODE = 0x0008; 

        // Virtual Key Codes for Media Keys (subset)
        private const ushort VK_VOLUME_MUTE = 0xAD;
        private const ushort VK_VOLUME_DOWN = 0xAE;
        private const ushort VK_VOLUME_UP = 0xAF;
        private const ushort VK_MEDIA_NEXT_TRACK = 0xB0;
        private const ushort VK_MEDIA_PREV_TRACK = 0xB1;
        private const ushort VK_MEDIA_STOP = 0xB2;
        private const ushort VK_MEDIA_PLAY_PAUSE = 0xB3;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();

        #endregion

        #region Fields
        private readonly IGlobalHookService _globalHookService;
        private readonly IAppSettingsService _appSettingsService;
        private List<KeyMappingRule> _rules; // Sẽ được khởi tạo trong constructor hoặc LoadRules
        private bool _isServiceCurrentlyEnabled;
        private readonly string _rulesFilePath; // Đảm bảo non-null sau constructor

        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };
        #endregion

        #region Properties
        /// <inheritdoc cref="IKeyMappingService.IsServiceEnabled"/>
        public bool IsServiceEnabled => _isServiceCurrentlyEnabled;
        #endregion

        #region Constructors
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="KeyMappingService"/> class.</para>
        /// <para>Khởi tạo một đối tượng mới của lớp <see cref="KeyMappingService"/>.</para>
        /// </summary>
        /// <param name="globalHookService">
        /// <para>The global hook service to subscribe to key events. Must not be null.</para>
        /// <para>Dịch vụ hook toàn cục để đăng ký sự kiện phím. Không được null.</para>
        /// </param>
        /// <param name="appSettingsService">
        /// <para>The application settings service to get configuration like rules file path. Must not be null.</para>
        /// <para>Dịch vụ cài đặt ứng dụng để lấy cấu hình như đường dẫn tệp quy tắc. Không được null.</para>
        /// </param>
        public KeyMappingService(IGlobalHookService globalHookService, IAppSettingsService appSettingsService)
        {
            _globalHookService = globalHookService ?? throw new ArgumentNullException(nameof(globalHookService));
            _appSettingsService = appSettingsService ?? throw new ArgumentNullException(nameof(appSettingsService));

            // Lấy đường dẫn file rules ngay khi khởi tạo và đảm bảo nó non-null
            AppSettings settings = _appSettingsService.LoadSettings(); // LoadSettings() đảm bảo trả về non-null
            _rulesFilePath = settings.RulesFilePath; // RulesFilePath trong AppSettings cũng đảm bảo non-null
            if (string.IsNullOrWhiteSpace(_rulesFilePath)) // Kiểm tra dự phòng (mặc dù AppSettings nên đảm bảo)
            {
                // Gán một đường dẫn mặc định an toàn nếu RulesFilePath từ AppSettings vẫn rỗng
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                _rulesFilePath = Path.Combine(appDataFolder, "QuickFnMapper", "DefaultKeyMappings.json");
                Debug.WriteLine($"[WARN] KeyMappingService: RulesFilePath from AppSettings was empty. Using default: {_rulesFilePath}");
            }

            _rules = new List<KeyMappingRule>(); // Khởi tạo list rỗng trước
            _isServiceCurrentlyEnabled = false;

            LoadRules(); // Tải quy tắc khi khởi tạo. LoadRules sẽ sử dụng _rulesFilePath đã được gán.
        }
        #endregion

        #region IKeyMappingService Implementation

        /// <inheritdoc cref="IKeyMappingService.EnableService"/>
        public void EnableService()
        {
            if (_isServiceCurrentlyEnabled) return;

            try
            {
                _globalHookService.KeyDownEvent += OnGlobalKeyDown;
                _globalHookService.StartHook();
                _isServiceCurrentlyEnabled = true;
                Debug.WriteLineIf(_isServiceCurrentlyEnabled, "[INFO] KeyMappingService: Enabled and hook started.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] KeyMappingService: Failed to enable service. {ex.Message}");
                DisableService(); // Cố gắng dọn dẹp
                // Cân nhắc ném lại exception hoặc một custom exception
                // throw new InvalidOperationException("Failed to enable KeyMappingService.", ex);
            }
        }

        /// <inheritdoc cref="IKeyMappingService.DisableService"/>
        public void DisableService()
        {
            // Chỉ thực hiện nếu service đang chạy hoặc hook đang chạy (để đảm bảo an toàn)
            if (!_isServiceCurrentlyEnabled && !_globalHookService.IsHookRunning) return;

            try
            {
                _globalHookService.StopHook(); // Luôn cố gắng stop hook
                _globalHookService.KeyDownEvent -= OnGlobalKeyDown;
                _isServiceCurrentlyEnabled = false;
                Debug.WriteLineIf(!_isServiceCurrentlyEnabled, "[INFO] KeyMappingService: Disabled and hook stopped.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] KeyMappingService: Failed to disable service properly. {ex.Message}");
                // Trong trường hợp này, trạng thái có thể không nhất quán, cần log cẩn thận.
            }
        }

        /// <inheritdoc cref="IKeyMappingService.LoadRules"/>
        public void LoadRules()
        {
            // _rulesFilePath đã được gán trong constructor và đảm bảo non-null/non-whitespace
            try
            {
                if (File.Exists(_rulesFilePath))
                {
                    string jsonString = File.ReadAllText(_rulesFilePath);
                    if (!string.IsNullOrWhiteSpace(jsonString))
                    {
                        List<KeyMappingRule>? loadedRules = JsonSerializer.Deserialize<List<KeyMappingRule>>(jsonString, _jsonSerializerOptions);
                        _rules = loadedRules ?? new List<KeyMappingRule>();
                        Debug.WriteLine($"[INFO] KeyMappingService: Loaded {_rules.Count} rules from '{_rulesFilePath}'.");
                        return;
                    }
                    else
                    {
                        Debug.WriteLine($"[INFO] KeyMappingService: Rules file '{_rulesFilePath}' is empty. Initializing with empty list.");
                    }
                }
                else
                {
                    Debug.WriteLine($"[INFO] KeyMappingService: Rules file '{_rulesFilePath}' not found. Initializing with empty list.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] KeyMappingService: Error loading rules from '{_rulesFilePath}'. {ex.Message}. Initializing with empty list.");
            }
            _rules = new List<KeyMappingRule>();
        }

        /// <inheritdoc cref="IKeyMappingService.SaveRules"/>
        public void SaveRules()
        {
            // _rulesFilePath đã được gán trong constructor và đảm bảo non-null/non-whitespace
            try
            {
                string? directory = Path.GetDirectoryName(_rulesFilePath); // GetDirectoryName có thể trả về null nếu path là root
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string jsonString = JsonSerializer.Serialize(_rules, _jsonSerializerOptions);
                File.WriteAllText(_rulesFilePath, jsonString);
                Debug.WriteLine($"[INFO] KeyMappingService: Saved {_rules.Count} rules to '{_rulesFilePath}'.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] KeyMappingService: Error saving rules to '{_rulesFilePath}'. {ex.Message}");
            }
        }

        /// <inheritdoc cref="IKeyMappingService.GetAllRules"/>
        public IEnumerable<KeyMappingRule> GetAllRules()
        {
            return _rules.ToList(); // Trả về bản sao để bảo vệ list gốc
        }

        /// <inheritdoc cref="IKeyMappingService.GetRuleById"/>
        public KeyMappingRule? GetRuleById(Guid id) // Kiểu trả về là nullable
        {
            return _rules.FirstOrDefault(r => r.Id == id);
        }

        /// <inheritdoc cref="IKeyMappingService.AddRule"/>
        public void AddRule(KeyMappingRule rule)
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            if (_rules.Any(r => r.Id == rule.Id))
            {
                throw new InvalidOperationException($"Rule with ID {rule.Id} already exists.");
            }
            _rules.Add(rule);
            SaveRules();
        }

        /// <inheritdoc cref="IKeyMappingService.UpdateRule"/>
        public void UpdateRule(KeyMappingRule rule)
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            int index = _rules.FindIndex(r => r.Id == rule.Id);
            if (index == -1)
            {
                throw new InvalidOperationException($"Rule with ID {rule.Id} not found for update.");
            }
            // Đảm bảo OriginalKey và TargetActionDetails không bị null nếu rule đầu vào có thể bị thiếu
            rule.OriginalKey ??= new OriginalKeyData(Keys.None);
            rule.TargetActionDetails ??= new TargetAction();

            _rules[index] = rule;
            rule.LastModifiedDate = DateTime.UtcNow;
            SaveRules();
        }

        /// <inheritdoc cref="IKeyMappingService.DeleteRule"/>
        public void DeleteRule(Guid id)
        {
            int removedCount = _rules.RemoveAll(r => r.Id == id);
            if (removedCount == 0)
            {
                // Không ném lỗi nếu không tìm thấy có thể thân thiện hơn, tùy yêu cầu
                // throw new InvalidOperationException($"Rule with ID {id} not found for deletion.");
                Debug.WriteLine($"[WARN] KeyMappingService: Rule with ID {id} not found for deletion.");
                return;
            }
            SaveRules();
        }

        #endregion

        #region Private Helper Methods

        private void OnGlobalKeyDown(object? sender, KeyEventArgsProcessed e) // Sửa: object? sender
        {
            if (!_isServiceCurrentlyEnabled || e == null) return; // e.KeyData đã được kiểm tra non-null trong constructor của KeyEventArgsProcessed

            var activeRules = _rules.Where(r => r.IsEnabled)
                                    .OrderBy(r => r.Order)
                                    .ThenByDescending(r => r.LastModifiedDate);

            foreach (var rule in activeRules) // rule ở đây là KeyMappingRule (non-null)
            {
                // OriginalKey và TargetActionDetails trong rule được đảm bảo non-null bởi constructor của KeyMappingRule
                if (DoesKeyMatchRule(e.KeyData, rule.OriginalKey!)) // Thêm '!' nếu chắc chắn OriginalKey không null sau khi khởi tạo
                {                                                    // Hoặc kiểm tra rule.OriginalKey != null trước
                    Debug.WriteLine($"[INFO] KeyMappingService: Matched rule '{rule.Name}' for key {e.KeyData.Key}");
                    ExecuteAction(rule.TargetActionDetails!); // Tương tự cho TargetActionDetails
                    e.Handled = true;
                    break;
                }
            }
        }

        private bool DoesKeyMatchRule(OriginalKeyData pressedKey, OriginalKeyData ruleKey)
        {
            // pressedKey và ruleKey được mong đợi là non-null ở đây
            // Constructor của OriginalKeyData đảm bảo Key không phải là null (là Keys.None)
            bool keyMatch = pressedKey.Key == ruleKey.Key;
            bool ctrlMatch = pressedKey.Ctrl == ruleKey.Ctrl;
            bool shiftMatch = pressedKey.Shift == ruleKey.Shift;
            bool altMatch = pressedKey.Alt == ruleKey.Alt;
            bool winMatch = pressedKey.Win == ruleKey.Win;

            return keyMatch && ctrlMatch && shiftMatch && altMatch && winMatch;
        }

        /// <summary>
        /// <para>Executes the target action defined in a key mapping rule.</para>
        /// <para>Thực thi hành động đích được định nghĩa trong một quy tắc ánh xạ phím.</para>
        /// </summary>
        private void ExecuteAction(TargetAction actionDetails) // actionDetails được đảm bảo non-null từ OnGlobalKeyDown
        {
            try
            {
                Debug.WriteLine($"[INFO] Executing action: {actionDetails.Type}, Param: '{actionDetails.ActionParameter}'");
                switch (actionDetails.Type)
                {
                    case ActionType.SendMediaKey:
                        SendMediaKeyAction(actionDetails.ActionParameter);
                        break;
                    case ActionType.RunApplication:
                        RunApplicationAction(actionDetails.ActionParameter);
                        break;
                    case ActionType.OpenUrl:
                        OpenUrlAction(actionDetails.ActionParameter);
                        break;
                    case ActionType.SendText:
                        SendTextAction(actionDetails.ActionParameter ?? string.Empty);
                        break;
                    case ActionType.TriggerHotkeyOrCommand:
                        TriggerHotkeyOrCommandAction(actionDetails.ActionParameter, actionDetails.ActionParameterSecondary);
                        break;
                    case ActionType.None:
                    default:
                        Debug.WriteLine($"[INFO] ActionType is None or unhandled: {actionDetails.Type}");
                        break;
                }
            }
            catch (Exception ex)
            {
                // Nên có cơ chế thông báo lỗi cho người dùng nếu AppSettings.ShowNotifications = true
                Debug.WriteLine($"[ERROR] KeyMappingService: Error executing action '{actionDetails.Type}' with param '{actionDetails.ActionParameter}'. {ex.Message}");
                // Ví dụ: _notificationService.ShowError($"Failed to execute action: {actionDetails.Name}", ex.Message);
            }
        }

        /// <summary>
        /// <para>Sends a media key press and release using SendInput.</para>
        /// <para>Gửi một lệnh nhấn và thả phím media sử dụng SendInput.</para>
        /// </summary>
        private void SendMediaKeyAction(string? mediaKeyName)
        {
            if (string.IsNullOrWhiteSpace(mediaKeyName))
            {
                Debug.WriteLine("[WARN] SendMediaKeyAction: Media key name is null or empty.");
                return;
            }

            ushort vkCode = 0;
            // Chuẩn hóa tên key để so sánh không phân biệt chữ hoa/thường và bỏ dấu cách nếu có
            string normalizedKeyName = mediaKeyName.Replace(" ", "").ToUpperInvariant();

            switch (normalizedKeyName)
            {
                case "VOLUMEMUTE": case "MUTE": vkCode = VK_VOLUME_MUTE; break;
                case "VOLUMEDOWN": vkCode = VK_VOLUME_DOWN; break;
                case "VOLUMEUP": vkCode = VK_VOLUME_UP; break;
                case "MEDIANEXTTRACK": case "NEXT": vkCode = VK_MEDIA_NEXT_TRACK; break;
                case "MEDIAPREVIOUSTRACK": case "PREVIOUS": case "PREV": vkCode = VK_MEDIA_PREV_TRACK; break;
                case "MEDIASTOP": case "STOP": vkCode = VK_MEDIA_STOP; break;
                case "MEDIAPLAYPAUSE": case "PLAYPAUSE": case "PLAY": case "PAUSE": vkCode = VK_MEDIA_PLAY_PAUSE; break;
                // Đại ca có thể thêm các media key khác ở đây nếu cần (ví dụ: VK_BROWSER_*, VK_LAUNCH_MAIL, ...)
                default:
                    // Thử parse nếu mediaKeyName là một số (mã VK)
                    if (!ushort.TryParse(mediaKeyName, out vkCode))
                    {
                        Debug.WriteLine($"[WARN] SendMediaKeyAction: Unknown media key name or invalid VK code '{mediaKeyName}'");
                        return;
                    }
                    break;
            }

            INPUT[] inputs = new INPUT[2];
            IntPtr extraInfo = GetMessageExtraInfo();

            // Key down
            inputs[0].type = INPUT_KEYBOARD;
            inputs[0].U.ki = new KEYBDINPUT
            {
                wVk = vkCode,
                wScan = 0, // 0 for virtual key codes
                // KEYEVENTF_EXTENDEDKEY không thực sự cần cho hầu hết các phím media này, nhưng để cũng không sao.
                dwFlags = 0, // Bỏ KEYEVENTF_EXTENDEDKEY nếu không cần thiết
                time = 0,
                dwExtraInfo = extraInfo
            };

            // Key up
            inputs[1].type = INPUT_KEYBOARD;
            inputs[1].U.ki = new KEYBDINPUT
            {
                wVk = vkCode,
                wScan = 0,
                dwFlags = KEYEVENTF_KEYUP, // Thêm KEYEVENTF_EXTENDEDKEY nếu đã dùng ở key down
                time = 0,
                dwExtraInfo = extraInfo
            };

            uint inputsSent = SendInput((uint)inputs.Length, inputs, INPUT.Size);
            if (inputsSent != inputs.Length)
            {
                Debug.WriteLine($"[ERROR] SendMediaKeyAction: SendInput failed to send all inputs. Error code: {Marshal.GetLastWin32Error()}");
            }
        }

        /// <summary>
        /// <para>Sends a string of text, simulating keyboard typing.</para>
        /// <para>Gửi một chuỗi văn bản, mô phỏng việc gõ phím.</para>
        /// <para>WARNING: This implementation is a placeholder. Full text simulation via SendInput is complex.</para>
        /// <para>CẢNH BÁO: Việc triển khai này là một placeholder. Mô phỏng văn bản đầy đủ qua SendInput rất phức tạp.</para>
        /// </summary>
        private void SendTextAction(string textToSend) // textToSend được đảm bảo non-null từ ExecuteAction
        {
            if (string.IsNullOrEmpty(textToSend)) return;

            Debug.WriteLine($"[INFO] SendTextAction: Action to send text '{textToSend}' triggered.");

            // --- Lựa chọn 1: Sử dụng System.Windows.Forms.SendKeys.SendWait() ---
            // **Ưu điểm**: Đơn giản để triển khai.
            // **Nhược điểm**:
            //    1. Tạo phụ thuộc vào System.Windows.Forms trong project Core (không lý tưởng).
            //    2. Phụ thuộc vào cửa sổ đang có focus. Có thể không hoạt động đúng nếu ứng dụng chạy nền hoặc không có UI focus.
            //    3. Cần xử lý (escape) các ký tự đặc biệt của SendKeys: +, ^, %, ~, (, ), [, ].
            /*
            try
            {
                // Cần thêm using System.Windows.Forms; ở đầu file nếu dùng cách này
                // Và project QuickFnMapper.Core cần tham chiếu đến System.Windows.Forms.dll
                // string escapedText = EscapeSendKeysText(textToSend); // Hàm escape tự viết
                // SendKeys.SendWait(escapedText);
                Debug.WriteLine($"[PLACEHOLDER] Would use SendKeys.SendWait for: {textToSend}. Ensure project references and handles special characters.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] SendTextAction with SendKeys: {ex.Message}");
            }
            */

            // --- Lựa chọn 2: Triển khai bằng SendInput (Phức tạp) ---
            // Gửi từng ký tự một, xử lý Unicode (KEYEVENTF_UNICODE), trạng thái Shift, v.v.
            // Đây là một tác vụ không hề nhỏ.
            // Dưới đây là một ví dụ rất, rất cơ bản chỉ gửi ký tự ASCII không có Shift.
            /*
            List<INPUT> inputs = new List<INPUT>();
            IntPtr extraInfo = GetMessageExtraInfo();

            foreach (char c in textToSend)
            {
                ushort vkCode = (ushort)char.ToUpper(c); // Lấy VK code (rất thô sơ, không đúng cho nhiều ký tự)
                                                       // Cần một hàm MapCharToVkScan đầy đủ hơn.
                
                // Key down
                KEYBDINPUT kd = new KEYBDINPUT();
                kd.wVk = vkCode; 
                // kd.wScan = 0; // Hoặc scan code nếu dùng KEYEVENTF_SCANCODE
                // kd.dwFlags = 0; // Hoặc KEYEVENTF_UNICODE nếu wScan là char code
                kd.time = 0;
                kd.dwExtraInfo = extraInfo;
                inputs.Add(new INPUT { type = INPUT_KEYBOARD, U = new InputUnion { ki = kd } });

                // Key up
                KEYBDINPUT ku = new KEYBDINPUT();
                ku.wVk = vkCode;
                // ku.wScan = 0;
                ku.dwFlags = KEYEVENTF_KEYUP; // Thêm KEYEVENTF_UNICODE nếu dùng ở key down
                ku.time = 0;
                ku.dwExtraInfo = extraInfo;
                inputs.Add(new INPUT { type = INPUT_KEYBOARD, U = new InputUnion { ki = ku } });
            }
            if (inputs.Count > 0)
            {
                SendInput((uint)inputs.Count, inputs.ToArray(), INPUT.Size);
            }
            */
            Debug.WriteLine("[WARN] SendTextAction: Full implementation via SendInput is complex and not provided here. Consider using a dedicated library or a simpler approach like SendKeys (with caveats).");

            // --- Lựa chọn 3: Sử dụng thư viện ngoài (ví dụ: InputSimulatorPlus trên NuGet) ---
            // Thêm thư viện vào project và sử dụng API của nó.
            // Ví dụ (giả sử đã cài InputSimulatorPlus):
            // WindowsInput.InputSimulator sim = new WindowsInput.InputSimulator();
            // sim.Keyboard.TextEntry(textToSend);
        }

        /// <summary>
        /// <para>Runs an application specified by its path.</para>
        /// <para>Chạy một ứng dụng được chỉ định bởi đường dẫn của nó.</para>
        /// </summary>
        private void RunApplicationAction(string? applicationPath)
        {
            if (string.IsNullOrWhiteSpace(applicationPath))
            {
                Debug.WriteLine("[WARN] RunApplicationAction: Application path is null or empty.");
                return;
            }
            try
            {
                Process.Start(applicationPath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] RunApplicationAction: Failed to start application '{applicationPath}'. {ex.Message}");
                // Có thể hiển thị lỗi cho người dùng
            }
        }

        /// <summary>
        /// <para>Opens a URL in the default web browser.</para>
        /// <para>Mở một địa chỉ URL trong trình duyệt web mặc định.</para>
        /// </summary>
        private void OpenUrlAction(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                Debug.WriteLine("[WARN] OpenUrlAction: URL is null or empty.");
                return;
            }
            try
            {
                // Đảm bảo URL có scheme (http, https)
                string processedUrl = url;
                if (!processedUrl.Contains("://"))
                {
                    processedUrl = "http://" + processedUrl;
                }
                Process.Start(new ProcessStartInfo(processedUrl) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] OpenUrlAction: Failed to open URL '{url}'. {ex.Message}");
            }
        }
        /// <summary>
        /// <para>Triggers another hotkey or executes a system command.</para>
        /// <para>Kích hoạt một phím nóng khác hoặc thực thi một lệnh hệ thống.</para>
        /// <para>ActionParameter: The hotkey string (e.g., "^C" for Ctrl+C) or command line.</para>
        /// <para>Tham số Hành động: Chuỗi phím nóng (VD: "^C" cho Ctrl+C) hoặc dòng lệnh.</para>
        /// <para>ActionParameterSecondary: "Hotkey" or "Command" to distinguish.</para>
        /// <para>Tham số Hành động Phụ: "Hotkey" hoặc "Command" để phân biệt.</para>
        /// </summary>
        private void TriggerHotkeyOrCommandAction(string? parameter, string? secondaryParameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                Debug.WriteLine("[WARN] TriggerHotkeyOrCommandAction: Parameter is null or empty.");
                return;
            }

            string type = secondaryParameter?.ToUpperInvariant() ?? "COMMAND"; // Mặc định là Command nếu không rõ

            if (type == "HOTKEY")
            {
                Debug.WriteLine($"[INFO] TriggerHotkeyOrCommandAction: Simulating hotkey: '{parameter}'");
                // Logic để parse 'parameter' (ví dụ: "^C", "%{F4}") và sử dụng SendInput để gửi tổ hợp phím đó.
                // Việc này khá phức tạp, tương tự như SendTextAction nhưng cần phân tích các modifier.
                // Ví dụ rất đơn giản nếu parameter chỉ là một Virtual Key Code dạng số:
                // if (ushort.TryParse(parameter, out ushort vkCode))
                // {
                //    // Gửi vkCode này bằng SendInput (down and up)
                // }
                // else 
                // { // Parse chuỗi như SendKeys (ví dụ: "^" cho Ctrl, "%" cho Alt, "+" cho Shift)
                //    // SendKeys.SendWait(parameter); // Lại là vấn đề của SendKeys
                // }
                Debug.WriteLine("[WARN] TriggerHotkeyOrCommandAction (Hotkey): Full implementation via SendInput is complex. Consider SendKeys or a library.");
            }
            else // Mặc định hoặc type == "COMMAND"
            {
                Debug.WriteLine($"[INFO] TriggerHotkeyOrCommandAction: Executing command: '{parameter}'");
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c \"{parameter}\"", // /c để chạy lệnh rồi thoát
                        WindowStyle = ProcessWindowStyle.Hidden, // Ẩn cửa sổ cmd
                        CreateNoWindow = true, // Không tạo cửa sổ mới
                        UseShellExecute = false // Cần false để redirect output nếu muốn, hoặc để chạy cmd với arguments
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[ERROR] TriggerHotkeyOrCommandAction (Command): Failed to execute command '{parameter}'. {ex.Message}");
                }
            }
        }

        #endregion
    }
}