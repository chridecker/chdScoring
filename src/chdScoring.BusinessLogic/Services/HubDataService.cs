using chdScoring.BusinessLogic.Hubs;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Services
{
    public class HubDataService : IHubDataService
    {
        private readonly IHubContext<FlightHub, IFlightHub> _hub;
        private readonly IFlightCacheService _cacheService;

        public HubDataService(IHubContext<FlightHub, IFlightHub> hub, IFlightCacheService cacheService)
        {
            this._hub = hub;
            this._cacheService = cacheService;
        }

        public async Task SendAll(CancellationToken cancellationToken)
        {
            await this._hub.Clients.All.ReceiveFlightData(this._cacheService.GetCurrentFlight(DateTime.Now), cancellationToken);
        }

        public async Task SendJudge(int judge, CancellationToken cancellationToken)
        {
            await this._hub.Clients.Group($"judge{judge}").ReceiveFlightData(this._cacheService.GetCurrentFlight(DateTime.Now), cancellationToken);
        }
    }
    public interface IHubDataService
    {
        Task SendAll(CancellationToken cancellationToken);
        Task SendJudge(int judge, CancellationToken cancellationToken);
    }
}
