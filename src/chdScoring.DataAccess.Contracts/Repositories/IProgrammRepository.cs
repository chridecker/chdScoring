using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories.Base;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Contracts.Repositories
{
    public interface IProgrammRepository : IBaseEntityRepository<Programm>
    {
        Task<Programm> FindToRound(int round, CancellationToken cancellationToken);
        Task<Programm> GetProgramToRound(int round, CancellationToken cancellationToken);
    }
}
