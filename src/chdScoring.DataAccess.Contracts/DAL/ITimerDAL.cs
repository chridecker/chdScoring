using chdScoring.Contracts.Dtos;
using chdScoring.DataAccess.Contracts.DAL.Base;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Contracts.DAL
{
    public interface ITimerDAL : IBaseDAL
    {
        Task<int> GetFinishedRound(CancellationToken cancellationToken);
        Task<bool> HandleStart(TimerOperationDto dto, CancellationToken cancellationToken);
        Task<bool>HandleStop(TimerOperationDto dto, CancellationToken cancellationToken);
        Task<bool> SaveRound(SaveRoundDto dto, CancellationToken cancellationToken);
    }
}
