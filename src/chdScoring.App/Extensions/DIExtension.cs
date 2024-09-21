using Blazored.LocalStorage;
using chdScoring.App.Handler;
using chdScoring.App.Helper;
using chdScoring.App.Services;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.Extensions
{
    public static class DIExtension
    {
        public static IServiceCollection AddChdScoringClient(this IServiceCollection services)
        {
            services.AddBlazoredLocalStorage(opt =>
             {
             });
            services.AddHttpClient<MainService>();

            services.AddSingleton<IDialogHelper, DialogHelper>();
            services.AddSingleton<IKeyHandler, KeyHandler>();
            services.AddSingleton<IVibrationHelper, VibrationHelper>();

            services.AddScoped<ISettingManager, SettingManager>();
            services.AddTransient<IMainService, MainService>();
            services.AddScoped<IJudgeHubClient, JudgeHubClient>();
            services.AddSingleton<IJudgeDataCache, JudgeDataCache>();

            return services;
        }

        private static IServiceCollection AddHttpClient<TService>(this IServiceCollection services, Func<IServiceProvider, Task<UriBuilder>> func) where TService : class
           => services.AddHttpClient(typeof(TService).Name, func.Invoke(services.BuildServiceProvider()).Result.Uri);
        private static IServiceCollection AddHttpClient(this IServiceCollection services, string name, Uri baseAddress)
        {
            services.AddHttpClient(name, client =>
               {
                   client.BaseAddress = baseAddress;
                   client.DefaultRequestHeaders.Accept.Clear();
                   client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
               });
            return services;
        }

        public static HttpClient CreateClient<TService>(this IHttpClientFactory factory) where TService : class
        => factory.CreateClient(nameof(TService));
    }
}
