using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Contracts.Interfaces
{
    public interface IBaseRepository
    {
        Task CreateTransaction(CancellationToken cancellationToken);
        Task Commit(CancellationToken cancellationToken);
        Task Rollback(CancellationToken cancellationToken);
        Task SetTransaction(DbTransaction transaction);
    }
}
