using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows.Forms; // Để dùng Application.ExecutablePath

namespace QuickFnMapper.WinForms.Utils
{
    public static class StartupRegistryHelper
    {
        // Tên giá trị sẽ lưu trong Registry, nên là duy nhất cho ứng dụng
        private const string AppRegistryKeyName = "QuickFnMapper";
        private const string RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        public static bool IsStartupEnabled()
        {
            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryPath, false))
                {
                    if (key == null)
                    {
                        Debug.WriteLine($"[INFO] StartupRegistryHelper: Registry key '{RegistryPath}' not found.");
                        return false;
                    }
                    object? value = key.GetValue(AppRegistryKeyName);
                    return value != null && value.ToString().Equals(Application.ExecutablePath, StringComparison.OrdinalIgnoreCase);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] StartupRegistryHelper: Error checking startup status. {ex.Message}");
                return false;
            }
        }

        public static void SetStartup(bool enableStartup)
        {
            try
            {
                // Mở key Run với quyền ghi
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryPath, true))
                {
                    if (key == null)
                    {
                        // Nếu key không tồn tại (ít khả năng với CurrentVersion\Run), thử tạo nó
                        // Tuy nhiên, thường thì key này luôn có. Nếu không có, có thể có vấn đề lớn hơn.
                        // Hoặc đơn giản là báo lỗi.
                        // Để an toàn, nếu không mở được key, ta không làm gì cả và log lỗi.
                        Debug.WriteLine($"[ERROR] StartupRegistryHelper: Could not open or create registry key '{RegistryPath}' for writing.");
                        // Có thể ném ngoại lệ ở đây để controller xử lý và báo cho người dùng
                        // throw new InvalidOperationException($"Could not open or create registry key '{RegistryPath}' for writing.");
                        return;
                    }

                    if (enableStartup)
                    {
                        // Ghi đường dẫn thực thi của ứng dụng vào Registry
                        key.SetValue(AppRegistryKeyName, Application.ExecutablePath);
                        Debug.WriteLine($"[INFO] StartupRegistryHelper: Application set to start with Windows. Path: {Application.ExecutablePath}");
                    }
                    else
                    {
                        // Xóa giá trị khỏi Registry nếu nó tồn tại
                        if (key.GetValue(AppRegistryKeyName) != null)
                        {
                            key.DeleteValue(AppRegistryKeyName, false); // false: không ném lỗi nếu value không tồn tại
                            Debug.WriteLine($"[INFO] StartupRegistryHelper: Application removed from Windows startup.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] StartupRegistryHelper: Error setting startup status. {ex.Message}");
                // Nên thông báo lỗi này cho người dùng qua UI
                // Ví dụ: throw; // Để controller bắt và hiển thị
            }
        }
    }
}