﻿using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.EFCore;
using chdScoring.DataAccess.Repositories.Base;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Repositories
{
    public class TeilnehmerRepository : BaseRepository<Teilnehmer>, ITeilnehmerRepository
    {
        public TeilnehmerRepository(ILogger<TeilnehmerRepository> logger,  IContextFactory<chdScoringContext> contextFactory): base(logger, contextFactory)
        {
        }

        public Task<Teilnehmer> FindById(int teilnehmer, CancellationToken cancellationToken)
        => this._context.Teilnehmer.FindAsync(teilnehmer,cancellationToken).AsTask();
    }
}
