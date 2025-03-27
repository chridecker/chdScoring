using chdScoring.App.UI.Extensions;
using chdScoring.App.UI.Interfaces;
using chdScoring.App.WPF.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows;
using Microsoft.Extensions.Hosting;

namespace chdScoring.App.WPF.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection AddChdScoringApp(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddChdScoringAppUI<VibrationHelper, UpdateService, SettingManager, BatteryService, TTSService>(configuration);

            services.AddSingleton<INotificationManagerService, NotificationManagerService>();
            return services;
        }
    }
}
