using chd.UI.Base.Client.Implementations.Authorization;
using chd.UI.Base.Contracts.Constants;
using chd.UI.Base.Contracts.Dtos.Authentication;
using chd.UI.Base.Contracts.Interfaces.Authentication;
using chdScoring.App.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Enums;
using chdScoring.Contracts.Interfaces;
using System.Security.Cryptography;

namespace chdScoring.App.Services
{
    public class chdScoringProfileService : ProfileService<int, int>, IchdScoringProfileService
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
            if (dto is csUserDto user)
            {
                if (user.Role == EUserRole.Admin)
                {
                    return new UserPermissionDto<int>()
                    {
                        UserRightLst = new List<UserRightDto<int>> {
                        new() { Id = RightConstants.Setting, Name = "Einstellungen" },
                    new() { Id = RightConstants.ControlBoard, Name = "Scorboard" },
                    new() { Id = RightConstants.CompMgmt, Name = "Comp Mgmt" },
                        new() { Id = RightConstants.Scoring, Name = "Scoring" },
                        new() { Id = RightConstants.UIX, Name = "UIX" }}}
                    };
                }
                else if (user.Role == EUserRole.Judge)
                {
                    return new UserPermissionDto<int>()
                    {
                        UserRightLst = new List<UserRightDto<int>> {
                    new() { Id = RightConstants.Scoring, Name = "Scoring" } },
                    };
                }
            }
            return new UserPermissionDto<int>();
        }

        protected override async Task<UserDto<int, int>> GetUser(LoginDto<int> dto, CancellationToken cancellationToken = default)
        {
            if (dto.Id.HasValue && dto.Id != RightConstants.AdminId)
            {
                var judge = (await this._judgeService.GetJudges(cancellationToken)).FirstOrDefault(x => x.Id == dto.Id);
                return new csUserDto
                {
                    Id = dto.Id.Value,
                    FirstName = judge.Name.Split(' ')[1],
                    LastName = judge.Name.Split(' ')[0],
                    Role = EUserRole.Judge
                };
            }
            else if (dto.Id == RightConstants.AdminId || (dto.Username?.ToLower() == "admin" && dto.Password == "ch3510ri"))
            {
                return new csUserDto
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Id = RightConstants.AdminId,
                    Role = EUserRole.Admin

                };
            }
            else if ((dto.Username?.ToLower() ?? "").StartsWith($"judge"))
            {
                dto.Id = int.TryParse(dto.Username.Substring(dto.Username.Length - 1, 1), out var id) ? id : 0;
                var judge = (await this._judgeService.GetJudges(cancellationToken)).FirstOrDefault(x => x.Id == dto.Id && x.Password == dto.Password)
                     ?? throw new Exception("Kein JJudge gefunden");
                return new csUserDto
                {
                    Id = dto.Id.Value,
                    FirstName = judge.Name.Split(' ')[1],
                    LastName = judge.Name.Split(' ')[0],
                    Role = EUserRole.Judge
                };
            }
            throw new Exception();
        }

    }
    public interface IchdScoringProfileService : IProfileService<int, int> { }
}
