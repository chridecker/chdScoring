using chd.UI.Base.Extensions;
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
        void SendNotification(string title, string message, object data, bool autoCloseOnLick = true, DateTime? notifyTime = null);
        void ReceiveNotification(NotificationEventArgs dto);
    }
    public class NotificationEventArgs : EventArgs
    {
        public NotificationEventArgs(int id, string title, string message, object data, bool cancel = true)
        {
            this.Id = id;
            this.Title = title;
            this.Message = message;
            this.Data = data;
            this.Cancel = cancel;
        }
        public int Id { get; set; }
        public string Title { get; }
        public string Message { get; }
        public object Data { get; set; }
        public bool Cancel { get; set; }
    }
}
