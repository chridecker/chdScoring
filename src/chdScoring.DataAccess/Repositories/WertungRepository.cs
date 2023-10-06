
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.EFCore;
using chdScoring.DataAccess.Repositories.Base;
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
    public class WertungRepository : BaseRepository<Wertung>, IWertungRepository
    {
        public WertungRepository(ILogger<WertungRepository> logger, chdScoringContext context) : base(logger, context)
        {
        }

        public async Task<IEnumerable<Wertung>> FindByRound(int round, CancellationToken stoppingToken)
        => await this._context.Wertung.Where(x => x.Durchgang == round).ToListAsync(stoppingToken);
    }
}
