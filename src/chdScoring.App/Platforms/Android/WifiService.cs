using Android.Net.Wifi;
using chdScoring.App.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Android.Icu.Text.IDNA;

namespace chdScoring.App.Platforms.Android
{
    public class WifiService : IWifiService
    {
        public string SSID => this.GetInfo()?.SSID ?? string.Empty;

        public int SignalLevel => WifiManager.CalculateSignalLevel(this.GetInfo()?.Rssi ?? 0, 5);

        public string IPAddress => new IPAddress(this.GetInfo()?.IpAddress ?? 0).ToString();

        private WifiInfo GetInfo()
        {

            var manager = WifiManager.FromContext(MainApplication.Current.ApplicationContext);
            return manager?.ConnectionInfo;
        }
    }
}
