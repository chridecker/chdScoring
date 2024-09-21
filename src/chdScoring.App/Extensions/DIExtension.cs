using chd.UI.Base.Client.Extensions;
using chdScoring.App.Handler;
using chdScoring.App.Helper;
using chdScoring.App.Services;
using chdScoring.Main.Client.Extensions;

namespace chdScoring.App.Extensions
{
    public static class DIExtension
    {
        public static IServiceCollection AddChdScoringApp(this IServiceCollection services)
        {
            services.AddSingleton<IDialogHelper, DialogHelper>();
            services.AddSingleton<IKeyHandler, KeyHandler>();
            services.AddSingleton<IVibrationHelper, VibrationHelper>();

            services.AddUtilities<chdScoringProfileService, int, int>();

            services.AddScoped<ISettingManager, SettingManager>();
            services.AddTransient<IMainService, MainService>();
            services.AddScoped<IJudgeHubClient, JudgeHubClient>();
            services.AddSingleton<IJudgeDataCache, JudgeDataCache>();

            services.AddChdScoringClient((sp) =>
            {
                try
                {
                    var url = sp.GetRequiredService<ISettingManager>().MainUrl.Result;
                    return new Uri(url);
                }
                catch { }
                return new Uri("http://127.0.0.1:8081/");
            });

            return services;
        }
    }
}
