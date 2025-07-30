using chdScoring.App.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace chdScoring.App.Platforms.iOS
{
    public class DeviceDisplayService : BaseDeviceDisplayService
    {
        public DeviceDisplayService(IDeviceDisplay deviceDisplay) : base(deviceDisplay)
        {
        }

        protected override float? GetScreenBrightness()
        {
            try
            {
                return (float)UIScreen.MainScreen.Brightness;
            }
            catch
            {
                return null;
            }
        }

        protected override void SetScreenBrightness(float brightness)
        {
            try
            {
                UIScreen.MainScreen.Brightness = brightness;
            }
            catch
            {
            }
        }
    }
}
