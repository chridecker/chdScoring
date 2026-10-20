using System;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.EFCore;
using chdScoring.DataAccess.Repositories.Base;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace chdScoring.DataAccess.Repositories
{
    public class TeilnehmerRepository(
        ILogger<TeilnehmerRepository> logger,
        IContextFactory<chdScoringContext> contextFactory)
        : BaseRepository<Teilnehmer>(logger, contextFactory), ITeilnehmerRepository
    {
        private const int ChangeNr = -99;

        public Task<Teilnehmer> FindById(int teilnehmer, CancellationToken cancellationToken)
        => this._context.Set<Teilnehmer>().FindAsync(teilnehmer, cancellationToken).AsTask();

        public async Task<bool> ChangeStartNumber(int id, int newId, CancellationToken cancellationToken)
        {
            try
            {
                var pilot = await this.FindById(newId, cancellationToken);
                if (pilot is not null && pilot.Id == id)
                {
                    return false;
                }

                var current = await FindById(id, cancellationToken);
                if (pilot is not null)
                {
                    await ChangeId(ChangeNr, pilot.Id, cancellationToken);
                }

                await ChangeId(newId, current.Id, cancellationToken);
                if (pilot is not null)
                {
                    await ChangeId(id, ChangeNr, cancellationToken);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private Task ChangeId(int id, int oldId, CancellationToken cancellationToken)
        => _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE teilnehmer SET id = {id} WHERE id = {oldId};", cancellationToken);

    }
}
