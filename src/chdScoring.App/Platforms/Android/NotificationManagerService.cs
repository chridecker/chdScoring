﻿using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using AndroidX.Core.App;
using chdScoring.App.Services.Base;
using chdScoring.App.UI.Interfaces;
using System.Text.Json;

namespace chdScoring.App.Platforms.Android
{
    public class NotificationManagerService : BaseNotificationManager
    {
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";

        public const string IdKey = "intentid";
        public const string TitleKey = "title";
        public const string MessageKey = "message";
        public const string CancelKey = "cancel";

        bool channelInitialized = false;
        int pendingIntentId = 0;

        NotificationManagerCompat compatManager;


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
        public override void SendNotification(string title, string message, bool autoCloseOnLick = true)
        {
            if (!this.channelInitialized)
            {
                this.CreateNotificationChannel();
            }
            this.Show(title, message, autoCloseOnLick);
        }

        public override void SendNotification<TData>(string title, string message, TData data, bool autoCloseOnLick = true)
        {
            if (!this.channelInitialized)
            {
                this.CreateNotificationChannel();
            }
            this.Show(title, message, data, autoCloseOnLick);
        }

        public override void ReceiveNotification(NotificationEventArgs args)
        {
            if (args.Cancel)
            {
                this.compatManager.Cancel(args.Id);
            }
            this.OnNotificationReceived(args);
        }

        public void Show(string title, string message, bool autoCancel)
        {
            var id = this._messageId++;
            var intent = this.CreateIntent(id, title, message, autoCancel, typeof(MainActivity));
            this.SendIntent(id, intent, title, message, autoCancel);
        }

        private void Show<TData>(string title, string message, TData data, bool autoCancel)
        {
            var id = this._messageId++;
            var intent = this.CreateIntent(id, title, message, autoCancel, typeof(MainActivity));
            if (data is not null) { }
            intent.PutExtra(DataTypeKey, typeof(TData).FullName);
            intent.PutExtra(DataKey, JsonSerializer.Serialize(data));
            this.SendIntent(id, intent, title, message, autoCancel);
        }

        private void SendIntent(int id, Intent intent, string title, string message, bool autoCancel)
        {
            var pendingIntentFlags = (Build.VERSION.SdkInt >= BuildVersionCodes.S)
              ? PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable
          : PendingIntentFlags.UpdateCurrent;

            //        var remoteInput = new  AndroidX.Core.App.RemoteInput.Builder("key_text_reply")
            //.SetLabel("Your answer...")
            //.Build();

            var pendingIntent = PendingIntent.GetActivity(Platform.AppContext, this.pendingIntentId++, intent, pendingIntentFlags);
            //var replyAction = new NotificationCompat.Action.Builder(Resource.Drawable.logo_small, "Reply", pendingIntent)
            //    .AddRemoteInput(remoteInput)
            //    .Build();

            var notification = new NotificationCompat.Builder(Platform.AppContext, channelId)
                //.AddAction(replyAction)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(Platform.AppContext.Resources, Resource.Drawable.navigation_empty_icon))
                .SetSmallIcon(Resource.Drawable.navigation_empty_icon)
                .SetAutoCancel(autoCancel).Build();

            this.compatManager.Notify(id, notification);
        }

        private Intent CreateIntent(int id, string title, string message, bool cancel, Type type)
        {
            var intent = new Intent(Platform.AppContext, type);
            intent.PutExtra(IdKey, id);
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);
            intent.PutExtra(CancelKey, cancel);

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