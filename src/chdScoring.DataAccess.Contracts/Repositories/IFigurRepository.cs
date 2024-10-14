using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Contracts.Repositories
{
    public interface IFigurRepository : IBaseEntityRepository<Figur>
    {
        Task<IEnumerable<Figur>> GetProgramToRound(int round, CancellationToken cancellationToken);
    }
}
