using chdScoring.Contracts.Enums;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Services
{
    public class CompetitionService(
        ITeilnehmerRepository teilnehmerRepository,
        IStammDatenRepository stammDatenRepository,
        IWettkampfLeitungRepository wettkampfLeitungRepository,
        IDurchgangRepository durchgangRepository,
        IContextFactory<chdScoringContext> contextFactory,
        IHubDataService hubDataService, IFlightCacheService flightCacheService) : ICompetitionService
    {

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var stamm = await stammDatenRepository.FirstOrDefaultAsync(_ => true, cancellationToken);
            var wl = await wettkampfLeitungRepository.FindAll(cancellationToken);

            if (!wl.Any())
            {
                await IntialStartOrderAsync(stamm, cancellationToken);
            }
            else if (stamm.Finale == 1
                     && wl.All(x => x.Durchgang <= (stamm.End_Finale == 1 ? stamm.End_Final_Durchgang - 1 : stamm.Durchgaenge) && x.Status >= (int)EFlightState.Saved))
            {
                await SemiFinalStartOrderAsync(stamm, wl.Max(s => s.Durchgang) + 1, cancellationToken);
            }
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

        private async Task SemiFinalStartOrderAsync(Stammdaten stamm, int round, CancellationToken cancellationToken)
        {
            var roundResults = await durchgangRepository.FindAll(cancellationToken);
            var start = 0;

            foreach (var pilotId in roundResults.GroupBy(g => g.Teilnehmer).ToDictionary(d => d.Key,
                             d => (d.ToList().Sum(s => s.Wert_prom) - d.ToList().Min(s => s.Wert_prom)))
                         .OrderByDescending(o => o.Value)
                         .Take(stamm.Final_Teilnehmer)
                         .Select(s => s.Key))
            {
                start++;
                var wl = new Wettkampf_Leitung()
                {
                    Start = start,
                    Teilnehmer = pilotId,
                    Durchgang = round,
                    Status = 0,
                    Start_Time = TimeSpan.Zero
                };
                await wettkampfLeitungRepository.SaveAsync(wl, cancellationToken);
            }

        }

        private async Task IntialStartOrderAsync(Stammdaten stamm, CancellationToken cancellationToken)
        {
            var participants = await teilnehmerRepository.Count(cancellationToken);
            var rounds = stamm.Finale == 1 ? stamm.Final_Durchgang - 1 : stamm.Durchgaenge;

            var leftOver = participants % rounds;
            var count = participants / rounds;

            var group = 0;
            var groups = new Dictionary<int, Dictionary<int, int>>();
            var t = 0;
            var participantLst = await teilnehmerRepository.FindAll(cancellationToken);
            int[] participantArray = participantLst.Select(s => s.Id).ToArray();
            for (int d = 0; d < rounds; d++)
            {
                if (!groups.ContainsKey(d))
                {
                    groups[d] = new Dictionary<int, int>();
                }
                for (var i = 0; i < count; i++)
                {
                    if (t < participants)
                    {
                        groups[d][i] = participantArray[t];
                    }
                    t++;
                }
                if (d == (rounds - 1) && leftOver == 1)
                {
                    groups[d][count] = participantArray[t];
                }
                if (d < (rounds - 1) && leftOver == 2)
                {
                    groups[d][count] = participantArray[t];
                    t++;
                }
            }

            for (var j = 1; j <= rounds; j++)
            {
                var start = 0;
                group = j - 1;
                for (var d = group; d < groups.Count; d++)
                {
                    for (var i = 0; i < groups[d].Count; i++)
                    {
                        start++;
                        var wl = new Wettkampf_Leitung()
                        {
                            Start = start,
                            Teilnehmer = groups[d][i],
                            Durchgang = j,
                            Status = 0,
                            Start_Time = TimeSpan.Zero
                        };
                        await wettkampfLeitungRepository.SaveAsync(wl, cancellationToken);
                    }
                }
                if (j > 1)
                {
                    group = 0;
                    for (var d = group; d < (j - 1); d++)
                    {
                        for (var i = 0; i < groups[d].Count; i++)
                        {
                            start++;
                            var wl = new Wettkampf_Leitung()
                            {
                                Start = start,
                                Teilnehmer = groups[d][i],
                                Durchgang = j,
                                Status = 0,
                                Start_Time = TimeSpan.Zero
                            };
                            await wettkampfLeitungRepository.SaveAsync(wl, cancellationToken);
                        }
                    }
                }
            }
        }
    }
}
