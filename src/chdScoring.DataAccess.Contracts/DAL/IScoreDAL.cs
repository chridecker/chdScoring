using chdScoring.Contracts.Dtos;
using chdScoring.DataAccess.Contracts.DAL.Base;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Contracts.DAL
{
    public interface IScoreDAL : IBaseDAL
    {
        Task<bool> ConfirmScores(ConfirmScoresDto saveScoreDto, CancellationToken cancellationToken);
        Task<NotificationDto> CreateZeroNotification(SaveScoreDto dto);
        Task<bool> HasNotObserved(SaveScoreDto dto, CancellationToken cancellationToken);
        Task<bool> SaveScore(SaveScoreDto dto, CancellationToken cancellationToken);
        Task<bool> TryHandleNotObserved(SaveScoreDto dto, CancellationToken cancellationToken);
        Task<bool> UnConfirmScores(ConfirmScoresDto saveScoreDto, CancellationToken cancellationToken);
        Task<bool> UpdateScore(SaveScoreDto dto, CancellationToken cancellationToken);
        Task<bool> ImportFlight(ImportRoundScoreDto dto, CancellationToken cancellationToken)
    }
}
