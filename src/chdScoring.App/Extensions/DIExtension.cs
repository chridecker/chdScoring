using chd.UI.Base.Client.Extensions;
using chd.UI.Base.Components.Helper;
using chd.UI.Base.Contracts.Interfaces.Authentication;
using chd.UI.Base.Contracts.Interfaces.Services;
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
            services.AddHttpClient();

            services.AddOptions();
            services.AddAuthorizationCore();

            services.AddSingleton<IDialogHelper, DialogHelper>();
            services.AddSingleton<IKeyHandler, KeyHandler>();
            services.AddSingleton<IVibrationHelper, VibrationHelper>();

            services.AddUtilities<chdScoringProfileService, int, int,HandleUserIdLogin, SettingManager, ISettingManager, UiHandler, IBaseUIComponentHandler, UpdateService, BaseFilterHelper>();

            services.AddSingleton<IchdScoringProfileService>(sp => sp.GetRequiredService<chdScoringProfileService>());

            services.AddTransient<IPasswordHashService, PasswordHashService>();

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
                return new Uri("http://localhost:8081/");
            });

            return services;
        }
    }
}
