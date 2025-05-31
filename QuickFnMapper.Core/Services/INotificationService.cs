namespace QuickFnMapper.Core.Services
{
    // Sử dụng enum này để Core không cần tham chiếu trực tiếp System.Windows.Forms.ToolTipIcon
    //public enum NotificationType
    //{
    //    None,
    //    Info,
    //    Warning,
    //    Error
    //}

    public interface INotificationService
    {
        /// <summary>
        /// Shows a notification to the user.
        /// </summary>
        /// <param name="title">The title of the notification.</param>
        /// <param name="message">The main message of the notification.</param>
        /// <param name="iconType">The type of icon to display.</param>
        /// <param name="timeout">Duration in milliseconds for the balloon tip to be displayed. Default is 1000ms.</param>
        void ShowNotification(string title, string message, NotificationType iconType = NotificationType.Info, int timeout = 2000);
    }
}