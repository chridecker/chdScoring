using chd.UI.Base.Client.Implementations.Authorization;
using chd.UI.Base.Contracts.Dtos.Authentication;
using chdScoring.App.Constants;
using chdScoring.Contracts.Interfaces;
using System.Security.Cryptography;

namespace chdScoring.App.Services
{
    public class chdScoringProfileService : ProfileService<int, int>
    {
        private readonly IJudgeService _judgeService;
        private readonly IPasswordHashService _passwordHashService;

        public chdScoringProfileService(IJudgeService judgeService, IPasswordHashService passwordHashService)
        {
            this._judgeService = judgeService;
            this._passwordHashService = passwordHashService;
        }
        protected override async Task<UserPermissionDto<int>> GetPermissions(UserDto<int, int> dto, CancellationToken cancellationToken = default)
        {
            if (dto.Id == RightConstants.AdminId)
            {
                return new UserPermissionDto<int>()
                {
                    UserRightLst = new List<UserRightDto<int>> { new() { Id = RightConstants.Setting, Name = "Einstellungen" } },
                };
            }
            return new UserPermissionDto<int>();
        }

        protected override async Task<UserDto<int, int>> GetUser(LoginDto<int> dto, CancellationToken cancellationToken = default)
        {
            if (dto.Username.ToLower().StartsWith($"judge"))
            {
                dto.Id = int.TryParse(dto.Username.Substring(dto.Username.Length - 1, 1), out var id) ? id : 0;
                var judge = (await this._judgeService.GetJudges(cancellationToken)).FirstOrDefault(x => x.Id == dto.Id && x.Password == dto.Password);
                return new UserDto<int, int>
                {
                    Id = dto.Id,
                    FirstName = judge.Name.Split(' ')[1],
                    LastName = judge.Name.Split(' ')[0],
                };
            }
            else if (dto.Username.ToLower() == "admin" && dto.Password == "ch3510ri")
            {
                return new UserDto<int, int>()
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Id = -1,
                };
            }
            throw new Exception();
        }

    }
}
