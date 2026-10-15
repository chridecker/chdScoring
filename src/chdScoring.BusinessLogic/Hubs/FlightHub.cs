using chd.Hub.Base.Server;
using chdScoring.BusinessLogic.Services;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Domain;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Hubs
{
    public class FlightHub : BaseAuthenticationHub<IFlightHub>, IFlightHub
    {
        private readonly IFlightCacheService _flightCacheService;
        private readonly IApiLogger _apiLogger;

        public FlightHub(IFlightCacheService flightCacheService, ILogger<FlightHub> logger, IApiLogger apiLogger) : base(logger)
        {
            _flightCacheService = flightCacheService;
            _apiLogger = apiLogger;
        }
        public override async Task OnConnectedAsync()
        {
            await this.Clients.Caller.ReceiveFlightData(this._flightCacheService.GetCurrentFlight(DateTime.Now), this.Context.ConnectionAborted);
            await this.Clients.Caller.ReceiveRoundData(this._flightCacheService.GetCurrentRoundResults(), this.Context.ConnectionAborted);

            await base.OnConnectedAsync();
            var time = DateTime.Now;
            await _apiLogger.Log($"{Context.GetHttpContext().Request.Host} connected ID:{Context.ConnectionId}, API-Key:{GetApiKey()}");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task<bool> RegisterAsJudge(int judge)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, $"judge{judge}", this.Context.ConnectionAborted);
            await this.Clients.Caller.ReceiveFlightData(this._flightCacheService.GetCurrentFlight(DateTime.Now), this.Context.ConnectionAborted);
            return true;
        }
        public async Task<bool> RegisterAsControlCenter()
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, $"controlcenter", this.Context.ConnectionAborted);
            await this.Clients.Caller.ReceiveFlightData(this._flightCacheService.GetCurrentFlight(DateTime.Now), this.Context.ConnectionAborted);
            return true;
        }


        public Task ReceiveFlightData(CurrentFlight dto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task ReceiveNotification(NotificationDto dto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task ReceiveRoundData(List<RoundResultDto> dtos, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
