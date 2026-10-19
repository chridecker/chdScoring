
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Enums;
using chdScoring.DataAccess.Contracts.DAL;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.Contracts.Repositories.Base;
using chdScoring.DataAccess.DAL.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.DAL
{
    public class PilotDAL : BaseDAL, IPilotDAL
    {
        private readonly IBaseEntityRepository<Teilnehmer_Durchgang_Judge> _teilnehmerDurchgangJudgeRespository;

        public PilotDAL(ILogger<PilotDAL> logger,
            IWettkampfLeitungRepository wettkampfLeitungRepository, ITeilnehmerRepository teilnehmerRepository, IJudgeRepository judgeRepository,
            IFigurRepository figurRepository, IProgrammRepository programmRepository, IWertungRepository wertungRepository, IKlasseRepository klasseRepository,
            IBaseEntityRepository<Country_Images> countryImageRepository, IBaseEntityRepository<Images> imageRepository, IBaseEntityRepository<Durchgang_Panel> durchgangPanelRepository,
            IBaseEntityRepository<Durchgang_Programm> durchgangProgramRepository,  IBaseEntityRepository<Judge_Panel> judgePanelRepository,
            IBaseEntityRepository<Contracts.Domain.Stammdaten> stammDatenRepository, IBaseEntityRepository<Bewerb> bebwerbRepository, IDurchgangRepository durchgangRepository,
            IBaseEntityRepository<Teilnehmer_Bewerb> teilnehmerBewerbRepository,
            IBaseEntityRepository<Teilnehmer_Durchgang_Judge> teilnehmerDurchgangJudgeRespository) 
            : base(logger, wettkampfLeitungRepository, teilnehmerRepository, judgeRepository, figurRepository, programmRepository, wertungRepository, klasseRepository, countryImageRepository, imageRepository, durchgangPanelRepository, durchgangProgramRepository, judgePanelRepository, stammDatenRepository, bebwerbRepository, durchgangRepository, teilnehmerBewerbRepository)
        {
            this._teilnehmerDurchgangJudgeRespository = teilnehmerDurchgangJudgeRespository;
        }

        public async Task<bool> AddNewPilot(PilotDto dto, CancellationToken cancellationToken)
        {
            var entity = new Teilnehmer()
            {
                Id = dto.Id,
                Bild = 1,
                Club = dto.Club,
                Land = dto.CountryId,
                License = dto.License,
                Vorname = dto.Firstname,
                Nachname = dto.Lastname,
                Email = string.Empty,
                Info = string.Empty,
                Ort = string.Empty,
                Plz = string.Empty,
                Strasse = string.Empty,
                Telefon = string.Empty,
            };
            _ = await this._teilnehmerRepository.SaveAsync(entity, cancellationToken);
            var e = new Teilnehmer_Bewerb
            {
                Bewerb = 1,
                Teilnehmer = dto.Id
            };
            return await this._teilnehmerBewerbRepository.SaveAsync(e, cancellationToken);
        }

        public async Task<bool> ChangeStartingOrder(int newStart, int pilot, int round,
            CancellationToken cancellationToken)
        {
            var wl = await _wettkampfLeitungRepository.FirstOrDefaultAsync(x => x.Teilnehmer == pilot && x.Durchgang == round,
                cancellationToken);
            if (wl is not null && wl.Start != newStart)
            {
                wl.Start = newStart;
                await _wettkampfLeitungRepository.SaveAsync(wl, cancellationToken);
            }

            return true;
        }
        public async Task<bool> DeletePilot(PilotDto dto, CancellationToken cancellationToken)
        {
            var entity = await this._teilnehmerRepository.FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);
            if (entity is null) { return false; }
            return await this._teilnehmerRepository.Delete(entity);
        }

        public async Task<bool> DeleteRoundScoring(int pilot, int round, CancellationToken cancellationToken)
        {
            var confirms = await this._teilnehmerDurchgangJudgeRespository.Where(x => x.Teilnehmer == pilot && x.Durchgang == round).ToListAsync(cancellationToken);
            var scores = await this._wertungRepository.Where(x => x.Teilnehmer == pilot && x.Durchgang == round).Include(i => i.Histories).ToListAsync(cancellationToken);
            var durchgang = await this._durchgangRepository.FirstOrDefaultAsync(x => x.Teilnehmer == pilot && x.Durchgang == round, cancellationToken);
            foreach (var c in confirms)
            {
                await this._teilnehmerDurchgangJudgeRespository.Delete(c);
            }
            foreach (var score in scores)
            {
                await this._wertungRepository.Delete(score);
            }
            if (durchgang is not null)
            {
                await this._durchgangRepository.Delete(durchgang);
            }
            return await this.SetPilotActive(new LoadPilotDto()
            {
                Pilot = pilot,
                Round = round
            }, cancellationToken);
        }

        public async Task<List<PilotRoundDto>> GetStartingOrderPerRound(int? round, CancellationToken cancellationToken)
        {
            if (!round.HasValue)
            {
                round = (await this._wettkampfLeitungRepository.Where(x => x.Status == (int)EFlightState.Loaded).OrderBy(o => o.Durchgang).FirstOrDefaultAsync())?.Durchgang ?? 0;
            }

            var wl = await this._wettkampfLeitungRepository.Where(x => x.Durchgang == round).OrderBy(o => o.Start)
                .Include(i => i.Pilot)
                .ThenInclude(i => i.Country_Image)
                .ToListAsync(cancellationToken);
            var lst = new List<PilotRoundDto>();

            foreach (var wettkampfLeitung in wl)
            {
                lst.Add(new()
                {
                    Round = round.Value,
                    StartNr = wettkampfLeitung.Start,
                    State = (EFlightState)wettkampfLeitung.Status,
                    Pilot = MapPilot(wettkampfLeitung.Pilot)
                });
            }

            return lst;
        }

        public async Task<IEnumerable<OpenRoundDto>> LoadOpenPilots(int? round, CancellationToken cancellationToken)
        {
            if (!round.HasValue)
            {
                round = (await this._wettkampfLeitungRepository.Where(x => x.Status == (int)EFlightState.Loaded).OrderBy(o => o.Durchgang).FirstOrDefaultAsync())?.Durchgang ?? 0;
            }

            var lst = await this._wettkampfLeitungRepository.Where(x => x.Durchgang == round.Value && x.Status == (int)EFlightState.Loaded)
                .Include(i => i.Pilot).ThenInclude(i => i.Country_Image)
                .AsSplitQuery()
                .ToListAsync();
            return lst.Select(wl => new OpenRoundDto
            {
                StartNumber = wl.Start,
                Round = round.Value,
                Pilot = MapPilot(wl.Pilot)
            });
        }

        public async Task<IEnumerable<FinishedRoundDto>> GetFinishedFlights(CancellationToken cancellationToken)
        {
            var rounds = await this._wettkampfLeitungRepository.Where(x => x.Status >= (int)EFlightState.Saved)
                .Include(x => x.Pilot).ThenInclude(i => i.Country_Image)
                .AsSplitQuery().ToListAsync(cancellationToken);
            return rounds.Select(s => new FinishedRoundDto
            {
                Start = s.Start,
                Pilot = MapPilot(s.Pilot),
                Round = new()
                {
                    Id = s.Durchgang,
                }
            });
        }


        public async Task<IEnumerable<RoundResultDto>> LoadRoundResults(int? round, CancellationToken cancellationToken)
        {
            if (!round.HasValue)
            {
                round = (await this._wettkampfLeitungRepository.Where(x => x.Status == (int)EFlightState.Loaded).OrderBy(o => o.Durchgang).FirstOrDefaultAsync())?.Durchgang
                    ?? (await this._wettkampfLeitungRepository.FirstOrDefaultAsync(x => x.Status == (int)EFlightState.OnAir))?.Durchgang
                    ?? 0;
            }

            var retValue = new List<RoundResultDto>();
            var lst = await this._wettkampfLeitungRepository.Where(x => x.Durchgang == round.Value && x.Status >= (int)EFlightState.Saved)
                .Include(i => i.Pilot).ThenInclude(i => i.Country_Image)
                .Include(i => i.Round)
                .AsSplitQuery()
                .ToListAsync();

            var rank = 1;
            foreach (var wl in lst.OrderByDescending(o => o.Round.Wert_abs))
            {
                var dto = new RoundResultDto
                {
                    StartNumber = wl.Start,
                    Round = round.Value,
                    Score = wl.Round.Wert_abs,
                    ScoreProm = (decimal)wl.Round.Wert_prom,
                    Rank = rank++,
                    Pilot = MapPilot(wl.Pilot)
                };
                retValue.Add(dto);
            }
            return retValue.OrderBy(o => o.Rank);
        }

        public async Task<bool> SetPilotActive(LoadPilotDto dto, CancellationToken cancellationToken)
        {
            var active = await this._wettkampfLeitungRepository.FirstOrDefaultAsync(x => x.Status == (int)EFlightState.OnAir, cancellationToken);
            if (active is not null)
            {
                active.Status = (int)EFlightState.Loaded;
                active.Start_Time = TimeSpan.Zero;
                await this._wettkampfLeitungRepository.SaveAsync(active, cancellationToken);
            }
            var wl = await this._wettkampfLeitungRepository.FirstOrDefaultAsync(x => x.Teilnehmer == dto.Pilot && x.Durchgang == dto.Round, cancellationToken);
            if (wl != null)
            {
                wl.Status = (int)EFlightState.OnAir;
                return await this._wettkampfLeitungRepository.SaveAsync(wl, cancellationToken);
            }
            return false;
        }

        public async Task<bool> UnLoadPilot(LoadPilotDto dto, CancellationToken cancellationToken)
        {
            var wl = await this._wettkampfLeitungRepository.FirstOrDefaultAsync(x => x.Teilnehmer == dto.Pilot && x.Durchgang == dto.Round);
            if (wl != null)
            {
                wl.Status = (int)EFlightState.Loaded;
                wl.Start_Time = TimeSpan.Zero;
                return await this._wettkampfLeitungRepository.SaveAsync(wl, cancellationToken);
            }
            return false;
        }

        public async Task<Country_Images> GetCountryImage(int id, CancellationToken cancellationToken)
        {
            return await this._countryImageRepository.FirstOrDefaultAsync(x => x.Img_Id == id, cancellationToken);
        }
        public Task<IEnumerable<Country_Images>> GetAllCountryImages(CancellationToken cancellationToken) => this._countryImageRepository.FindAll(cancellationToken);

        public async Task<IEnumerable<PilotDto>> GetAllPilots(CancellationToken cancellationToken = default)
        {
            var lst = await this._teilnehmerRepository.Where(x => true).
                Include(i => i.Country_Image).ToListAsync(cancellationToken);

            return lst.Select(s => MapPilot(s));
        }

        public Task<bool> ChangeStartNumber(PilotDto pilot, int number, CancellationToken cancellationToken = default)
            => this._teilnehmerRepository.ChangeStartNumber(pilot.Id, number, cancellationToken);

        public async Task<bool> UpdatePilotData(PilotDto dto, CancellationToken cancellationToken)
        {
            var pilot = await this._teilnehmerRepository.FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);
            if (pilot is not null)
            {
                pilot.Club = dto.Club;
                pilot.Vorname = dto.Firstname;
                pilot.Nachname = dto.Lastname;
                pilot.License = dto.License;
                pilot.Land = dto.CountryId;
                return await this._teilnehmerRepository.SaveAsync(pilot, cancellationToken);
            }
            return false;
        }

        public async Task<PilotDto> Get(int id, CancellationToken cancellationToken)
        {
            var t = await _teilnehmerRepository
                .Where(x => x.Id == id)
                .Include(i => i.Country_Image)
                .FirstOrDefaultAsync(cancellationToken);
            return MapPilot(t);
        }

        private PilotDto MapPilot(Teilnehmer teilnehmer)
            => new()
            {
                Id = teilnehmer.Id,
                Firstname = teilnehmer.Vorname,
                Lastname = teilnehmer.Nachname,
                Club = teilnehmer.Club,
                CountryId = teilnehmer.Land,
                License = teilnehmer.License,
                Country = teilnehmer.Country_Image.Name,
                CountryCode = teilnehmer.Country_Image.Short,
                CountryImage = new ImageDto
                {
                    Data = teilnehmer.Country_Image.Img_Data,
                    Type = teilnehmer.Country_Image.Img_Type
                }
            };
    }
}