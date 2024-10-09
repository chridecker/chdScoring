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
        public event EventHandler NotificationReceived;

        public NotificationManagerService()
        {
            AppNotificationManager.Default.NotificationInvoked += this.Default_NotificationInvoked;
        }

        private void Default_NotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
        {
        }

        public void ReceiveNotification(string title, string message)
        {
        }

        public void SendNotification(string title, string message, DateTime? notifyTime = null)
        {
            var ap = new AppNotificationBuilder()
                .AddText(title)
                .AddText(message)
                .AddButton(new AppNotificationButton("OK"))
                .SetTimeStamp(notifyTime ?? DateTime.Now)
                .BuildNotification();
            AppNotificationManager.Default.Show(ap);
        }
    }
}
