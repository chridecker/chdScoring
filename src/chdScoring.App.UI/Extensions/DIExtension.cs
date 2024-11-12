using chd.Api.Base.Client.Extensions;
using chd.UI.Base.Client.Extensions;
using chd.UI.Base.Client.Implementations.Services;
using chd.UI.Base.Client.Implementations.Services.Base;
using chd.UI.Base.Contracts.Interfaces.Services;
using chd.UI.Base.Contracts.Interfaces.Services.Base;
using chdScoring.App.UI.Handler;
using chdScoring.App.UI.Helper;
using chdScoring.App.UI.Interfaces;
using chdScoring.App.UI.Services;
using chdScoring.Main.Client.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace chdScoring.App.UI.Extensions
{
    public static class DIExtension
    {
        public static IServiceCollection AddChdScoringAppUI<TVibrationHelper, TUpdateService, TSettingManager, TBatteryService, TTTS>(this IServiceCollection services, IConfiguration configuration, ServiceLifetime profileServiceLifeTime = ServiceLifetime.Singleton)
            where TVibrationHelper : class, IVibrationHelper
            where TSettingManager : BaseSettingManager, ISettingManager
            where TUpdateService : BaseUpdateService
            where TBatteryService : class, IBatteryService
            where TTTS : class, ITTSService
        {
            services.AddAuthorizationCore();

            services.AddUtilities<chdScoringProfileService, int, int, HandleUserIdLogin, TSettingManager, ISettingManager, UiHandler, IBaseUIComponentHandler, TUpdateService>(profileServiceLifeTime);

            services.Add(new(typeof(IDeviceStatusService), typeof(DeviceStatusService), profileServiceLifeTime));

            services.AddMauiModalHandler();

            services.AddSingleton<IKeyHandler, KeyHandler>();
            services.AddSingleton<IVibrationHelper, TVibrationHelper>();
            services.AddSingleton<IBatteryService, TBatteryService>();

            services.AddSingleton<IAppInfoService, AppInfoService>();
            services.Add(new(typeof(ITTSService), typeof(TTTS), ServiceLifetime.Scoped));

            services.AddSingleton<IchdScoringProfileService>(sp => sp.GetRequiredService<chdScoringProfileService>());

            /* State Container Singletons */
            services.AddSingleton<INavigationHistoryStateContainer, NavigationHistoryStateContainer>();

            /* Scoped */
            services.AddScoped<INavigationHandler, NavigationHandler>();

            services.AddScoped<IJudgeHubClient, JudgeHubClient>();
            services.AddSingleton<IJudgeDataCache, JudgeDataCache>();

            services.AddChdScoringClient((sp) => configuration.GetApiKey("chdScoringApi"));
            return services;
        }
    }
}
