using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Contracts.Interfaces
{
    public interface IBaseRepository
    {
        Task<DbTransaction> CreateTransaction(CancellationToken cancellationToken);
        Task Commit(CancellationToken cancellationToken);
        Task Rollback(CancellationToken cancellationToken);
        Task SetTransaction(DbTransaction transaction, CancellationToken cancellationToken);
    }
}
