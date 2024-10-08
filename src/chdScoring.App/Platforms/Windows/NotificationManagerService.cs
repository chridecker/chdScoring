using chdScoring.App.Interfaces;
using Microsoft.Maui.Controls;
using Microsoft.Windows.AppNotifications;
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

        public void ReceiveNotification(string title, string message)
        {
        }

        public void SendNotification(string title, string message, DateTime? notifyTime = null)
        {
        }
    }
}
