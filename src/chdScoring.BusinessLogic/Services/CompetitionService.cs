using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.EFCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace chdScoring.BusinessLogic.Services
{
    public class CompetitionService(IContextFactory<chdScoringContext> contextFactory,
        IHubDataService hubDataService, IFlightCacheService flightCacheService) : ICompetitionService
    {

        public async Task StartAsync(CancellationToken cancellationToken)
        {

        }

        public async Task ClearDataAsync(CancellationToken cancellationToken)
        {
            using var context = contextFactory.Create();
            await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await context.Database.ExecuteSqlRawAsync($"DELETE FROM durchgang", cancellationToken);
                await context.Database.ExecuteSqlRawAsync($"DELETE FROM wettkampf_leitung", cancellationToken);
                await context.Database.ExecuteSqlRawAsync($"DELETE FROM wertung", cancellationToken);
                await context.Database.ExecuteSqlRawAsync($"DELETE FROM wertung_history", cancellationToken);
                await context.Database.ExecuteSqlRawAsync($"DELETE FROM teilnehmer_durchgang_judge", cancellationToken);
                await context.Database.ExecuteSqlRawAsync($"DELETE FROM tbl_result", cancellationToken);
                await context.Database.CommitTransactionAsync(cancellationToken);

                await flightCacheService.Update(cancellationToken);
                await hubDataService.SendAll(cancellationToken);

            }
            catch
            {
                await context.Database.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
