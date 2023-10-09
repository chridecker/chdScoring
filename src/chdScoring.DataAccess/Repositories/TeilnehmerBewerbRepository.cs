using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.EFCore;
using chdScoring.DataAccess.Repositories.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Repositories
{
    public class TeilnehmerBewerbRepository : BaseRepository<Teilnehmer_Bewerb>, ITeilnehmerBewerbRepository
    {
        public TeilnehmerBewerbRepository(ILogger<TeilnehmerBewerbRepository> logger, chdScoringContext context) : base(logger, context)
        {
        }
    }
}
