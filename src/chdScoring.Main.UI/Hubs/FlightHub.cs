using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.Main.UI.Hubs
{
    public class FlightHub : Hub<IFlightHub>
    {
        public FlightHub()
        {

        }
        public async Task<bool> RegisterAsJudge(int judge, CancellationToken cancellationToken = default)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, $"judge{judge}", cancellationToken);
            return true;
        }




    }
}
