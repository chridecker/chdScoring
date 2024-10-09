using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.Interfaces
{
    public interface INotificationManagerService
    {
        event EventHandler<NotificationEventArgs> NotificationReceived;
        void SendNotification(string title, string message, DateTime? notifyTime = null);
        void ReceiveNotification(string title, string message);
    }
    public class NotificationEventArgs : EventArgs
    {
        public NotificationEventArgs(string title, string message)
        {
            this.Title = title;
            this.Message = message;
        }

        public string Title { get; }
        public string Message { get; }
    }
}
