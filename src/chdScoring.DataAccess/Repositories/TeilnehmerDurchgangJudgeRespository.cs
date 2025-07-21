using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.Contracts.Repositories.Base;
using chdScoring.DataAccess.EFCore;
using chdScoring.DataAccess.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Repositories
{
    public class Teilnehmer_Durchgang_JudgeRepository : BaseRepository<Teilnehmer_Durchgang_Judge>, ITeilnehmerDurchgangJudgeRespository
    {
        public Teilnehmer_Durchgang_JudgeRepository(ILogger<Teilnehmer_Durchgang_JudgeRepository> logger, IContextFactory<chdScoringContext> contextFactory) : base(logger, contextFactory)
        {
        }
        public Task<bool> Exists(int pilot, int round, int judge, CancellationToken cancellationToken)
            => this._context.Teilnehmer_Durchgang_Judges.AnyAsync(a => a.Teilnehmer == pilot && a.Durchgang == round && a.Judge == judge, cancellationToken);
    }
}
