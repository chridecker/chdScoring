using chd.UI.Base.Client.Implementations.Authorization;
using chd.UI.Base.Contracts.Dtos.Authentication;
using chdScoring.App.UI.Constants;
using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Enums;
using chdScoring.Contracts.Interfaces;

namespace chdScoring.App.UI.Services
{
    public class chdScoringProfileService(IAuthenticationService authenticationService) : ProfileService<int, int>, IchdScoringProfileService
    {
        public csUserDto? CsUser => this.User is csUserDto cs ? cs : null;

        protected override Task<UserPermissionDto<int>> GetPermissions(UserDto<int, int> dto, CancellationToken cancellationToken = default)
        {
            var perm = new UserPermissionDto<int>();
            if (dto is csUserDto user)
            {
                if (user.Role == EUserRole.Admin)
                {
                    perm.UserRightLst = new List<UserRightDto<int>> {
                        new() { Id = RightConstants.AdminId, Name = "Administrator" },
                        };
                }
                else if (user.Role == EUserRole.Judge)
                {
                    perm.UserRightLst = new List<UserRightDto<int>>();

                }
            }
            return Task.FromResult(perm);
        }

        protected override async Task<UserDto<int, int>> GetUser(LoginDto<int> dto, CancellationToken cancellationToken = default)
        {
            return await authenticationService.GetUserFromApiKeyAsync(cancellationToken)
                   ?? await authenticationService.GetUserAsync(dto, cancellationToken);
        }

    }
}
