using chd.Api.Base.Client.Extensions;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Interfaces;
using chdScoring.Contracts.Settings;
using chdScoring.Main.Client.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.Client.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection AddChdScoringClient<TKeyHandler>(this IServiceCollection services, IConfiguration configuration, Func<IServiceProvider, Uri> func)
        where TKeyHandler : class, IApiKeyHandler
        {
            services.AddTransient<IApiKeyHandler, TKeyHandler>();
            services.AddTransient<ApiKeyHttpInterceptionDelegateHandler>();

            services.AddHttpClient<PrintClient>(sp => func.Invoke(sp).Append(ROOT).Append(Print.ROUTE));
            services.AddTransient<IPrintService, PrintClient>();

            services.AddHttpClient<TimerClient>(sp => func.Invoke(sp).Append(ROOT).Append(Control.ROUTE))
                .AddHttpMessageHandler<ApiKeyHttpInterceptionDelegateHandler>();
            services.AddTransient<ITimerService, TimerClient>();

            services.AddHttpClient<JudgeClient>(sp => func.Invoke(sp).Append(ROOT).Append(Judge.ROUTE))
                .AddHttpMessageHandler<ApiKeyHttpInterceptionDelegateHandler>();
            services.AddTransient<IJudgeService, JudgeClient>();

            services.AddHttpClient<ScoringClient>(sp => func.Invoke(sp).Append(ROOT).Append(Scoring.ROUTE))
                .AddHttpMessageHandler<ApiKeyHttpInterceptionDelegateHandler>();
            services.AddTransient<IScoringService, ScoringClient>();

            services.AddHttpClient<PilotClient>(sp => func.Invoke(sp).Append(ROOT).Append(Pilot.ROUTE))
                .AddHttpMessageHandler<ApiKeyHttpInterceptionDelegateHandler>();
            services.AddTransient<IPilotService, PilotClient>();

            services.AddHttpClient<DatabaseClient>(sp => func.Invoke(sp).Append(ROOT).Append(Database.ROUTE))
                .AddHttpMessageHandler<ApiKeyHttpInterceptionDelegateHandler>();
            services.AddTransient<IDatabaseService, DatabaseClient>();

            services.AddHttpClient<ImportClient>(sp => func.Invoke(sp).Append(ROOT).Append(Import.ROUTE));
            services.AddTransient<IImportService, ImportClient>();
            return services;
        }
    }
}
