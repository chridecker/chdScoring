using chdScoring.BusinessLogic.Services;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var data = this._flightCacheService.GetCurrentFlight();
            await this.Clients.Caller.ReceiveFlightData(data, this.Context.ConnectionAborted);
            await base.OnConnectedAsync();
        }

        public async Task<bool> RegisterAsJudge(int judge)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, $"judge{judge}", this.Context.ConnectionAborted);
            return true;
        }
        public async Task<bool> RegisterAsControlCenter()
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, $"controlcenter", this.Context.ConnectionAborted);
            await this.Clients.Caller.ReceiveFlightData(this._flightCacheService.GetCurrentFlight(), this.Context.ConnectionAborted);
            return true;
        }
    }
}
