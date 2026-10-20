using System;
using System.Collections.Generic;
using System.Text;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.EFCore;
using Microsoft.Extensions.Logging;

namespace chdScoring.DataAccess.Repositories.Base
{
    public class DefaultRepository<TEntity>(
        ILogger<DefaultRepository<TEntity>> logger,
        IContextFactory<chdScoringContext> contextFactory)
        : BaseRepository<TEntity>(logger, contextFactory)
        where TEntity : class;
}
