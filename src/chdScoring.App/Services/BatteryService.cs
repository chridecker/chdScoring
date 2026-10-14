using Blazorise.DeepCloner;
using chdScoring.App.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if ANDROID
using Android.Content;
using Android.App;
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

        public event EventHandler InfoChanged;
        public Task ExitApplication(bool restart)
        {
#if ANDROID
            if (restart)
            {
                var context = Platform.CurrentActivity!;

                var intent = context.PackageManager!
                    .GetLaunchIntentForPackage(context.PackageName!);

                intent!.AddFlags(ActivityFlags.ClearTop |
                                 ActivityFlags.NewTask);

                context.StartActivity(intent);
            }
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#endif
            return Task.CompletedTask;
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
