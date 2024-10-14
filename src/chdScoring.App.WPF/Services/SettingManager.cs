using chd.UI.Base.Client.Implementations.Services.Base;
using chd.UI.Base.Contracts.Interfaces.Services;
using chdScoring.App.UI.Interfaces;
using chdScoring.App.UI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.WPF.Services
{
    public class SettingManager : BaseSettingManager
    {
        public SettingManager(ILogger<SettingManager> logger, IConfiguration configuration, IProtecedLocalStorageHandler protecedLocalStorageHandler, NavigationManager navigationManager)
            : base(logger, configuration, protecedLocalStorageHandler, navigationManager)
        {
        }

        public override T? GetLocalSetting<T>(string key) where T : class
        {
            return default(T);
        }

        public override void SetLocalSetting<T>(string key, T value)
        {
        }
    }
}
