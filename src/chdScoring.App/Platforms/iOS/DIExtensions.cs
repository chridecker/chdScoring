using chdScoring.App.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.Platforms.iOS
{
    public static class DIExtensions
    {
        public static IServiceCollection AddiOS(this IServiceCollection services)
        {

            services.ConfigureHttpClientDefaults(builder => builder.ConfigurePrimaryHttpMessageHandler(HttpsClientHandlerService.GetPlatformMessageHandler));
            services.AddSingleton<NotificationReceiver>();
            services.AddSingleton<INotificationManagerService, NotificationManagerService>();
            return services;
        }
    }
}
