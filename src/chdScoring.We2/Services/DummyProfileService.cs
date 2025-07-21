using chd.UI.Base.Client.Implementations.Authorization;
using chd.UI.Base.Contracts.Dtos.Authentication;

namespace chdScoring.Web.Services
{
    public class DummyProfileService : ProfileService<int, int>
    {
        protected override Task<UserPermissionDto<int>> GetPermissions(UserDto<int, int> dto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        protected override Task<UserDto<int, int>> GetUser(LoginDto<int> dto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
