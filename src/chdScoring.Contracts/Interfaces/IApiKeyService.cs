using chdScoring.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.Contracts.Interfaces
{
    public interface IApiKeyService
    {
        Task<List<ApiKeyDto>> GetAllAsync(CancellationToken cancellationToken);
        Task SaveAsync(ApiKeyDto dto, CancellationToken cancellationToken); 
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
