﻿using chd.Api.Base.Client;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.Extensions.Logging;

namespace chdScoring.Main.Client.Clients
{
    public class TimerClient : BaseApiService, ITimerService
    {
        public TimerClient(ILogger<TimerClient> logger, IHttpClientFactory httpClientFactory) : base(logger, httpClientFactory)
        {
        }

        public Task<bool> HandleOperation(TimerOperationDto dto, CancellationToken cancellationToken)
       => base.Post<bool>(EndpointConstants.Control.POST_TIMER, dto, cancellationToken);

        public Task<int> GetFinishedRound(CancellationToken cancellationToken)
       => base.Get<int>(EndpointConstants.Control.GET_OpenRound, cancellationToken);

        public Task<bool> SaveRound(SaveRoundDto dto, CancellationToken cancellation)
        => base.Post<bool>(EndpointConstants.Control.POST_SaveRound, dto, cancellation);

        public Task<bool> CalculateRoundTBL(CalcRoundDto dto, CancellationToken cancellation)
        => base.Post<bool>(EndpointConstants.Control.POST_CalcRound, dto, cancellation);
    }
}
