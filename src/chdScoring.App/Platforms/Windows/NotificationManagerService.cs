using chdScoring.App.UI.Interfaces;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;

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

        public void SendNotification<TData>(string title, string message, TData data, bool cancel = true)
        => this.SendNotification(title, message, cancel);


        public void SendNotification(string title, string message, bool cancel = true)
        {
            var id = this.messageId++;

            var ap = new AppNotificationBuilder()
                .AddArgument(IdKey, id.ToString())
                .AddText(title)
                .AddText(message)
                .AddButton(new AppNotificationButton("OK"))
                .BuildNotification();
            AppNotificationManager.Default.Show(ap);
        }
    }
}
