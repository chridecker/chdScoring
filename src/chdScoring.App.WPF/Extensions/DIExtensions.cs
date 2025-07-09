using chdScoring.App.UI.Constants;
using chdScoring.App.UI.Extensions;
using chdScoring.App.UI.Interfaces;
using chdScoring.App.WPF.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace chdScoring.App.WPF.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection AddChdScoringApp(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddKeyedSingleton(SettingConstants.AvailableLanguages, Dict());

            services.AddChdScoringAppUI<VibrationHelper, UpdateService, SettingManager, BatteryService, TTSService>(configuration);

            services.AddSingleton<INotificationManagerService, NotificationManagerService>();
            return services;
        }

        private static Task<Dictionary<string, string>> Dict() => Task.FromResult(new Dictionary<string, string>());
    }
}
