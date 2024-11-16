using Blazored.Toast.Services;
using chd.UI.Base.Client.Implementations.Services;
using chdScoring.App.Auth;
using chdScoring.App.Helper;
using chdScoring.App.Services;
using chdScoring.App.UI.Constants;
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
            var t = LoadLocales();
            services.AddKeyedSingleton(SettingConstants.AvailableLanguages, t);

            services.AddChdScoringAppUI<VibrationHelper, UpdateService, SettingManager, BatteryService, TTSService>(configuration);

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


        private static async Task<Dictionary<string, string>> LoadLocales()
        {
            var langs = await TextToSpeech.Default.GetLocalesAsync();
            return langs.GroupBy(s => s.Language, s => s).ToDictionary(d => d.Key, d => $"{d.FirstOrDefault()?.Name} ({d.FirstOrDefault()?.Language})");
        }
    }
}
