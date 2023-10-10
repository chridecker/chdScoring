using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using chdScoring.DataAccess.Contracts.Interfaces;

namespace chdScoring.DataAccess.Contracts.Repositories.Base
{
    public interface IBaseEntityRepository<TEntity> : IBaseRepository where TEntity : class
    {
        Task<IEnumerable<TEntity>> FindAll(CancellationToken cancellationToken);
        Task<bool> SaveAsync(TEntity entity, CancellationToken cancellationToken);

    }
}
