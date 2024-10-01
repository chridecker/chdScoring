using chdScoring.PrintService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.PrintService.Extensions
{
    public static class DIExntesions
    {
        public static IServiceCollection AddPrint(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IPrintCache,PrintCache>();

            services.AddHostedService<PrintService.Services.PrintService>();

            return services;
        }
    }
}
