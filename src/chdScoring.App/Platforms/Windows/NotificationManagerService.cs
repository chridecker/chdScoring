using chdScoring.App.Interfaces;
using Microsoft.Maui.Controls;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.Platforms.Windows
{
    public class NotificationManagerService : INotificationManagerService
    {
        public event EventHandler<NotificationEventArgs> NotificationReceived;

        public const string IdKey = "intentid";

        private int messageId = 0;

        public NotificationManagerService()
        {
            AppNotificationManager.Default.NotificationInvoked += this.Default_NotificationInvoked;
        }

        private void Default_NotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
        {
        }

        public void ReceiveNotification(NotificationEventArgs args)
        {
            this.NotificationReceived?.Invoke(this, args);
        }

        public void SendNotification(string title, string message, object data, bool cancel = true, DateTime? notifyTime = null)
        {
            var id = this.messageId++;

            var ap = new AppNotificationBuilder()
                .AddArgument(IdKey, id.ToString())
                .AddText(title)
                .AddText(message)
                .AddButton(new AppNotificationButton("OK"))
                .SetTimeStamp(notifyTime ?? DateTime.Now)
                .BuildNotification();
            AppNotificationManager.Default.Show(ap);
        }
    }
}
