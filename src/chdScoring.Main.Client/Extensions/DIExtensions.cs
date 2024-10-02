using chd.Api.Base.Client.Extensions;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Interfaces;
using chdScoring.Main.Client.Clients;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.Client.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection AddChdScoringClient(this IServiceCollection services, Func<IServiceProvider, Uri> func)
        {
            services.AddHttpClient<TimerClient>(sp => func.Invoke(sp).Append(ROOT).Append(Control.ROUTE));

            services.AddTransient<ITimerService, TimerClient>();

            services.AddHttpClient<JudgeClient>(sp => func.Invoke(sp).Append(ROOT).Append(EndpointConstants.Judge.ROUTE));
            services.AddTransient<IJudgeService, JudgeClient>();

            services.AddHttpClient<ScoringClient>(sp => func.Invoke(sp).Append(ROOT).Append(EndpointConstants.Scoring.ROUTE));
            services.AddTransient<IScoringService, ScoringClient>();
            return services;
        }
    }
}
