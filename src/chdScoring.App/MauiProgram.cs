using Blazored.Modal;
using chdScoring.App.UI.Constants;
using chdScoring.App.Extensions;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.Maui.LifecycleEvents;

#if ANDROID
using Maui.Android.InAppUpdates;
#endif


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

            builder.ConfigureLifecycleEvents(events =>
           {
#if ANDROID
               events.AddAndroid(android => android.OnCreate((activity, _) =>
               {

               }));
#elif IOS
               events.AddiOS(iOS => iOS.FinishedLaunching((_, _) =>
               {
                    var updateSvc = IPlatformApplication.Current.Services.GetRequiredService<IUpdateService>();
                    updateSvc.UpdateAsync(0);
                    return false;
               }));
#endif
           });

#if ANDROID
            builder.UseAndroidInAppUpdates(options =>
            {
                options.ImmediateUpdatePriority = 6;
            });
#endif

            return builder.Build();
        }
        private static IConfiguration GetAppSettingsConfig()
        {
            var fileName = "appsettings.txt";
            if (!FileSystem.AppPackageFileExistsAsync(fileName).Result)
            {
                throw new ApplicationException($"Unable to read file [{fileName}]");
            }
            using var stream = FileSystem.OpenAppPackageFileAsync(fileName).Result;
            return new ConfigurationBuilder()
                    .AddJsonStream(stream)
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