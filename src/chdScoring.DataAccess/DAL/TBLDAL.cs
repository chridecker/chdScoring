using chdScoring.DataAccess.Contracts.DAL;
using chdScoring.DataAccess.Contracts.DAL.Base;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.DAL.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using chdScoring.Contracts.Extensions;
using System.Text.Json.Serialization;
using System.IO;

namespace chdScoring.DataAccess.DAL
{
    public class TBLDAL : BaseDAL, ITBLDAL
    {
        public TBLDAL(ILogger<TBLDAL> logger, IWettkampfLeitungRepository wettkampfLeitungRepository, ITeilnehmerRepository teilnehmerRepository, IJudgeRepository judgeRepository, IFigurRepository figurRepository, IProgrammRepository programmRepository, IWertungRepository wertungRepository, IKlasseRepository klasseRepository, ICountryImageRepository countryImageRepository, IImageRepository imageRepository, IDurchgangPanelRepository durchgangPanelRepository, IDurchgangProgramRepository durchgangProgramRepository, IFigurProgrammRepository figurProgrammRepository, IJudgePanelRepository judgePanelRepository, IStammDatenRepository stammDatenRepository, IBebwerbRepository bebwerbRepository, IDurchgangRepository durchgangRepository, ITeilnehmerBewerbRepository teilnehmerBewerbRepository) : base(logger, wettkampfLeitungRepository, teilnehmerRepository, judgeRepository, figurRepository, programmRepository, wertungRepository, klasseRepository, countryImageRepository, imageRepository, durchgangPanelRepository, durchgangProgramRepository, figurProgrammRepository, judgePanelRepository, stammDatenRepository, bebwerbRepository, durchgangRepository, teilnehmerBewerbRepository)
        {
        }
        public async Task Calculate(int round, CancellationToken stoppingToken)
        {
            try
            {
                var stammdaten = await this._stammDatenRepository.FindAll(stoppingToken);
                var teilnehmerLst = await this._teilnehmerRepository.FindAll(stoppingToken);

                var judgesDG = await this._judgeRepository.GetRoundPanel(round, stoppingToken);
                var figuren = await this._figurRepository.GetProgramToRound(round, stoppingToken);
                var wertungen = await this._wertungRepository.FindByRound(round, stoppingToken);

                var figurMapDict = new Dictionary<int, decimal>();
                for (int i = 1; i <= figuren.Count(); i++)
                {
                    figurMapDict[i] = figuren.ElementAt(i - 1).Wert;
                }

                var teilnehmerRes = new Dictionary<int, decimal>();
                var teilnehmerJRes = new Dictionary<int, Dictionary<int, decimal>>();
                var judgeAvg = new Dictionary<int, Dictionary<int, decimal>>();

                var z = 1.645m;

                foreach (var teilnehmer in teilnehmerLst)
                {
                    if (!teilnehmerJRes.ContainsKey(teilnehmer.Id))
                    {
                        teilnehmerJRes[teilnehmer.Id] = new Dictionary<int, decimal>();
                    }

                    foreach (var judge in judgesDG)
                    {
                        if (!judgeAvg.ContainsKey(judge.Id))
                        {
                            judgeAvg[judge.Id] = new Dictionary<int, decimal>();
                        }
                        teilnehmerJRes[teilnehmer.Id][judge.Id] = 0;
                        judgeAvg[judge.Id][teilnehmer.Id] = 0;
                        var jWertungen = wertungen.Where(x => x.Teilnehmer == teilnehmer.Id && x.Judge == judge.Id);
                        foreach (var jWertung in jWertungen.OrderBy(o => o.Figur))
                        {
                            teilnehmerJRes[teilnehmer.Id][judge.Id] += jWertung.Wert * figurMapDict[jWertung.Figur];
                            judgeAvg[judge.Id][teilnehmer.Id] += jWertung.Wert * figurMapDict[jWertung.Figur];
                        }
                    }
                }
              
                var avgAll = judgeAvg.SelectMany(s => s.Value.Values).Average();
                var stdvAll = judgeAvg.SelectMany(s => s.Value.Values).StandardDeviation();
                foreach (var judge in judgesDG)
                {
                    var jAvg = judgeAvg[judge.Id].Select(s => s.Value).Average();
                    var jStdv = judgeAvg[judge.Id].Select(s => s.Value).StandardDeviation();
                    foreach (var teilnehmer in teilnehmerLst)
                    {
                        var val = teilnehmerJRes[teilnehmer.Id][judge.Id];
                        val = val + avgAll - jAvg;
                        val = val - (val - avgAll - (val - avgAll) * (stdvAll / jStdv));
                        teilnehmerJRes[teilnehmer.Id][judge.Id] = val;
                        judgeAvg[judge.Id][teilnehmer.Id] = val;
                    }
                }

                foreach (var teilnehmer in teilnehmerLst)
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var avg = teilnehmerJRes[teilnehmer.Id].Select(s => s.Value).Average();
                        var stdv = teilnehmerJRes[teilnehmer.Id].Select(s => s.Value).StandardDeviation();
                        var min = avg - stdv * z;
                        var max = avg + stdv * z;

                        var discardLst = teilnehmerJRes[teilnehmer.Id].Where(x => x.Value < min || x.Value > max).Select(s => s.Key);
                        teilnehmerJRes[teilnehmer.Id] = teilnehmerJRes[teilnehmer.Id].Where(x => x.Value >= min && x.Value <= max).ToDictionary(x => x.Key, x => x.Value);
                        if (!discardLst.Any())
                        {
                            break;
                        }
                    }
                    teilnehmerRes[teilnehmer.Id] = teilnehmerJRes[teilnehmer.Id].Select(s => s.Value).Average();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
