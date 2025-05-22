using Blazored.Toast.Services;
using chd.UI.Base.Client.Implementations.Services;
using chdScoring.App.Helper;
#if ANDROID
using chdScoring.App.Platforms.Android;
#elif IOS
using chdScoring.App.Platforms.iOS;
#endif
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

#if ANDROID
            services.AddAndroidServices();
            services.AddChdScoringAppUI<VibrationHelper, UpdateService, SettingManager, BatteryService, TTSService>(configuration);
#elif IOS
            services.AddiOS();
            services.AddChdScoringAppUI<VibrationHelper, InAppUpdateService, SettingManager, BatteryService, TTSService>(configuration);
#endif

            services.AddSingleton<IDeviceInfo>(_ => DeviceInfo.Current);
            services.AddSingleton<IAppInfo>(_ => AppInfo.Current);
            return services;
        }


        private static async Task<Dictionary<string, string>> LoadLocales()
        {
            var langs = await TextToSpeech.Default.GetLocalesAsync();
            return langs.GroupBy(s => s.Language, s => s).ToDictionary(d => d.Key, d => $"{d.FirstOrDefault()?.Name} ({d.FirstOrDefault()?.Language})");
        }
    }
}
