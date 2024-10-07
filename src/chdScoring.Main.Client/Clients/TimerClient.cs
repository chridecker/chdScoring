using chd.Api.Base.Client;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.Main.Client.Clients
{
    public class TimerClient : BaseApiService, ITimerService
    {
        public TimerClient(ILogger<TimerClient> logger, IHttpClientFactory httpClientFactory) : base(logger, httpClientFactory)
        {
        }

        public Task<bool> HandleOperation(TimerOperationDto dto, CancellationToken cancellationToken)
       => base.Post<bool>(EndpointConstants.Control.POST_TIMER, dto, cancellationToken);

        public Task<bool> SaveRound(SaveRoundDto dto, CancellationToken cancellation)
        => base.Post<bool>(EndpointConstants.Control.POST_SaveRound, dto, cancellation);
    }
}
