using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.UI.Interfaces
{
    public interface IBatteryService : IDisposable
    {
        string DeviceName { get; }
        double BatteryLevel { get; }
        bool? Charging { get; }

        event EventHandler InfoChanged;
    }
}
