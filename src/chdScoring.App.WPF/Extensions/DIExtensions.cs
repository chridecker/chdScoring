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
using chdScoring.App.WPF.Hosting;
using Microsoft.Extensions.Hosting;

namespace chdScoring.App.WPF.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection AddChdScoringApp(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddChdScoringAppUI<VibrationHelper, UpdateService, SettingManager, BatteryService, WifiService>(configuration);

            services.AddSingleton<INotificationManagerService, NotificationManagerService>();
            return services;
        }
        public static IServiceCollection AddWPFWindow<TWindow>(this IServiceCollection services) where TWindow : Window
            => services.AddTransient<TWindow>();

        public static IServiceCollection UseWPFLifeTime<TApp>(this IServiceCollection services)
            where TApp : Application, IInitComponents
        {
            services.AddSingleton<TApp>().AddSingleton(sp => Application.Current);

            services.AddSingleton<IHostLifetime, WPFLifetime>();
            services.AddSingleton<WPFHostingService<TApp>>();
            services.AddHostedService<WPFHostingService<TApp>>(sp => sp.GetRequiredService<WPFHostingService<TApp>>());

            services.AddSingleton<IAppProvider, AppProvider>();

            // Synchronization context
            services.AddSingleton<WPFSynchronizationContextProvider>();
            services.AddSingleton<IWPFSynchronizationContextProvider>(sp => sp.GetRequiredService<WPFSynchronizationContextProvider>());
            services.AddSingleton<IGuiContext>(sp => sp.GetRequiredService<WPFSynchronizationContextProvider>());

            return services;
        }
    }
}
