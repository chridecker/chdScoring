using Blazorise.DeepCloner;
using chdScoring.App.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
