using Blazored.Modal;
using chdScoring.App.UI.Constants;
using chdScoring.App.Extensions;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
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

            builder.Configuration.AddConfiguration(GetAppSettingsConfig());
            builder.Configuration.AddConfiguration(GetLocalSetting());
            builder.AddServices();


            return builder.Build();
        }
        private static IConfiguration GetAppSettingsConfig()
        {
            var fileName = "appsettings.json";
            var appSettingsFileName = "chdScoring.App.appsettings.json";
            var assembly = Assembly.GetExecutingAssembly();
            using var resStream = assembly.GetManifestResourceStream(appSettingsFileName);
            if (resStream == null)
            {
                throw new ApplicationException($"Unable to read file [{appSettingsFileName}]");
            }
            return new ConfigurationBuilder()
                    .AddJsonStream(resStream)
                    .Build();
        }

        private static IConfiguration GetLocalSetting()
        {
            if (Preferences.ContainsKey(SettingConstants.BaseAddress))
            {
                var pref = Preferences.Default.Get<string>(SettingConstants.BaseAddress, string.Empty);
                var dict = new Dictionary<string, string>()
                {
                    {$"ApiKeys:chdScoringApi",pref }
                };
                return new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
            }
            return new ConfigurationBuilder().Build();
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