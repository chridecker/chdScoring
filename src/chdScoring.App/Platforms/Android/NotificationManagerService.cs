using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using AndroidX.Core.App;
using chdScoring.App.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace chdScoring.App.Platforms.Android
{
    public class NotificationManagerService : INotificationManagerService
    {
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";

        public const string TitleKey = "title";
        public const string MessageKey = "message";
        public const string CancelKey = "cancel";
        public const string DataKey = "data";
        public const string DataTypeKey = "datatype";

        bool channelInitialized = false;
        int messageId = 0;
        int pendingIntentId = 0;

        NotificationManagerCompat compatManager;

        public event EventHandler<NotificationEventArgs> NotificationReceived;

        public static NotificationManagerService Instance { get; private set; }

        public NotificationManagerService()
        {
            if (Instance == null)
            {
                this.CreateNotificationChannel();
                this.compatManager = NotificationManagerCompat.From(Platform.AppContext);
                Instance = this;
            }
        }

        public void SendNotification(string title, string message, object data, bool autoCloseOnLick = true, DateTime? notifyTime = null)
        {
            if (!this.channelInitialized)
            {
                this.CreateNotificationChannel();
            }

            if (notifyTime.HasValue)
            {
                var intent = this.CreateIntent(title, message, data, autoCloseOnLick, typeof(AlarmHandler));

                var pendingIntentFlags = (Build.VERSION.SdkInt >= BuildVersionCodes.S)
                    ? PendingIntentFlags.CancelCurrent | PendingIntentFlags.Immutable
                    : PendingIntentFlags.CancelCurrent;

                var pendingIntent = PendingIntent.GetBroadcast(Platform.AppContext, this.pendingIntentId++, intent, pendingIntentFlags);
                var triggerTime = this.GetNotifyTime(notifyTime.Value);
                var alarmManager = Platform.AppContext.GetSystemService(Context.AlarmService) as AlarmManager;
                alarmManager.Set(AlarmType.RtcWakeup, triggerTime, pendingIntent);
            }
            else
            {
                this.Show(title, message, data, autoCloseOnLick);
            }
        }

        public void ReceiveNotification(string title, string message, object data)
        {
            var args = new NotificationEventArgs(title, message, data);
            NotificationReceived?.Invoke(this, args);
        }

        public void Show(string title, string message, object data, bool autoCancel)
        {
            var intent = this.CreateIntent(title, message, data, autoCancel, typeof(MainActivity));

            var pendingIntentFlags = (Build.VERSION.SdkInt >= BuildVersionCodes.S)
                ? PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable
                : PendingIntentFlags.UpdateCurrent;

            var pendingIntent = PendingIntent.GetActivity(Platform.AppContext, this.pendingIntentId++, intent, pendingIntentFlags);
            var builder = new NotificationCompat.Builder(Platform.AppContext, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(Platform.AppContext.Resources, Resource.Drawable.logo_small))
                .SetSmallIcon(Resource.Drawable.logo_small)
                .SetAutoCancel(autoCancel);

            var notification = builder.Build();
            this.compatManager.Notify(this.messageId++, notification);
        }

        private Intent CreateIntent(string title, string message, object data, bool cancel, Type type)
        {
            var intent = new Intent(Platform.AppContext, type);
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);
            intent.PutExtra(CancelKey, cancel);
            intent.PutExtra(DataTypeKey, data.GetType().FullName);
            intent.PutExtra(DataKey, JsonSerializer.Serialize(data));
            intent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTop);
            return intent;
        }

        private void CreateNotificationChannel()
        {
            // Create the notification channel, but only on API 26+.
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = channelDescription
                };
                // Register the channel
                var manager = (NotificationManager)Platform.AppContext.GetSystemService(Context.NotificationService);
                manager.CreateNotificationChannel(channel);
                this.channelInitialized = true;
            }
        }

        private long GetNotifyTime(DateTime notifyTime)
        {
            var utcTime = TimeZoneInfo.ConvertTimeToUtc(notifyTime);
            double epochDiff = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;
            long utcAlarmTime = utcTime.AddSeconds(-epochDiff).Ticks / 10000;
            return utcAlarmTime; // milliseconds
        }
    }
}