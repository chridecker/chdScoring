using Android.Views;
using chdScoring.App.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.Platforms.Android
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
                var window = Platform.CurrentActivity.Window;
                var attributesWindow = new WindowManagerLayoutParams();

                return window.Attributes.ScreenBrightness;
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
                var window = Platform.CurrentActivity.Window;
                var attributesWindow = new WindowManagerLayoutParams();

                attributesWindow.CopyFrom(window.Attributes);
                attributesWindow.ScreenBrightness = brightness;
                window.Attributes = attributesWindow;
            }
            catch
            {
            }
        }
    }
}
