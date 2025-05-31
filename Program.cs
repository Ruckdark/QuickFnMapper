using QuickFnMapper.Core.Services;
using QuickFnMapper.WinForms.Views;
// using QuickFnMapper.WinForms.Controllers;
using System;
using System.Windows.Forms;
using QuickFnMapper.WinForms.Utils;

namespace QuickFnMapper.WinForms
{
    internal static class Program
    {
        public static ThemeManager? AppThemeManager { get; private set; }
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // 1. Khởi tạo các service
            IAppSettingsService appSettingsService = new AppSettingsService();
            IGlobalHookService globalHookService = new GlobalHookService(); // Sẽ được dispose bởi MainForm
            IKeyMappingService keyMappingService = new KeyMappingService(globalHookService, appSettingsService);


            AppThemeManager = new ThemeManager(appSettingsService);

            try
            {
                MainForm mainApplicationForm = new MainForm(appSettingsService, globalHookService, keyMappingService, AppThemeManager); // Truyền ThemeManager vào
                Application.Run(mainApplicationForm);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi chi tiết hơn với StackTrace cho dễ debug
                string errorMessage = $"A critical error occurred during application startup: {ex.Message}\n\nStackTrace:\n{ex.StackTrace}";
                MessageBox.Show(errorMessage, "QuickFn Mapper - Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"[FATAL] Application Startup Error: {ex.ToString()}");
            }
            finally
            {
                // globalHookService sẽ được MainForm dispose khi MainForm đóng,
                // vì nó được truyền vào constructor của MainForm và MainForm có logic Dispose cho nó.
            }
        }
    }
}