using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.DAL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Services
{
    public class FlightCacheService : IFlightCacheService
    {
        private readonly ILogger<FlightCacheService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private CurrentFlight _currentFlight;
        private List<RoundResultDto> _currentRoundResults = [];

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

        public async Task UpdateRoundResults(CancellationToken cancellationToken)
        {
            using var scope = this._serviceProvider.CreateScope();
            var dal = scope.ServiceProvider.GetRequiredService<IPilotService>();
            var res = await dal.GetRoundResult(null, cancellationToken);
            this._currentRoundResults.Clear();
            this._currentRoundResults.AddRange(res);

        }
        public List<RoundResultDto> GetCurrentRoundResults()=> this._currentRoundResults;

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
