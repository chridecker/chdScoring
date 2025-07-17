using chd.Api.Base.Client.Extensions;
using chd.UI.Base.Client.Extensions;
using chd.UI.Base.Contracts.Interfaces.Services;
using chdScoring.Main.Client.Extensions;
using chdScoring.Web.Services;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;

namespace chdScoring.Web.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection AddWebUI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<HubClient>();

            services.AddUtilities<DummyProfileService, int, int, HandleUserIdLogin, SettingManager, ISettingManager, UiHandler, IBaseUIComponentHandler, UpdateService>(ServiceLifetime.Scoped);

            services.AddChdScoringClient((sp) => configuration.GetApiKey("chdScoringApi"));

            return services;
        }
    }
}
