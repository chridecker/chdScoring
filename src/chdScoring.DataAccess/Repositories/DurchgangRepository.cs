using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.EFCore;
using chdScoring.DataAccess.Repositories.Base;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Repositories
{
    public class DurchgangRepository : BaseRepository<Durchgang>, IDurchgangRepository
    {
        public DurchgangRepository(ILogger<DurchgangRepository> logger, chdScoringContext context) : base(logger, context)
        {
        }

        public async Task<bool> NoramlizeRound(int round, decimal normalizationCore, CancellationToken cancellationToken)
        {
            object[] paramItems = new object[]
            {
                new SqlParameter("@norm", normalizationCore),
                new SqlParameter("@round", round),
            };
            var x = await this._context.Database.ExecuteSqlRawAsync($"UPDATE durchgang SET wert_prom = ROUND((wert_abs / @norm * 1000),2) WHERE durchgang = @round", paramItems, cancellationToken);
            return x == 1;
        }

    }
}
