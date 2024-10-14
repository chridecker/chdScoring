using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories.Base;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Contracts.Repositories
{
    public interface ITeilnehmerRepository : IBaseEntityRepository<Teilnehmer>
    {
        Task<Teilnehmer> FindById(int teilnehmer, CancellationToken cancellationToken);
    }
}
