﻿
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.EFCore;
using chdScoring.DataAccess.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Repositories
{
    public class ProgrammRepository : BaseRepository<Programm>, IProgrammRepository
    {
        public ProgrammRepository(ILogger<ProgrammRepository> logger,  IContextFactory<chdScoringContext> contextFactory): base(logger, contextFactory)
        {
        }

        public async Task<Programm> FindToRound(int round, CancellationToken cancellationToken)
        {
            var dg = await this._context.Durchgang_Programm.FirstOrDefaultAsync(x => x.Durchgang == round, cancellationToken);
            return await this._context.Programm.FindAsync(dg.Programm, cancellationToken);
        }

        public async Task<Programm> GetProgramToRound(int round, CancellationToken cancellationToken)
        {
            var pg = await this._context.Durchgang_Programm.FirstOrDefaultAsync(x => x.Durchgang == round, cancellationToken);
            return await this._context.Programm.FindAsync(pg.Programm, cancellationToken);
        }
    }
}
