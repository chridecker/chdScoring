using chd.Base.UI.WPF.Hosting;
using chdScoring.App.UI.Interfaces;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.WPF.Services
{
    public class BatteryService : IBatteryService
    {
        private readonly IGuiContext _guiContext;
        private readonly App _app;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        private double _level = 0;
        public double BatteryLevel => this._level;
        public string DeviceName => Environment.MachineName;

        public bool? Charging => false;

        public event EventHandler InfoChanged;


        public BatteryService(IGuiContext guiContext, App app)
        {
            _guiContext = guiContext;
            _app = app;
            this.LeveLoading();
        }

        public Task ExitApplication(bool restart)
        {
            if (restart)
            {
                Process.Start(Environment.ProcessPath!);
            }
            this._guiContext.Invoke(_app.Shutdown);
            return Task.CompletedTask;
        }

        public Task Identify()
        {
            System.Media.SystemSounds.Beep.Play();
            return Task.CompletedTask;
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
        }
    }
}
