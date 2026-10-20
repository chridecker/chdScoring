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
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Hubs
{
    public class FlightHub(IFlightCacheService flightCacheService, ILogger<FlightHub> logger, IApiLogger apiLogger)
        : BaseAuthenticationHub<IFlightHub>(logger), IFlightHub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await this.Clients.Caller.ReceiveFlightData(flightCacheService.GetCurrentFlight(DateTime.Now), this.Context.ConnectionAborted);
            await this.Clients.Caller.ReceiveRoundData(flightCacheService.GetCurrentRoundResults(), this.Context.ConnectionAborted);

            var context = Context.GetHttpContext();
            var ip = context.Connection.RemoteIpAddress;
            var hostName = string.Empty;
            if (ip != null)
            {
                var hostEntry = await Dns.GetHostEntryAsync(ip);
                hostName = hostEntry.HostName;
            }

            await apiLogger.Log($"{(!string.IsNullOrWhiteSpace(hostName) ? hostName : ip)}({GetApiKey()}) connected ID: {Context.ConnectionId}");
        }

        public async Task<bool> RegisterAsJudge(int judge)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, $"judge{judge}", this.Context.ConnectionAborted);
            await this.Clients.Caller.ReceiveFlightData(flightCacheService.GetCurrentFlight(DateTime.Now), this.Context.ConnectionAborted);
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
