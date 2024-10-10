using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace chdScoring.App.Platforms.Android
{
    [BroadcastReceiver(Enabled = true, Label = "Local Notifications Broadcast Receiver")]
    public class AlarmHandler : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent?.Extras != null)
            {
                string title = intent.GetStringExtra(NotificationManagerService.TitleKey);
                string message = intent.GetStringExtra(NotificationManagerService.MessageKey);
                string type = intent.GetStringExtra(Platforms.Android.NotificationManagerService.DataTypeKey);
                string data = intent.GetStringExtra(Platforms.Android.NotificationManagerService.DataKey);
                var cancel = intent.GetBooleanExtra(Platforms.Android.NotificationManagerService.CancelKey, false);


                object intentData = null;

                if (!string.IsNullOrEmpty(type) && Type.GetType(type) is not null && !string.IsNullOrEmpty(data))
                {
                    var t = Type.GetType(type);
                    intentData = JsonSerializer.Deserialize(data, t);
                }
                var manager = NotificationManagerService.Instance ?? new NotificationManagerService();
                manager.Show(title, message, intentData, cancel);
            }
        }
    }
}
