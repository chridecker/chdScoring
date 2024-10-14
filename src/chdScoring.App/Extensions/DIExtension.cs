using chd.Api.Base.Client.Extensions;
using chd.UI.Base.Client.Extensions;
using chd.UI.Base.Client.Implementations.Services;
using chd.UI.Base.Contracts.Interfaces.Services;
using chdScoring.App.Auth;
using chdScoring.App.Handler;
using chdScoring.App.Helper;
using chdScoring.App.Interfaces;
using chdScoring.App.Services;
using chdScoring.Main.Client.Extensions;
using Microsoft.Extensions.Configuration;

namespace chdScoring.App.Extensions
{
    public static class DIExtension
    {
        public static IServiceCollection AddChdScoringApp(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorizationCore();

            services.AddUtilities<chdScoringProfileService, int, int, HandleUserIdLogin, SettingManager, ISettingManager, UiHandler, IBaseUIComponentHandler, UpdateService>();

            services.AddMauiModalHandler();

            services.AddSingleton<IDialogHelper, DialogHelper>();
            services.AddSingleton<IKeyHandler, KeyHandler>();
            services.AddSingleton<IVibrationHelper, VibrationHelper>();

            services.AddSingleton<IAppInfoService, AppInfoService>();

            services.AddSingleton<IchdScoringProfileService>(sp => sp.GetRequiredService<chdScoringProfileService>());


            /* State Container Singletons */
            services.AddSingleton<INavigationHistoryStateContainer, NavigationHistoryStateContainer>();

            /* Scoped */
            services.AddScoped<INavigationHandler, NavigationHandler>();

            services.AddScoped<IJudgeHubClient, JudgeHubClient>();
            services.AddSingleton<IJudgeDataCache, JudgeDataCache>();

#if ANDROID
            services.ConfigureHttpClientDefaults(builder => builder.ConfigurePrimaryHttpMessageHandler(HttpsClientHandlerService.GetPlatformMessageHandler));
#endif

            services.AddNotification();
            services.AddChdScoringClient((sp) => configuration.GetApiKey("chdScoringApi"));
            return services;
        }

        private static IServiceCollection AddNotification(this IServiceCollection services)
        {
#if ANDROID
            services.AddSingleton<INotificationManagerService, Platforms.Android.NotificationManagerService>();
#endif        
#if WINDOWS
            services.AddSingleton<INotificationManagerService, Platforms.Windows.NotificationManagerService>();
#endif

            return services;
        }
    }
}
