using chdScoring.BusinessLogic.Extensions;
using chdScoring.BusinessLogic.Services;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
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
        private readonly ITBLCalculationService _tBLCalculationService;

        public chdScoringService(IFlightCacheService flightCacheService, ITBLCalculationService tBLCalculationService)
        {
            this._flightCacheService = flightCacheService;
            this._tBLCalculationService = tBLCalculationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                Task.WaitAll(this._flightCacheService.Update(stoppingToken), this._tBLCalculationService.Calc(stoppingToken));

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }
}
