using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Hubs
{
    public class FlightHub : Hub<IFlightHub>, IFlightHub
    {
        private readonly IFlightCacheService _flightCacheService;
        public FlightHub(IFlightCacheService flightCacheService)
        {
            this._flightCacheService = flightCacheService;
        }
        public async override Task OnConnectedAsync()
        {
            await this.Clients.Caller.ReceiveFlightData(this._flightCacheService.GetCurrentFlight(DateTime.Now), this.Context.ConnectionAborted);
            await base.OnConnectedAsync();
        }

        public async Task<bool> RegisterAsStatus()
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, $"status", this.Context.ConnectionAborted);
            await this.Clients.Caller.ReceiveStatusRequest(this.Context.ConnectionAborted);
            return true;
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

        public async Task<bool> SendStatus(DeviceStatusDto dto)
        {
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

        public Task ReceiveStatusRequest(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
