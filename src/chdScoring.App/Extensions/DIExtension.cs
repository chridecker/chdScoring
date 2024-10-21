using Blazored.Toast.Services;
using chd.UI.Base.Client.Implementations.Services;
using chdScoring.App.Auth;
using chdScoring.App.Helper;
using chdScoring.App.Services;
using chdScoring.App.UI.Extensions;
using chdScoring.App.UI.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace chdScoring.App.Extensions
{
    public static class DIExtension
    {
        public static IServiceCollection AddChdScoringApp(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddChdScoringAppUI<VibrationHelper, UpdateService, SettingManager, BatteryService>(configuration);


#if ANDROID
            services.ConfigureHttpClientDefaults(builder => builder.ConfigurePrimaryHttpMessageHandler(HttpsClientHandlerService.GetPlatformMessageHandler));
#endif

            services.AddNotification();
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
