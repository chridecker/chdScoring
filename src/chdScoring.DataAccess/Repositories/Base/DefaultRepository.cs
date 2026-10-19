using System;
using System.Collections.Generic;
using System.Text;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.EFCore;
using Microsoft.Extensions.Logging;

namespace chdScoring.DataAccess.Repositories.Base
{
    public class DefaultRepository<TEntity> : BaseRepository<TEntity> where TEntity : class
    {
        public DefaultRepository(ILogger<DefaultRepository<TEntity>> logger, 
            IContextFactory<chdScoringContext> contextFactory) : base(logger, contextFactory)
        {
        }
    }
}
