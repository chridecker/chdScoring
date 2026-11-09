using System.Reflection;
using chd.Api.Base.Client.Extensions;
using chd.Base.Hosting.Contracts.Settings;
using chd.Base.Hosting.Modules;
using chdScoring.Contracts.Settings;
using chdScoring.Main.Client.Extensions;
using chdScoring.Web.Pages;
using chdScoring.Web.Services;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace chdScoring.Main.WebServer.Modules
{
    public class WebModule(IOptionsMonitor<RclModuleSettings> optionsMonitor) : BaseRclModule<RclModuleSettings>(optionsMonitor)
    {
        protected override void ConfigureModuleLogging(ILoggingBuilder logging, IServiceProvider appServices)
        {
        }

        protected override void ConfigureModuleServices(IServiceCollection services, IConfiguration configuration, IServiceProvider appServices)
        {
            services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost", policy =>
                {
                    policy.SetIsOriginAllowed(_ => true)
                        .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowAnyOrigin()
                          .AllowCredentials(); // wichtig für SignalR
                });
            });


            services.AddChdScoringClient<ApiKeyProvider>(configuration, sp => new Uri(sp.GetService<IOptionsMonitor<AppSettings>>().CurrentValue.LocalApiUrl));
            services.AddSingleton<ImageCache>();
            services.AddSingleton<IconStyleService>(_ => new IconStyleService() { IconStyle = chd.UI.Base.Contracts.Enum.EIconStyle.Regular });
        }

        protected override void ConfigureWebModule(WebApplication app)
        {
        }

        protected override Assembly RclAssembly => typeof(IndexModel).Assembly;
    }
}
