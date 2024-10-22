using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.UI.Interfaces
{
    public interface IWifiService
    {
        public string SSID { get; }
        public int SignalLevel { get; }
        public string IPAddress { get;  }
    }
}
