using chdScoring.App.UI.Interfaces;
#if ANDROID
using Android.Content;
using Android.App;
using Android.Media;
#endif

namespace chdScoring.App.Services
{
    public class BatteryService : IBatteryService
    {
        private readonly IBattery _battery;

        public double BatteryLevel => Battery.Default.ChargeLevel * 100;
        public bool? Charging => Battery.Default.PowerSource switch
        {
            BatteryPowerSource.Unknown => null,
            BatteryPowerSource.Battery => false,
            _ => true,
        };

        public BatteryState State => Battery.Default.State;

        public string DeviceName => DeviceInfo.Current.Name;
        public string Platform => DeviceInfo.Platform.ToString();

        public event EventHandler InfoChanged;
        public Task ExitApplication(bool restart)
        {
#if ANDROID
            if (restart)
            {
                var context = Microsoft.Maui.ApplicationModel.Platform.AppContext;

                var intent = context.PackageManager!.GetLaunchIntentForPackage(context.PackageName!)!;
                intent.AddFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);

                var pending = PendingIntent.GetActivity(
                    context, 0, intent,
                    PendingIntentFlags.Immutable | PendingIntentFlags.CancelCurrent);

                var alarm = (AlarmManager)context.GetSystemService(Context.AlarmService)!;
                alarm.Set(AlarmType.Rtc, Java.Lang.JavaSystem.CurrentTimeMillis() + 300, pending);
            }
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#endif
            return Task.CompletedTask;
        }

        public async Task Identify()
        {
#if ANDROID

            var tone = new Android.Media.ToneGenerator(Android.Media.Stream.Alarm, 100);
            try
            {
                tone.StartTone(Android.Media.Tone.PropBeep, 300);
                await Task.Delay(300 + 50);
            }
            finally
            {
                tone.Release();
            }
#endif
        }

        public BatteryService(IBattery battery)
        {
            this._battery = battery;
            this._battery.BatteryInfoChanged += this.Default_BatteryInfoChanged;
        }

        private void Default_BatteryInfoChanged(object sender, BatteryInfoChangedEventArgs e)
        {
            this.InfoChanged?.Invoke(this, e);
        }

        public void Dispose()
        {
            this._battery.BatteryInfoChanged -= this.Default_BatteryInfoChanged;
        }
    }
}
