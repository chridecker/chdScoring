using chd.Api.Base.Extensions;
using chd.Base.Hosting.Contracts.Settings;
using chd.Base.Hosting.Modules;
using chdScoring.BusinessLogic.Extensions;
using chdScoring.BusinessLogic.Hubs;
using chdScoring.Main.WebServer.Extensions;
using chdScoring.Main.WebServer.Services;
using Microsoft.Extensions.Options;
using NLog.Web;

namespace chdScoring.Main.WebServer.Modules
{
    public class ServerModule(IOptionsMonitor<WebModuleSettings> optionsMonitor) : BaseWebModule<WebModuleSettings>(optionsMonitor)
    {
        protected override void ConfigureModuleLogging(ILoggingBuilder logging, IServiceProvider appServices)
        {
            logging.AddNLogWeb();
        }

        protected override void ConfigureModuleServices(IServiceCollection services, IConfiguration configuration, IServiceProvider appServices)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost", policy =>
                {
                    policy.SetIsOriginAllowed(_ => true)
                            .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // wichtig für SignalR
                });
            });


            services.AddBaseApi();
            services.AddApiKeyAuth<ApiKeyHandler>();
            services.AddchdScoringDataAccess(configuration,appServices);
            services.AddHostedService<PrintExecuteService>();
            services.AddHostedService<ImportExecuteService>();
            services.AddHostedService<chdScoringService>();

            services.AddSignalR();
        }

        protected override void ConfigureWebModule(WebApplication app)
        {
            app.UseRouting();

            app.UseCors("AllowLocalhost");
            app.UseMiddleware<DatabaseMiddleware>();

            app.MapChdScoring();

            app.UseBaseApi();
            app.MapBaseApi("chdScoringAPI");
            app.MapHub<FlightHub>("/chdScoring/flight-hub").RequireSignalRKeyAuth();
            app.MapHub<DeviceHub>("/chdScoring/device-hub").RequireSignalRKeyAuth();

            app.UseAuthorization();
        }
    }
}
