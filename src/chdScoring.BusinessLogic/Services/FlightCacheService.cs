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


        public CurrentFlight GetCurrentFlight() => this._currentFlight;

        public void UpdateScore(SaveScoreDto dto)
        {
            if(this._currentFlight != null && this._currentFlight.ManeouvreLst.ContainsKey(dto.Judge))
            {
                this._currentFlight.ManeouvreLst[dto.Judge].FirstOrDefault(x => x.Id == dto.Figur).Score = dto.Value;
            }
        }
    }

    
}
