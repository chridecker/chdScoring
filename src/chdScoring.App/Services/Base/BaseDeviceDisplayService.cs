using chdScoring.App.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.Services.Base
{
    public abstract class BaseDeviceDisplayService : IDeviceDisplayService
    {
        private readonly IDeviceDisplay _deviceDisplay;

        public bool KeepScreenOn
        {
            get => this._deviceDisplay.KeepScreenOn;
            set
            {
                this._deviceDisplay.KeepScreenOn = value;
            }
        }

        public float? ScreenBrightness
        {
            get => this.GetScreenBrightness();
            set
            {
                if (value.HasValue) { this.SetScreenBrightness(value.Value); }
            }
        }

        protected BaseDeviceDisplayService(IDeviceDisplay deviceDisplay)
        {
            this._deviceDisplay = deviceDisplay;
        }

        protected abstract float? GetScreenBrightness();
        protected abstract void SetScreenBrightness(float brightness);
    }
}
