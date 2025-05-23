﻿
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
    public class WertungRepository : BaseRepository<Wertung>, IWertungRepository
    {
        public WertungRepository(ILogger<WertungRepository> logger,  IContextFactory<chdScoringContext> contextFactory): base(logger, contextFactory)
        {
        }

        public async Task<IEnumerable<Wertung>> FindByRound(int round, CancellationToken stoppingToken)
        => await this._context.Wertung.Where(x => x.Durchgang == round).ToListAsync(stoppingToken);

        public async Task<IEnumerable<Wertung>> GetScoresToPilotInRound(int id, int round, CancellationToken cancellationToken)
        => await this._context.Wertung.Where(x => x.Teilnehmer == id && x.Durchgang == round).ToListAsync();

        public Task<bool> Exists(int pilot, int round, int figur, int judge, CancellationToken cancellationToken)
            => this._context.Wertung.AnyAsync(a => a.Teilnehmer == pilot && a.Durchgang == round && a.Figur == figur && a.Judge == judge, cancellationToken);

        public async Task<Wertung> Find(int pilot, int round, int figur, int judge, CancellationToken cancellationToken)
            => await this._context.Wertung.FindAsync(judge, round, figur, pilot, cancellationToken);
    }
}
