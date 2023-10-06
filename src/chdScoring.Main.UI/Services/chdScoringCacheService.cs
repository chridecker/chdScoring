using chdScoring.BusinessLogic.Extensions;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.Main.UI.Services
{
    public class chdScoringCacheService : BackgroundService
    {
        private readonly ITeilnehmerRepository _teilnehmerRepository;
        private readonly IStammDatenRepository _stammDatenRepository;
        private readonly IJudgeRepository _judgeRepository;
        private readonly IJudgePanelRepository _judgePanelRepository;
        private readonly IDurchgangPanelRepository _durchgangPanelRepository;
        private readonly IDurchgangProgramRepository _durchgangProgramRepository;
        private readonly IFigurProgrammRepository _figurProgrammRepository;
        private readonly IFigurRepository _figurRepository;
        private readonly IWertungRepository _wertungRepository;
        private readonly IProgrammRepository _programmRepository;

        public chdScoringCacheService(ITeilnehmerRepository teilnehmerRepository, IStammDatenRepository stammDatanRepository,
            IJudgeRepository judgeRepository, IJudgePanelRepository judgePanelRepository, IDurchgangPanelRepository durchgangPanelRepository,
            IDurchgangProgramRepository durchgangProgramRepository, IFigurProgrammRepository figurProgrammRepository, IFigurRepository figurRepository,
            IWertungRepository wertungRepository, IProgrammRepository programmRepository)
        {
            this._teilnehmerRepository = teilnehmerRepository;
            this._stammDatenRepository = stammDatanRepository;
            this._judgeRepository = judgeRepository;
            this._judgePanelRepository = judgePanelRepository;
            this._durchgangPanelRepository = durchgangPanelRepository;
            this._durchgangProgramRepository = durchgangProgramRepository;
            this._figurProgrammRepository = figurProgrammRepository;
            this._figurRepository = figurRepository;
            this._wertungRepository = wertungRepository;
            this._programmRepository = programmRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {

                var stammdaten = await this._stammDatenRepository.FindAll(stoppingToken);
                var judgesPanels = await this._judgePanelRepository.FindAll(stoppingToken);
                var teilnehmerLst = await this._teilnehmerRepository.FindAll(stoppingToken);
                var round = 1;

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

                var figurMapDict = new Dictionary<int,decimal>();
                var c = 1;
                foreach(var figure in allFiguren.OrderBy(o => o.Id))
                {
                    figurMapDict[c]=figure.Wert;
                    c++;
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
                        var avg = teilnehmerJRes[teilnehmer.Id][judge.Id];
                        var val = avgAll + ((avg + avgAll - jAvg) - avgAll) * stdvAll / jStdv;
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
