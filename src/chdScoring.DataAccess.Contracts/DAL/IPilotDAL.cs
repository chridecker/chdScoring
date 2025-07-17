using chdScoring.Contracts.Dtos;
using chdScoring.DataAccess.Contracts.DAL.Base;
using chdScoring.DataAccess.Contracts.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Contracts.DAL
{
    public interface IPilotDAL : IBaseDAL
    {
        Task<Country_Images> GetCountryImage(int id, CancellationToken cancellationToken);
        Task<IEnumerable<FinishedRoundDto>> GetFinishedFlights(CancellationToken cancellationToken);
        Task<IEnumerable<OpenRoundDto>> LoadOpenPilots(int? round, CancellationToken cancellationToken);
        Task<IEnumerable<RoundResultDto>> LoadRoundResults(int? round, CancellationToken cancellationToken);
        Task<bool> SetPilotActive(LoadPilotDto dto, CancellationToken cancellationToken);
        Task<bool> UnLoadPilot(LoadPilotDto dto, CancellationToken cancellationToken);
    }
}
