using chd.Api.Base.Client;
using chdScoring.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using chd.UI.Base.Contracts.Dtos.Authentication;
using chdScoring.Contracts.Dtos;
using Microsoft.Extensions.Logging;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.Client.Clients
{
    public class AuthenticationClient : BaseApiService, IAuthenticationClient
    {
        public AuthenticationClient(ILogger<AuthenticationClient> logger, IHttpClientFactory httpClientFactory) : base(logger, httpClientFactory)
        {
        }

        public Task<csUserDto> GetUserFromApiKeyAsync(CancellationToken cancellationToken)
            => Get<csUserDto>(Authentication.USER_KEY, cancellationToken);

        public Task<csUserDto> GetUserAsync(LoginDto<int> dto, CancellationToken cancellationToken)
            => this.Post<csUserDto>(Authentication.USER, dto, cancellationToken);
    }
}
