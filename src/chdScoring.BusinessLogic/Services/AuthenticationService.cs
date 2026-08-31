using chdScoring.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using chd.Api.Base.Contracts.Constants;
using chd.UI.Base.Contracts.Dtos.Authentication;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Enums;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;

namespace chdScoring.BusinessLogic.Services
{
    public class AuthenticationService(IJudgeRepository judgeRepository) : IAuthenticationService
    {
        public async Task<csUserDto> GetUserFromApiKeyAsync(ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            var claim = user.Claims.FirstOrDefault(x => x.Type == ApiKeyConstants.CUSTOM_DATA);
            if (string.IsNullOrWhiteSpace(claim.Value)) { return null; }
            var entry = JsonSerializer.Deserialize<ApiKey>(claim.Value);
            if (entry is null) { return null; }
            if (entry.JudgeId.HasValue)
            {
                var judge = await judgeRepository.FirstOrDefaultAsync(x =>
                                x.Id == entry.JudgeId, cancellationToken)
                            ?? throw new Exception("Kein Judge gefunden");
                return new csUserDto
                {
                    Id = judge.Id,
                    FirstName = judge.Name.Split(' ')[1],
                    LastName = judge.Name.Split(' ')[0],
                    Role = entry.Role
                };
            }
            return new csUserDto
            {
                FirstName = entry.Surname,
                LastName = entry.Lastname,
                Id = RightConstants.AdminId,
                Role = entry.Role

            };
        }
        public Task<csUserDto> GetUserFromApiKeyAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<csUserDto> GetUserAsync(LoginDto<int> dto, CancellationToken cancellationToken)
        {
            if (dto.Id.HasValue)
            {
                var judge = await judgeRepository.FirstOrDefaultAsync(x =>
                    x.Id == dto.Id && x.Pin.ToString("D4") == dto.Password, cancellationToken)
                            ?? throw new Exception("Kein Judge gefunden");
                return new csUserDto
                {
                    Id = dto.Id.Value,
                    FirstName = judge.Name.Split(' ')[1],
                    LastName = judge.Name.Split(' ')[0],
                    Role = EUserRole.Judge
                };
            }
            if (dto.Id == RightConstants.AdminId || (dto.Username?.ToLower() == "admin" && dto.Password == "ch3510ri"))
            {
                return new csUserDto
                {
                    FirstName = "Christoph",
                    LastName = "Decker",
                    Id = RightConstants.AdminId,
                    Role = EUserRole.Admin

                };
            }
            if ((dto.Username?.ToLower() ?? "").StartsWith($"judge"))
            {
                dto.Id = int.TryParse(dto.Username.Trim().Substring(dto.Username.Length - 1, 1), out var id) ? id : 0;
                var judge = await judgeRepository.FirstOrDefaultAsync(x => x.Id == dto.Id && x.Pin.ToString("D4") == dto.Password)
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
