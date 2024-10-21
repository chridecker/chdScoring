using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.EFCore;
using chdScoring.DataAccess.Repositories.Base;
using Microsoft.Extensions.Logging;

namespace chdScoring.DataAccess.Repositories
{
    public class TeilnehmerBewerbRepository : BaseRepository<Teilnehmer_Bewerb>, ITeilnehmerBewerbRepository
    {
        public TeilnehmerBewerbRepository(ILogger<TeilnehmerBewerbRepository> logger,  IContextFactory<chdScoringContext> contextFactory): base(logger, contextFactory)
        {
        }
    }
}
