using chd.UI.Base.Contracts.Interfaces.Services;
using chdScoring.App.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chdScoring.App.UI.Interfaces;

namespace chdScoring.App.Platforms.iOS
{
    public class iOSSettingManager : AppSettingManager
    {
        public iOSSettingManager(ILogger<iOSSettingManager> logger, IBatteryService batteryService, IConfiguration configuration, IProtecedLocalStorageHandler protecedLocalStorageHandler, NavigationManager navigationManager)
            : base(logger, batteryService, configuration, protecedLocalStorageHandler, navigationManager)
        {
        }

        protected override bool _isiOS() => true;
        protected override bool _apiKeyIsEditable() => true;
    }
}
