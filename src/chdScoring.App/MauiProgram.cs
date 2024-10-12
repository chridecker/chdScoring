using Blazored.Modal;
using chdScoring.App.Extensions;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;
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
            var fileName = "appsettings.json";
            var directoryPath = string.Empty;
#if ANDROID
            var docDirectory = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);
            var fullPath = $"{docDirectory.AbsoluteFile.Path}/{fileName}";
#endif
#if WINDOWS
            var fullPath= Path.Combine( "c:\\chdScoring\\", fileName);
#endif
            if (!File.Exists(fullPath))
            {
                var fs = File.Create(fullPath);
                var appSettingsFileName = "chdScoring.App.appsettings.json";
                var assembly = Assembly.GetExecutingAssembly();
                using var resStream = assembly.GetManifestResourceStream(appSettingsFileName);
                if (resStream == null)
                {
                    throw new ApplicationException($"Unable to read file [{appSettingsFileName}]");
                }
                resStream.CopyTo(fs);
            }
            using Stream stream = File.OpenRead(fullPath);
            return new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();
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