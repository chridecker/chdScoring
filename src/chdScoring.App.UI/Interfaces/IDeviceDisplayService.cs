using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.UI.Interfaces
{
    public interface IDeviceDisplayService
    {
        bool KeepScreenOn { get; set; }
        float? ScreenBrightness { get; set; }
    }
}
