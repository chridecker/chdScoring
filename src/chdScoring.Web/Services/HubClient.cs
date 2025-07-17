using chd.Api.Base.Client.Extensions;
using chd.Hub.Base.Client;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.Web.Services
{
    public class HubClient : BaseHubClient<IFlightHub>
    {
        private readonly IConfiguration _configuration;

        public HubClient(ILogger<HubClient> logger, IConfiguration configuration) : base(logger)
        {
            this._configuration = configuration;
        }

        public event EventHandler<CurrentFlight> DataReceived;


        public async Task RegisterControlCenter()
        {
            await this.SendAsync(async (conn)=> await conn.SendAsync(nameof(IFlightHub.RegisterAsControlCenter)));
        }

        protected override async Task DoInvokations(HubConnection connection, CancellationToken cancellationToken)
        {
            
        }

        protected override void HookIncomingCalls(HubConnection connection)
        {
            connection.On<CurrentFlight>(nameof(IFlightHub.ReceiveFlightData), (dto) =>
           {
               this.DataReceived?.Invoke(this, dto);
           });
        }

        protected override Uri LoadUri()
        {
            var baseAddress = this._configuration.GetApiKey("chdScoringApi");
            return new UriBuilder($"{baseAddress}chdscoring/flight-hub").Uri;
        }

        protected override Task<bool> ShouldInitialize(CancellationToken cancellationToken) => Task.FromResult(true);

        protected override void SpecificReinitialize(HubConnection connection)
        {
            connection?.Remove(nameof(IFlightHub.ReceiveFlightData));
        }
    }
}
