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

namespace chdScoring.DataAccess.DAL
{
    public class TBLDAL : BaseDAL, ITBLDAL
    {
        public TBLDAL(ILogger<TBLDAL> logger, IWettkampfLeitungRepository wettkampfLeitungRepository, ITeilnehmerRepository teilnehmerRepository, IJudgeRepository judgeRepository, IFigurRepository figurRepository, IProgrammRepository programmRepository, IWertungRepository wertungRepository, IKlasseRepository klasseRepository, ICountryImageRepository countryImageRepository, IImageRepository imageRepository, IDurchgangPanelRepository durchgangPanelRepository, IDurchgangProgramRepository durchgangProgramRepository, IFigurProgrammRepository figurProgrammRepository, IJudgePanelRepository judgePanelRepository, IStammDatenRepository stammDatenRepository, IBebwerbRepository bebwerbRepository, IDurchgangRepository durchgangRepository, ITeilnehmerBewerbRepository teilnehmerBewerbRepository) : base(logger, wettkampfLeitungRepository, teilnehmerRepository, judgeRepository, figurRepository, programmRepository, wertungRepository, klasseRepository, countryImageRepository, imageRepository, durchgangPanelRepository, durchgangProgramRepository, figurProgrammRepository, judgePanelRepository, stammDatenRepository, bebwerbRepository, durchgangRepository, teilnehmerBewerbRepository)
        {
        }
        public async Task Calculate(int round,CancellationToken stoppingToken)
        {
            try
            {
                var stammdaten = await this._stammDatenRepository.FindAll(stoppingToken);
                var judgesPanels = await this._judgePanelRepository.FindAll(stoppingToken);
                var teilnehmerLst = await this._teilnehmerRepository.FindAll(stoppingToken);

                var resDict = new Dictionary<int, decimal>();

                var allFiguren = await this._figurRepository.FindAll(stoppingToken);
                var allJudges = await this._judgeRepository.FindAll(stoppingToken);
                var wertungen = await this._wertungRepository.FindByRound(round, stoppingToken);
                var panel = (await this._durchgangPanelRepository.FindAll(stoppingToken)).FirstOrDefault(x => x.Durchgang == round);
                var judgesinPanel = judgesPanels.Where(x => x.Panel == panel.Panel);
                var judgesDG = allJudges.Where(x => judgesinPanel.Any(a => a.Judge == x.Id));

                var program = (await this._durchgangProgramRepository.FindAll(stoppingToken)).FirstOrDefault(x => x.Durchgang == round);
                var figurenInProgram = (await this._figurProgrammRepository.FindAll(stoppingToken)).Where(x => x.Programm == program.Programm);
                var figuren = allFiguren.Where(x => figurenInProgram.Any(a => a.Figur == x.Id));

                var figurMapDict = new Dictionary<int, decimal>();
                for (int i = 1; i <= allFiguren.Count(); i++)
                {
                    figurMapDict[i] = allFiguren.ElementAt(i - 1).Wert;
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
                        var w = wertungen.Where(x => x.Teilnehmer == teilnehmer.Id && x.Judge == judge.Id);
                        teilnehmerJRes[teilnehmer.Id][judge.Id] = w.Select(s => figurMapDict[s.Figur] * s.Wert).Sum();
                        judgeAvg[judge.Id][teilnehmer.Id] = w.Select(s => figurMapDict[s.Figur] * s.Wert).Sum();
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
                    }
                }

                foreach (var teilnehmer in teilnehmerLst)
                {
                    var avg = teilnehmerJRes[teilnehmer.Id].Select(s => s.Value).Average();
                    var stdv = teilnehmerJRes[teilnehmer.Id].Select(s => s.Value).StandardDeviation() * z;
                    var min = avg - stdv;
                    var max = avg + stdv;

                    teilnehmerRes[teilnehmer.Id] = teilnehmerJRes[teilnehmer.Id].Select(s => s.Value).Where(x => x >= min && x <= max).Average();

                }


            }
            catch (Exception ex)
            {
            }
        }
    }
}
