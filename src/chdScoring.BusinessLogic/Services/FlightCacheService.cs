using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.DAL;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Services
{
    public class FlightCacheService : IFlightCacheService
    {
        private readonly ILogger<FlightCacheService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private CurrentFlight _currentFlight;

        public FlightCacheService(ILogger<FlightCacheService> logger, IServiceProvider serviceProvider)
        {
            this._logger = logger;
            this._serviceProvider = serviceProvider;
        }

        public async Task Update(CancellationToken cancellationToken)
        {
            using var scope = this._serviceProvider.CreateScope();
            var dal = scope.ServiceProvider.GetRequiredService<ICurrentFlightDAL>();
            this._currentFlight = await dal.GetCurrentFlightData(cancellationToken);
        }




        public CurrentFlight GetCurrentFlight(DateTime currentDateTime)
        {
            if (this._currentFlight is null) { return null; }
            var currentTime = currentDateTime.TimeOfDay;

            TimeSpan? time = this._currentFlight.StartTime == TimeSpan.Zero || currentTime < this._currentFlight.StartTime ? null : this._currentFlight.Round.Time - (currentTime - this._currentFlight.StartTime);
            this._currentFlight.LeftTime = time.HasValue && time.Value < TimeSpan.Zero ? TimeSpan.Zero : time;
            return this._currentFlight;
        }
    }


}
