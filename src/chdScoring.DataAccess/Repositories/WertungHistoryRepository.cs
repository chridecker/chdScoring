
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.EFCore;
using chdScoring.DataAccess.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Repositories
{
    public class WertungHistoryRepository : BaseRepository<Wertung_History>, IWertungHistoryRepository
    {
        public WertungHistoryRepository(ILogger<WertungHistoryRepository> logger,  IContextFactory<chdScoringContext> contextFactory): base(logger, contextFactory)
        {
        }

    }
}
