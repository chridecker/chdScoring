using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Contracts.Repositories
{
    public interface IWettkampfLeitungRepository : IBaseEntityRepository<Wettkampf_Leitung>
    {
        Task<IEnumerable<Wettkampf_Leitung>> CurrentRoundSet(CancellationToken token);
        Task<Wettkampf_Leitung> GetActiveOnAirfield(int airfield, CancellationToken cancellationToken);
    }
}
