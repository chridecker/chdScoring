using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Hubs
{
    public class FlightHub : Hub<IFlightHub>
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
    }
}
