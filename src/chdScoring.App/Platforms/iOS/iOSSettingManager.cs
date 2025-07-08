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

namespace chdScoring.App.Platforms.iOS
{
    public class iOSSettingManager : AppSettingManager
    {
        public iOSSettingManager(ILogger<iOSSettingManager> logger, IConfiguration configuration, IProtecedLocalStorageHandler protecedLocalStorageHandler, NavigationManager navigationManager) : base(logger, configuration, protecedLocalStorageHandler, navigationManager)
        {
        }

        protected override bool _isiOS() => true;
    }
}
