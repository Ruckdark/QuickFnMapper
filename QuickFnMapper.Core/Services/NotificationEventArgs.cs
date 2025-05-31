using System;

namespace QuickFnMapper.Core.Services
{
    /// <summary>
    /// Defines the type of icon for a notification.
    /// Used by Core services to avoid direct dependency on UI-specific enums like System.Windows.Forms.ToolTipIcon.
    /// </summary>
    public enum NotificationType
    {
        None,
        Info,
        Warning,
        Error
    }

    /// <summary>
    /// Provides data for the NotificationRequested event.
    /// </summary>
    public class NotificationEventArgs : EventArgs
    {
        public NotificationInfo Info { get; }

        public NotificationEventArgs(NotificationInfo info)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
        }

        /// <summary>
        /// Contains the details for the notification to be displayed.
        /// </summary>
        public class NotificationInfo
        {
            public string Title { get; }
            public string Message { get; }
            public NotificationType Type { get; }
            public int Timeout { get; } // Optional: timeout for balloon tip, UI can decide to use it or not.

            public NotificationInfo(string title, string message, NotificationType type, int timeout = 3000) // Tăng timeout mặc định một chút
            {
                Title = title ?? string.Empty;
                Message = message ?? string.Empty;
                Type = type;
                Timeout = timeout > 0 ? timeout : 3000; // Đảm bảo timeout dương
            }
        }
    }
}