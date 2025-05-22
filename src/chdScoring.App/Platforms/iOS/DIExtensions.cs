using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.Platforms.iOS
{
    public static IServiceCollection AddiOS(this IServiceCollection services)
    {

        services.ConfigureHttpClientDefaults(builder => builder.ConfigurePrimaryHttpMessageHandler(HttpsClientHandlerService.GetPlatformMessageHandler));
        services.AddSingleton<NotificationReceiver>();
        services.AddSingleton<INotificationManagerService, NotificationManagerService>();
        return services;
    }
}
