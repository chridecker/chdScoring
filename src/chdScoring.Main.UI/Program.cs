using chdScoring.BusinessLogic.Extensions;
using chdScoring.Main.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using WindowsFormsLifetime;
using NLog.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using chdScoring.Main.UI.Services;
using chdScoring.DataAccess.Repositories;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.Repositories.Base;
using chdScoring.DataAccess.Contracts.Interfaces;

Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

Host.CreateDefaultBuilder()
    .UseWindowsFormsLifetime<MainForm>()
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile(path, true, true);
    })
    .ConfigureLogging(builder =>
    {
        builder.ClearProviders();
        builder.AddNLogWeb();
    })
.ConfigureServices((context, services) =>
    {
        services.AddchdScoringDataAccess(context.Configuration);
        services.AddHostedService<chdScoringCacheService>();
    })
.Build().Run();
