using Blazored.Modal;
using chdScoring.App.Extensions;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace chdScoring.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                 .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            var appsetting = GetAppSettingsConfig();
            builder.Configuration.AddConfiguration(appsetting);
            builder.AddServices();

            return builder.Build();
        }
        private static IConfiguration GetAppSettingsConfig()
        {
            var appSettingsFileName = "chdScoring.App.appsettings.json";

            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(appSettingsFileName))
            {
                if (stream == null)
                {
                    throw new ApplicationException($"Unable to read file [{appSettingsFileName}]");
                }
                return new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();
            }
        }

        private static void AddServices(this MauiAppBuilder builder)
        {
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddBlazoredModal();
            builder.Services.AddBlazorWebViewDeveloperTools();


            builder.Services.AddChdScoringApp(builder.Configuration);
        }
    }
}