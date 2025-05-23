﻿using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.EFCore;
using chdScoring.DataAccess.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Repositories
{
    public class DurchgangRepository : BaseRepository<Round>, IDurchgangRepository
    {
        public DurchgangRepository(ILogger<DurchgangRepository> logger,  IContextFactory<chdScoringContext> contextFactory) : base(logger, contextFactory)
        {
        }

        public async Task<bool> NoramlizeRound(int round, decimal normalization, CancellationToken cancellationToken)
        {
            foreach (var dg in await this.Where(x => x.Durchgang == round).ToListAsync(cancellationToken))
            {
                dg.Wert_prom = (double)Math.Round(dg.Wert_abs / normalization, 4) * 1000;
                if (!await this.SaveAsync(dg, cancellationToken))
                {
                    return false;
                }
            }
            return true;
        }

    }
}
