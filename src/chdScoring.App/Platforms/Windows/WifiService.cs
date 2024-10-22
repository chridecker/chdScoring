using chdScoring.App.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.Platforms.Windows
{
    public class WifiService : IWifiService
    {
        public string SSID => string.Empty;

        public int SignalLevel => 0;

        public string IPAddress => string.Empty;
    }
}
