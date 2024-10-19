using chdScoring.BusinessLogic.Services;
using chdScoring.Contracts.Interfaces;
using chdScoring.Contracts.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace chdScoring.Main.WebServer.Services
{
    public class chdScoringService : BackgroundService
    {
        private readonly IFlightCacheService _flightCacheService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptionsMonitor<AppSettings> _optionsMonitor;

        public chdScoringService(IOptionsMonitor<AppSettings> optionsMonitor, IFlightCacheService flightCacheService, IServiceProvider serviceProvider)
        {
            this._optionsMonitor = optionsMonitor;
            this._flightCacheService = flightCacheService;
            this._serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.ExecuteSend(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await this._flightCacheService.Update(stoppingToken);
                await Task.Delay(this._optionsMonitor.CurrentValue.RefreshInterval, stoppingToken);
            }
        }
        private void ExecuteSend(CancellationToken cancellationToken) => Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = this._serviceProvider.CreateScope();
                await scope.ServiceProvider.GetService<IHubDataService>().SendAll(cancellationToken);
                await scope.ServiceProvider.GetService<IHubDataService>().RequestStatus(cancellationToken);
                await Task.Delay(this._optionsMonitor.CurrentValue.SendInterval, cancellationToken);
            }
        }, cancellationToken);
    }
}
