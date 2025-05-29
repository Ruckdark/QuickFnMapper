#region Using Directives
using QuickFnMapper.Core.Models; // Để sử dụng AppSettings
using System;
using System.IO;
using System.Text.Json; // Sử dụng System.Text.Json để làm việc với JSON
using System.Diagnostics; // Để sử dụng Debug.WriteLine cho logging đơn giản
#endregion

namespace QuickFnMapper.Core.Services
{
    /// <summary>
    /// <para>Provides services for loading and saving application settings.</para>
    /// <para>Cung cấp các dịch vụ để tải và lưu cài đặt ứng dụng.</para>
    /// <para>Settings are stored in a JSON file in the application's data directory.</para>
    /// <para>Cài đặt được lưu trữ trong một tệp JSON tại thư mục dữ liệu của ứng dụng.</para>
    /// </summary>
    public class AppSettingsService : IAppSettingsService
    {
        #region Fields
        private readonly string _settingsFilePath; // Phải được khởi tạo non-null trong constructor
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true, // Ghi JSON với định dạng thụt lề cho dễ đọc
            PropertyNameCaseInsensitive = true, // Không phân biệt chữ hoa/thường khi đọc tên thuộc tính
            AllowTrailingCommas = true // Cho phép dấu phẩy cuối cùng trong đối tượng JSON
        };
        #endregion

        #region Constructors
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="AppSettingsService"/> class.</para>
        /// <para>Khởi tạo một đối tượng mới của lớp <see cref="AppSettingsService"/>.</para>
        /// <para>Determines the path for the settings file and ensures the directory exists.</para>
        /// <para>Xác định đường dẫn cho tệp cài đặt và đảm bảo thư mục tồn tại.</para>
        /// </summary>
        /// <exception cref="System.IO.IOException">
        /// <para>Thrown if the application-specific settings directory cannot be created.</para>
        /// <para>Ném ra nếu thư mục cài đặt cụ thể của ứng dụng không thể được tạo.</para>
        /// </exception>
        public AppSettingsService()
        {
            string appDataFolder;
            try
            {
                appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            }
            catch (ArgumentException ex) // SpecialFolder không hợp lệ hoặc không tìm thấy
            {
                Debug.WriteLine($"[ERROR] AppSettingsService: Could not retrieve ApplicationData folder. {ex.Message}");
                // Sử dụng thư mục hiện tại làm fallback, hoặc ném ngoại lệ tùy theo yêu cầu
                appDataFolder = AppDomain.CurrentDomain.BaseDirectory;
            }

            string appSpecificFolder = Path.Combine(appDataFolder, "QuickFnMapper");

            try
            {
                if (!Directory.Exists(appSpecificFolder))
                {
                    Directory.CreateDirectory(appSpecificFolder);
                }
            }
            catch (Exception ex) // Bắt các lỗi chung khi tạo thư mục (IOException, UnauthorizedAccessException, etc.)
            {
                Debug.WriteLine($"[ERROR] AppSettingsService: Error creating application settings directory '{appSpecificFolder}'. {ex.Message}");
                // Ném ngoại lệ hoặc sử dụng một đường dẫn fallback an toàn hơn
                // Tùy thuộc vào yêu cầu, việc không thể tạo thư mục này có thể là một lỗi nghiêm trọng.
                // Ví dụ, có thể sử dụng thư mục của ứng dụng làm fallback:
                // appSpecificFolder = AppDomain.CurrentDomain.BaseDirectory;
                // Tuy nhiên, ghi vào thư mục ứng dụng có thể cần quyền admin. AppData là lựa chọn tốt hơn.
                // Nếu không tạo được, có thể throw để ứng dụng biết và xử lý.
                throw new IOException($"Failed to create application settings directory: {appSpecificFolder}", ex);
            }
            _settingsFilePath = Path.Combine(appSpecificFolder, "AppSettings.json");
        }
        #endregion

        #region IAppSettingsService Implementation

        /// <summary>
        /// <inheritdoc cref="IAppSettingsService.LoadSettings"/>
        /// </summary>
        public AppSettings LoadSettings()
        {
            try
            {
                if (File.Exists(_settingsFilePath))
                {
                    string jsonString = File.ReadAllText(_settingsFilePath);
                    if (!string.IsNullOrWhiteSpace(jsonString))
                    {
                        // Deserialize có thể trả về null nếu JSON là "null"
                        AppSettings? loadedSettings = JsonSerializer.Deserialize<AppSettings>(jsonString, _jsonSerializerOptions);
                        if (loadedSettings != null)
                        {
                            // Đảm bảo RulesFilePath trong loadedSettings không bị null (nếu nó có thể là null từ file JSON)
                            // Mặc dù constructor của AppSettings đã gán giá trị mặc định.
                            if (string.IsNullOrWhiteSpace(loadedSettings.RulesFilePath))
                            {
                                loadedSettings.RulesFilePath = GetDefaultSettings().RulesFilePath;
                            }
                            return loadedSettings;
                        }
                        Debug.WriteLine($"[WARN] AppSettingsService: Deserialized AppSettings from '{_settingsFilePath}' resulted in null. Returning default settings.");
                    }
                    else
                    {
                        Debug.WriteLine($"[INFO] AppSettingsService: Settings file '{_settingsFilePath}' is empty. Returning default settings.");
                    }
                }
                else
                {
                    Debug.WriteLine($"[INFO] AppSettingsService: Settings file '{_settingsFilePath}' not found. Returning default settings.");
                }
            }
            catch (JsonException jsonEx)
            {
                Debug.WriteLine($"[ERROR] AppSettingsService: Error deserializing AppSettings from '{_settingsFilePath}'. {jsonEx.Message}. Returning default settings.");
            }
            catch (IOException ioEx)
            {
                Debug.WriteLine($"[ERROR] AppSettingsService: Error reading AppSettings file '{_settingsFilePath}'. {ioEx.Message}. Returning default settings.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] AppSettingsService: An unexpected error occurred while loading AppSettings. {ex.Message}. Returning default settings.");
            }

            return GetDefaultSettings(); // Luôn trả về một instance non-null
        }

        /// <summary>
        /// <inheritdoc cref="IAppSettingsService.SaveSettings"/>
        /// </summary>
        public void SaveSettings(AppSettings settings)
        {
            // Tuân thủ hợp đồng interface: settings không được null
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings), "Application settings object cannot be null.");
            }

            try
            {
                // Đảm bảo thư mục tồn tại (constructor đã làm, nhưng kiểm tra lại không thừa)
                string? directory = Path.GetDirectoryName(_settingsFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string jsonString = JsonSerializer.Serialize(settings, _jsonSerializerOptions);
                File.WriteAllText(_settingsFilePath, jsonString);
                // Debug.WriteLine($"[INFO] AppSettingsService: Settings saved to '{_settingsFilePath}'.");
            }
            catch (JsonException jsonEx)
            {
                Debug.WriteLine($"[ERROR] AppSettingsService: Error serializing AppSettings to '{_settingsFilePath}'. {jsonEx.Message}");
                // Có thể ném lại một custom exception hoặc xử lý khác
            }
            catch (IOException ioEx)
            {
                Debug.WriteLine($"[ERROR] AppSettingsService: Error writing AppSettings file '{_settingsFilePath}'. {ioEx.Message}");
            }
            catch (UnauthorizedAccessException authEx)
            {
                Debug.WriteLine($"[ERROR] AppSettingsService: Unauthorized access writing AppSettings file '{_settingsFilePath}'. {authEx.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] AppSettingsService: An unexpected error occurred while saving AppSettings. {ex.Message}");
            }
        }

        /// <summary>
        /// <inheritdoc cref="IAppSettingsService.GetDefaultSettings"/>
        /// </summary>
        public AppSettings GetDefaultSettings()
        {
            return new AppSettings(); // Constructor của AppSettings đã có giá trị mặc định non-null
        }

        #endregion
    }
}