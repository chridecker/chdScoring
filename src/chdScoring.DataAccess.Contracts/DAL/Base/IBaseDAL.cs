using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Contracts.DAL.Base
{
    public interface IBaseDAL
    {
        Task CreateTransaction(CancellationToken cancellationToken);
        Task Commit(CancellationToken cancellationToken);
        Task<DbTransaction> GetTransaction();
        Task SetTransaction(DbTransaction dbTransaction, CancellationToken cancellationToken);
        Task RollBack(CancellationToken cancellationToken);

    }
}
