using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using chd.UI.Base.Contracts.Dtos.Authentication;
using chdScoring.Contracts.Dtos;

namespace chdScoring.Contracts.Interfaces
{
    public interface IAuthenticationService : IAuthenticationClient
    {
        Task<csUserDto> GetUserFromApiKeyAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default);
    }

    public interface IAuthenticationClient
    {
        Task<csUserDto> GetUserFromApiKeyAsync(CancellationToken cancellationToken);
        Task<csUserDto> GetUserAsync(LoginDto<int> dto, CancellationToken cancellationToken);

    }
}
