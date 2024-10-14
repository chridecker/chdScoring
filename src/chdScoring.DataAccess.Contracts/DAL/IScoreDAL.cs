using chdScoring.Contracts.Dtos;
using chdScoring.DataAccess.Contracts.DAL.Base;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Contracts.DAL
{
    public interface IScoreDAL : IBaseDAL
    {
        Task<NotificationDto> CreateZeroNotification(SaveScoreDto dto);
        Task<bool> SaveScore(SaveScoreDto dto, CancellationToken cancellationToken);
        Task<bool> UpdateScore(SaveScoreDto dto, CancellationToken cancellationToken);
    }
}
