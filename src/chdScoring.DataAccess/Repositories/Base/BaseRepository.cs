using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Repositories.Base;
using chdScoring.DataAccess.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Repositories.Base
{
    public abstract class BaseRepository<TEntity> : IBaseEntityRepository<TEntity> where TEntity : class
    {
        protected readonly ILogger<BaseRepository<TEntity>> _logger;
        protected readonly chdScoringContext _context;
        protected IDbContextTransaction _currentTransaction;

        protected BaseRepository(ILogger<BaseRepository<TEntity>> logger, IContextFactory<chdScoringContext> contextFactory)
        {
            this._logger = logger;
            this._context = contextFactory.Create();
        }

        public Task Commit(CancellationToken cancellationToken) => this._context.Database.CommitTransactionAsync(cancellationToken);
        public Task Rollback(CancellationToken cancellationToken) => this._context.Database.RollbackTransactionAsync(cancellationToken);
        public async Task SetTransaction(DbTransaction transaction, CancellationToken cancellationToken)
        => this._currentTransaction = await this._context.Database.UseTransactionAsync(transaction, cancellationToken);

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression) => this._context.Set<TEntity>().Where(expression);
        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression) => await this._context.Set<TEntity>().FirstOrDefaultAsync(expression);


        public async Task<DbTransaction> CreateTransaction(CancellationToken cancellationToken)
        {
            if (this._currentTransaction == null)
            {
                this._currentTransaction = await this._context.Database.BeginTransactionAsync(cancellationToken);
            }
            return this._currentTransaction.GetDbTransaction();
        }

        public async Task<bool> SaveAsync(TEntity entity, CancellationToken cancellationToken)
        {
            if (this._context.Entry<TEntity>(entity).State == EntityState.Detached)
            {
                await this._context.AddAsync<TEntity>(entity, cancellationToken);
            }
            else
            {
                this._context.Update<TEntity>(entity);
            }

            return (await this._context.SaveChangesAsync(cancellationToken)) == 1;
        }
        public async Task<IEnumerable<TEntity>> FindAll(CancellationToken cancellationToken) => await this._context.Set<TEntity>().ToListAsync(cancellationToken);


    }
}
