using chdScoring.Contracts.Dtos;
using chdScoring.DataAccess.Contracts.DAL.Base;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Contracts.DAL
{
    public interface ICurrentFlightDAL : IBaseDAL
    {

        Task<CurrentFlight> GetCurrentFlightData(CancellationToken cancellationToken);
        Task<RoundDataDto> GetRoundData(int pilot, int round, CancellationToken cancellationToken);
    }
}
