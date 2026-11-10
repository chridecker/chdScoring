using chd.UI.Base.Contracts.Interfaces.Services;
using chdScoring.App.Services;
using chdScoring.App.UI.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.Platforms.Android
{
    public class AndroidSettingManager : AppSettingManager
    {
        public AndroidSettingManager(ILogger<AndroidSettingManager> logger, IBatteryService batteryService, IConfiguration configuration, IProtecedLocalStorageHandler protecedLocalStorageHandler, NavigationManager navigationManager)
            : base(logger, batteryService, configuration, protecedLocalStorageHandler, navigationManager)
        {
        }

        protected override bool _isiOS() => false;
        protected override bool _apiKeyIsEditable() => true;
    }
}
