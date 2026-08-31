using chdScoring.DataAccess.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace chdScoring.DataAccess.Repositories
{
    public class ApiKeyRepository : BaseRepository<ApiKey>, IApiKeyRepository
    {
        public ApiKeyRepository(ILogger<BaseRepository<ApiKey>> logger, IContextFactory<chdScoringContext> contextFactory) : base(logger, contextFactory)
        {
        }

        public Task<bool> Exists(string key, CancellationToken cancellationToken)
            => this._context.Set<ApiKey>().AnyAsync(a => a.Key == key, cancellationToken);
    }
}
