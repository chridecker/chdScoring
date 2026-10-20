using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Enums;
using chdScoring.DataAccess.Contracts.DAL;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.DAL.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using chdScoring.Contracts.Constants;
using chdScoring.DataAccess.Contracts.Repositories.Base;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.DataAccess.DAL
{
    public class CurrentFlightDAL : BaseDAL, ICurrentFlightDAL
    {
        private readonly IBaseEntityRepository<Teilnehmer_Durchgang_Judge> _teilnehmerDurchgangJudgeRespository;

        public CurrentFlightDAL(ILogger<CurrentFlightDAL> logger,
            IWettkampfLeitungRepository wettkampfLeitungRepository, ITeilnehmerRepository teilnehmerRepository, IJudgeRepository judgeRepository,
            IFigurRepository figurRepository, IProgrammRepository programmRepository, IWertungRepository wertungRepository, IKlasseRepository klasseRepository,
            IBaseEntityRepository<Country_Images> countryImageRepository, IBaseEntityRepository<Images> imageRepository, IBaseEntityRepository<Durchgang_Panel> durchgangPanelRepository,
            IBaseEntityRepository<Durchgang_Programm> durchgangProgramRepository, IBaseEntityRepository<Judge_Panel> judgePanelRepository,
            IBaseEntityRepository<Contracts.Domain.Stammdaten> stammDatenRepository, IBaseEntityRepository<Bewerb> bebwerbRepository, IDurchgangRepository durchgangRepository,
            IBaseEntityRepository<Teilnehmer_Bewerb> teilnehmerBewerbRepository,
            IBaseEntityRepository<Teilnehmer_Durchgang_Judge> teilnehmerDurchgangJudgeRespository) : base(logger, wettkampfLeitungRepository, teilnehmerRepository, judgeRepository, figurRepository, programmRepository, wertungRepository, klasseRepository, countryImageRepository, imageRepository, durchgangPanelRepository, durchgangProgramRepository, judgePanelRepository, stammDatenRepository, bebwerbRepository, durchgangRepository, teilnehmerBewerbRepository)
        {
            this._teilnehmerDurchgangJudgeRespository = teilnehmerDurchgangJudgeRespository;
        }

        public async Task<CurrentFlight> GetCurrentFlightData(CancellationToken cancellationToken)
        {
            CurrentFlight dto = null;
            try
            {
                var lst = await this._wettkampfLeitungRepository.CurrentRoundSet(cancellationToken);
                var currentPilot = lst.OrderBy(o => o.Start).FirstOrDefault(x => x.Status == (int)EFlightState.OnAir);

                if (currentPilot != null)
                {
                    var stammdaten = await this._stammDatenRepository.FindAll(cancellationToken);
                    var klasse = await this._klasseRepository.GetCurrentKlasse(cancellationToken);

                    var currentTime = DateTime.Now.TimeOfDay;
                    TimeSpan? time = currentPilot.Start_Time == TimeSpan.Zero || currentTime < currentPilot.Start_Time ? null : TimeSpan.FromMinutes(klasse.Zeit) - (currentTime - currentPilot.Start_Time);

                    dto = new CurrentFlight()
                    {
                        ScoreMode = (EScoreMode)(stammdaten.FirstOrDefault()?.Score_Mode ?? 0),
                        EditScoreEnabled = stammdaten.FirstOrDefault()?.Edit ?? false,
                        JudeConfirmation = stammdaten.FirstOrDefault()?.Judge_Pin ?? false,
                        StartTime = currentPilot.Start_Time,
                        LeftTime = time.HasValue && time.Value < TimeSpan.Zero ? TimeSpan.Zero : time,
                    };
                    dto = await this.GetRoundData(dto, currentPilot, cancellationToken);
                    dto.Round.Time = TimeSpan.FromMinutes(klasse.Zeit);
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, ex.Message);
            }
            return dto;
        }

        public async Task<RoundDataDto> GetRoundData(int pilot, int round, CancellationToken cancellationToken)
        {
            var dto = new RoundDataDto();
            var wl = await this._wettkampfLeitungRepository
                .Where(x => x.Status >= (int)EFlightState.OnAir)
                .Include(i => i.Pilot)
                .ThenInclude(i => i.Country_Image)
                .FirstOrDefaultAsync(x => x.Teilnehmer == pilot && x.Durchgang == round, cancellationToken);

            return await this.GetRoundData(dto, wl, cancellationToken);
        }

        private async Task<T> GetRoundData<T>(T dto, Wettkampf_Leitung wl, CancellationToken cancellationToken)
            where T : RoundDataDto
        {
            var round = wl.Durchgang;
            var judges = await this._judgeRepository.GetRoundPanel(round, cancellationToken);
            var program = await this._programmRepository.GetProgramToRound(round, cancellationToken);
            var maneouvreLst = (await this._figurRepository.GetProgramToRound(round, cancellationToken)).OrderBy(o => o.Id);
            var scores = await this._wertungRepository.GetScoresToPilotInRound(wl.Teilnehmer, round, cancellationToken);
            var confirms = await this._teilnehmerDurchgangJudgeRespository.Where(x => x.Teilnehmer == wl.Teilnehmer && x.Durchgang == wl.Durchgang).ToListAsync(cancellationToken);
            var roundResult =
                await _durchgangRepository.FirstOrDefaultAsync(x => x.Durchgang == round && x.Teilnehmer == wl.Pilot.Id, cancellationToken);

            dto.Pilot = new PilotDto
            {
                Id = wl.Pilot.Id,
                Firstname = wl.Pilot.Vorname,
                Lastname = wl.Pilot.Nachname,
                CountryId = wl.Pilot.Land,
                CountryImage = new ImageDto()
                {
                    Data = wl.Pilot.Country_Image?.Img_Data,
                    Type = wl.Pilot.Country_Image?.Img_Type,
                },
                Club = wl.Pilot.Club,
                Country = wl.Pilot.Country_Image.Name,
                CountryCode = wl.Pilot.Country_Image.Short.ToUpper(),
                License = wl.Pilot.License
            };
            dto.JudgeConfirms = confirms.Select(s => new JudgeConfirmDto() { Durchgang = s.Durchgang, Judge = s.Judge, Teilnehmer = s.Teilnehmer });
            dto.Judges = judges.Select(judge => new JudgeDto { Id = judge.Id, Name = $"{judge.Vorname} {judge.Name.ToUpper()}", EditScore = judge.EditScore });
            dto.Round = new RoundDto
            {
                Id = round,
                Program = program.Title,
                Time = roundResult is null ? TimeSpan.Zero : TimeSpan.FromSeconds(roundResult.Duration)
            };

            foreach (var judge in judges.OrderBy(o => o.Id))
            {
                var judgeScores = scores.Where(x => x.Judge == judge.Id);
                var figurs = new List<ManeouvreDto>();
                for (int i = 1; i <= maneouvreLst.Count(); i++)
                {
                    var element = maneouvreLst.ElementAt(i - 1);
                    figurs.Add(new ManeouvreDto
                    {
                        Id = i,
                        Name = element.Name,
                        Value = element.Wert,
                        Score = judgeScores.FirstOrDefault(x => x.Figur == i)?.Wert,
                        Saved = judgeScores.Any(x => x.Figur == i),
                        Histories = judgeScores.Any(x => x.Figur == i) ? judgeScores.FirstOrDefault(x => x.Figur == i).Histories.Select(s => new ManeouvreHistoryDto
                        {
                            OldScore = s.Wert_alt,
                            Score = s.Wert_neu,
                            Changed = s.Time
                        }).ToList() : []
                    });
                }
                dto.ManeouvreLst[judge.Id] = figurs;
            }
            return dto;
        }
    }
}
