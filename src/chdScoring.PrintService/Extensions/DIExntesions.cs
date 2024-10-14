using chdScoring.PrintService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
