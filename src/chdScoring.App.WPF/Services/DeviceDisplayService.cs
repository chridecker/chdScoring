using chdScoring.App.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.WPF.Services
{
    public class DeviceDisplayService : IDeviceDisplayService
    {
        public bool KeepScreenOn { get; set; }
        public float? ScreenBrightness { get; set; }
    }
}
