using System.Drawing; // Cho Icon, Point etc.
using System.Windows.Forms;
using System.Diagnostics;

namespace QuickFnMapper.WinForms.Utils
{
    public static class UIMessageHelper
    {
        private static string DefaultCaption => "QuickFn Mapper";

        public static void ShowInfo(string message, string? caption = null, Form? owner = null)
        {
            MessageBox.Show(owner, message, caption ?? DefaultCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowWarning(string message, string? caption = null, Form? owner = null)
        {
            MessageBox.Show(owner, message, caption ?? DefaultCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowError(string message, string? caption = null, Form? owner = null)
        {
            MessageBox.Show(owner, message, caption ?? DefaultCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult ShowConfirmation(string message, string? caption = null, Form? owner = null, MessageBoxButtons buttons = MessageBoxButtons.YesNo)
        {
            return MessageBox.Show(owner, message, caption ?? DefaultCaption, buttons, MessageBoxIcon.Question);
        }

        // Có thể thêm các phương thức khác nếu cần
        // Ví dụ: Hiển thị lỗi với chi tiết Exception
        public static void ShowError(string message, Exception ex, string? caption = null, Form? owner = null)
        {
            // Có thể log ex.ToString() ở đây
            Debug.WriteLine($"[ERROR] Exception: {ex.ToString()}");
            string fullMessage = $"{message}\n\nDetails: {ex.Message}";
            MessageBox.Show(owner, fullMessage, caption ?? "Error Encountered", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}