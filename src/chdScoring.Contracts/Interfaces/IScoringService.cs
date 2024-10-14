using chdScoring.Contracts.Dtos;
using System.Threading.Tasks;
using System.Threading;

namespace chdScoring.Contracts.Interfaces
{
    public interface IScoringService
    {
        Task<bool> SaveScore(SaveScoreDto saveScoreDto, CancellationToken cancellationToken);
        Task<bool> UpdateScore(SaveScoreDto dto, CancellationToken cancellationToken);
    }
}
