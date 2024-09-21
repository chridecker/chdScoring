using chd.UI.Base.Client.Implementations.Authorization;
using chd.UI.Base.Contracts.Dtos.Authentication;
using chdScoring.Contracts.Interfaces;

namespace chdScoring.App.Services
{
    public class chdScoringProfileService : ProfileService<int, int>
    {
        private readonly IJudgeService _judgeService;

        public chdScoringProfileService(IJudgeService judgeService)
        {
            this._judgeService = judgeService;
        }
        protected override async Task<UserPermissionDto<int>> GetPermissions(UserDto<int, int> dto, CancellationToken cancellationToken = default)
        {
            return new UserPermissionDto<int>();
        }

        protected override async Task<UserDto<int, int>> GetUser(LoginDto<int> dto, CancellationToken cancellationToken = default)
        {
            if (dto.Username == $"Judge{dto.Id}" && dto.Password == $"ch3510ri")
            {
                var judge = (await this._judgeService.GetJudges(cancellationToken)).FirstOrDefault(x => x.Id == dto.Id);
                return new UserDto<int, int>
                {
                    Id = dto.Id,
                    FirstName = judge.Name.Split(' ')[1],
                    LastName = judge.Name.Split(' ')[0],
                };
            }
            throw new Exception();
        }
    }
}
