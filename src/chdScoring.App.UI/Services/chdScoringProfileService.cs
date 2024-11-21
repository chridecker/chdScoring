using chd.UI.Base.Client.Implementations.Authorization;
using chd.UI.Base.Contracts.Dtos.Authentication;
using chdScoring.App.UI.Constants;
using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Enums;
using chdScoring.Contracts.Interfaces;

namespace chdScoring.App.UI.Services
{
    public class chdScoringProfileService : ProfileService<int, int>, IchdScoringProfileService
    {
        private readonly IJudgeService _judgeService;

        public chdScoringProfileService(IJudgeService judgeService)
        {
            this._judgeService = judgeService;
        }

        public csUserDto? CsUser => this.User is csUserDto cs ? cs : null;

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
                        new() { Id = RightConstants.UIX, Name = "UIX" },
                        new() { Id = RightConstants.Devices, Name = "Devices" },
                        new() { Id = RightConstants.Database, Name = "Database" },
                        }
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
                    FirstName = "Christoph",
                    LastName = "Decker",
                    Id = RightConstants.AdminId,
                    Role = EUserRole.Admin

                };
            }
            else if ((dto.Username?.ToLower() ?? "").StartsWith($"judge"))
            {
                dto.Id = int.TryParse(dto.Username.Substring(dto.Username.Length - 1, 1), out var id) ? id : 0;
                var judge = (await this._judgeService.GetJudges(cancellationToken)).FirstOrDefault(x => x.Id == dto.Id && x.Password == dto.Password)
                     ?? throw new Exception("Kein Judge gefunden");
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
}
