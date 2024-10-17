using chdScoring.App.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using Microsoft.Web.WebView2.Core;

namespace chdScoring.App.WPF.Services
{
    public class BatteryService : IBatteryService
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();

        private double _level = 0;
        public double BatteryLevel => this._level;

        public bool? Charging => false;

        public event EventHandler InfoChanged;

        public BatteryService()
        {
            this.LeveLoading();
        }


        private void LeveLoading() => Task.Run(async () =>
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                try
                {
                    var level = this.Level();
                    if (level != this._level)
                    {
                        this.InfoChanged?.Invoke(this, EventArgs.Empty);
                    }
                    this._level = level;
                    await Task.Delay(TimeSpan.FromSeconds(10), this._cts.Token);
                }
                catch { }
            }
        });

        private double Level()
        {
            ManagementClass wmi = new ManagementClass("Win32_Battery");
            ManagementObjectCollection allBatteries = wmi.GetInstances();
            double batteryLevel = 0;

            foreach (var battery in allBatteries)
            {
                batteryLevel = Convert.ToDouble(battery["EstimatedChargeRemaining"]);
            }
            return batteryLevel;
        }

        public void Dispose()
        {
            this._cts?.Cancel();
            this.Dispose();
        }
    }
}
