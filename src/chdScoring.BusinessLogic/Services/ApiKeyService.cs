using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;

namespace chdScoring.BusinessLogic.Services
{
    public class ApiKeyService(IApiKeyRepository apiKeyRepository) : IApiKeyService
    {
        public async Task<List<ApiKeyDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var all = await apiKeyRepository.FindAll(cancellationToken);
            return all.Select(s => new ApiKeyDto()
            {
                Id = s.Id,
                JudgeId = s.JudgeId,
                Lastname = s.Lastname,
                Surname = s.Surname,
                Key = s.Key,
                Role = s.Role
            }).ToList();
        }

        public async Task SaveAsync(ApiKeyDto dto, CancellationToken cancellationToken)
        {
            var apiKey = await apiKeyRepository.FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);
            if (apiKey is null)
            {
                apiKey = new()
                {
                    Id = dto.Id,
                    JudgeId = dto.JudgeId,
                    Lastname = dto.Lastname,
                    Surname = dto.Surname,
                    Key = dto.Key,
                    Role = dto.Role
                };
            }
            else
            {
                apiKey.JudgeId = dto.JudgeId;
                apiKey.Lastname = dto.Lastname;
                apiKey.Surname = dto.Surname;
                apiKey.Key = dto.Key;
                apiKey.Role = dto.Role;
            }

            await apiKeyRepository.SaveAsync(apiKey, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var apiKey = await apiKeyRepository.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (apiKey is not null)
            {
                await apiKeyRepository.Delete(apiKey);
            }
        }
    }
}
