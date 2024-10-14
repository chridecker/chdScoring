using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using chdScoring.DataAccess.Contracts.Interfaces;
using System.Linq.Expressions;
using System.Linq;

namespace chdScoring.DataAccess.Contracts.Repositories.Base
{
    public interface IBaseEntityRepository<TEntity> : IBaseRepository where TEntity : class
    {
        Task<IEnumerable<TEntity>> FindAll(CancellationToken cancellationToken);
        Task<bool> SaveAsync(TEntity entity, CancellationToken cancellationToken);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression);
    }
}
