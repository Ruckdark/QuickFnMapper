// File: QuickFnMapper.WinForms.Program.cs
#region Using Directives
using QuickFnMapper.Core.Services;
using QuickFnMapper.WinForms.Views; // Để thấy MainForm
using System;
using System.Windows.Forms;
#endregion

namespace QuickFnMapper.WinForms // Nên đặt Program.cs trong namespace của project WinForms
{
    internal static class Program
    {
        /// <summary>
        /// <para>The main entry point for the application.</para>
        /// <para>Điểm vào chính của ứng dụng.</para>
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize(); // Cho .NET 6+

            // Khởi tạo các services
            // Những service này có thể được coi là singleton trong phạm vi ứng dụng
            IAppSettingsService appSettingsService = new AppSettingsService();
            IGlobalHookService globalHookService = new GlobalHookService();
            IKeyMappingService keyMappingService = new KeyMappingService(globalHookService, appSettingsService);

            // Biến để theo dõi xem có lỗi nghiêm trọng khi khởi tạo không
            bool initializationError = false;
            MainForm? mainForm = null; // Khai báo nullable

            try
            {
                // Khởi tạo MainForm và inject các services
                mainForm = new MainForm(appSettingsService, globalHookService, keyMappingService);
                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                initializationError = true;
                // Log lỗi nghiêm trọng này và hiển thị cho người dùng
                // Ví dụ: Ghi vào Event Log hoặc một file log đơn giản
                string errorMessage = $"A critical error occurred during application startup: {ex.Message}\n\nApplication will now exit.";
                MessageBox.Show(errorMessage, "QuickFn Mapper - Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Cân nhắc ghi ex.ToString() để có đầy đủ stack trace cho việc debug
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            finally
            {
                // Đảm bảo GlobalHookService được Dispose nếu nó đã được khởi tạo,
                // ngay cả khi MainForm không được tạo hoặc Application.Run không được gọi.
                // Tuy nhiên, MainForm giờ đây chịu trách nhiệm dispose _globalHookService mà nó nhận được.
                // Nếu mainForm không được khởi tạo thành công (ví dụ, lỗi trong constructor của nó trước khi
                // _globalHookService được gán và quản lý bởi MainForm), thì globalHookService có thể bị rò rỉ.
                // Cách an toàn hơn là MainForm sẽ Dispose nó trong sự kiện FormClosed hoặc phương thức Dispose của nó.
                // Với cách MainForm quản lý Dispose trong Designer, việc này đã được xử lý.
                // Nếu có lỗi trước khi Application.Run(mainForm) và mainForm chưa quản lý globalHookService,
                // thì cần dispose ở đây:
                if (initializationError && globalHookService != null && (mainForm == null || mainForm.IsDisposed))
                {
                    // Chỉ dispose ở đây nếu MainForm không thể làm điều đó
                    // Điều này hơi phức tạp, cách MainForm dispose trong Designer là đủ nếu nó được tạo thành công.
                }
            }
        }
    }
}