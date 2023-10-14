using chdScoring.BusinessLogic.Extensions;
using chdScoring.BusinessLogic.Services;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.Main.UI.Hubs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.Main.UI.Services
{
    public class chdScoringService : BackgroundService
    {
        private readonly IFlightCacheService _flightCacheService;
        private readonly IServiceProvider _serviceProvider;

        public chdScoringService(IFlightCacheService flightCacheService, IServiceProvider serviceProvider)
        {
            this._flightCacheService = flightCacheService;
            this._serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await this._flightCacheService.Update(stoppingToken);
                using var scope = this._serviceProvider.CreateScope();

                    await scope.ServiceProvider.GetService<IHubContext<FlightHub,IFlightHub>>().Clients.All.ReceiveFlightData(this._flightCacheService.GetCurrentFlight());


                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }
}
