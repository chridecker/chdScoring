
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
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.DAL
{
    public class PilotDAL : BaseDAL, IPilotDAL
    {
        private readonly ITeilnehmerDurchgangJudgeRespository _teilnehmerDurchgangJudgeRespository;

        public PilotDAL(ILogger<PilotDAL> logger, ITeilnehmerDurchgangJudgeRespository teilnehmerDurchgangJudgeRespository, IWettkampfLeitungRepository wettkampfLeitungRepository, ITeilnehmerRepository teilnehmerRepository, IJudgeRepository judgeRepository, IFigurRepository figurRepository, IProgrammRepository programmRepository, IWertungRepository wertungRepository, IKlasseRepository klasseRepository, ICountryImageRepository countryImageRepository, IImageRepository imageRepository, IDurchgangPanelRepository durchgangPanelRepository, IDurchgangProgramRepository durchgangProgramRepository, IFigurProgrammRepository figurProgrammRepository, IJudgePanelRepository judgePanelRepository, IStammDatenRepository stammDatenRepository, IBebwerbRepository bebwerbRepository, IDurchgangRepository durchgangRepository, ITeilnehmerBewerbRepository teilnehmerBewerbRepository) : base(logger, wettkampfLeitungRepository, teilnehmerRepository, judgeRepository, figurRepository, programmRepository, wertungRepository, klasseRepository, countryImageRepository, imageRepository, durchgangPanelRepository, durchgangProgramRepository, figurProgrammRepository, judgePanelRepository, stammDatenRepository, bebwerbRepository, durchgangRepository, teilnehmerBewerbRepository)
        {
            this._teilnehmerDurchgangJudgeRespository = teilnehmerDurchgangJudgeRespository;
        }

        public async Task<bool> DeleteRoundScoring(int pilot, int round, CancellationToken cancellationToken)
        {
            var confirms = await this._teilnehmerDurchgangJudgeRespository.Where(x => x.Teilnehmer == pilot && x.Durchgang == round).ToListAsync();
            var scores = await this._wertungRepository.Where(x => x.Teilnehmer == pilot && x.Durchgang == round).Include(i => i.Histories).ToListAsync();
            var durchgang = await this._durchgangRepository.FirstOrDefaultAsync(x => x.Teilnehmer == pilot && x.Durchgang == round);
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
                Pilot = new PilotDto
                {
                    Id = wl.Teilnehmer,
                    Firstname = wl.Pilot.Vorname,
                    Lastname = wl.Pilot.Nachname,
                    Club = wl.Pilot.Club,
                    License = wl.Pilot.License,
                    CountryId = wl.Pilot.Land,
                    Country = wl.Pilot.Country_Image.Name,
                    CountryCode = wl.Pilot.Country_Image.Short,
                    CountryImage = new ImageDto
                    {
                        Data = wl.Pilot.Country_Image.Img_Data,
                        Type = wl.Pilot.Country_Image.Img_Type
                    }
                }
            });
        }

        public async Task<IEnumerable<FinishedRoundDto>> GetFinishedFlights(CancellationToken cancellationToken)
        {
            var rounds = await this._wettkampfLeitungRepository.Where(x => x.Status >= (int)EFlightState.Saved)
                .Include(x => x.Pilot).ThenInclude(i => i.Country_Image)
                .AsSplitQuery().ToListAsync();
            return rounds.Select(s => new FinishedRoundDto
            {
                Start = s.Start,
                Pilot = new()
                {
                    Id = s.Pilot.Id,
                    Club = s.Pilot.Club,
                    Firstname = s.Pilot.Vorname,
                    Lastname = s.Pilot.Nachname,
                    License = s.Pilot.License,
                    Country = s.Pilot.Country_Image.Name,
                    CountryCode = s.Pilot.Country_Image.Short,
                    CountryImage = new ImageDto()
                    {
                        Data = s.Pilot.Country_Image.Img_Data,
                        Type = s.Pilot.Country_Image.Img_Type
                    }
                },
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
                round = (await this._wettkampfLeitungRepository.Where(x => x.Status == (int)EFlightState.Loaded).OrderBy(o => o.Durchgang).FirstOrDefaultAsync())?.Durchgang ?? 0;
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
                    Pilot = new PilotDto
                    {
                        Id = wl.Teilnehmer,
                        Firstname = wl.Pilot.Vorname,
                        Lastname = wl.Pilot.Nachname,
                        License = wl.Pilot.License,
                        Club = wl.Pilot.Club,
                        Country = wl.Pilot.Country_Image.Name,
                        CountryCode = wl.Pilot.Country_Image.Short,
                        CountryImage = new ImageDto
                        {
                            Data = wl.Pilot.Country_Image.Img_Data,
                            Type = wl.Pilot.Country_Image.Img_Type
                        }
                    }
                };
                retValue.Add(dto);
            }
            return retValue.OrderBy(o => o.Rank);
        }

        public async Task<bool> SetPilotActive(LoadPilotDto dto, CancellationToken cancellationToken)
        {
            var active = await this._wettkampfLeitungRepository.FirstOrDefaultAsync(x => x.Status == (int)EFlightState.OnAir);
            if (active is not null)
            {
                active.Status = (int)EFlightState.Loaded;
                active.Start_Time = TimeSpan.Zero;
                await this._wettkampfLeitungRepository.SaveAsync(active, cancellationToken);
            }
            var wl = await this._wettkampfLeitungRepository.FirstOrDefaultAsync(x => x.Teilnehmer == dto.Pilot && x.Durchgang == dto.Round);
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
            return await this._countryImageRepository.FindById(id, cancellationToken);
        }
        public Task<IEnumerable<Country_Images>> GetAllCountryImages(CancellationToken cancellationToken) => this._countryImageRepository.FindAll(cancellationToken);

        public async Task<IEnumerable<PilotDto>> GetAllPilots(CancellationToken cancellationToken = default)
        {
            var lst = await this._teilnehmerRepository.Where(x => true).
                Include(i => i.Country_Image).ToListAsync(cancellationToken);

            return lst.Select(s => new PilotDto()
            {
                Id = s.Id,
                Firstname = s.Vorname,
                Lastname = s.Nachname,
                Club = s.Club,
                CountryId = s.Land,
                License = s.License,
                Country = s.Country_Image.Name,
                CountryCode = s.Country_Image.Short,
                CountryImage = new ImageDto
                {
                    Data = s.Country_Image.Img_Data,
                    Type = s.Country_Image.Img_Type
                }
            });
        }

        public async Task<bool> ChangeStartNumber(PilotDto pilot, int number, CancellationToken cancellationToken = default)
        {
            using var client = new HttpClient();
            var res = await client.GetAsync($"http://localhost/operations/change_startnumber.php?id={pilot.Id}&newid={number}");
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> UpdatePilotData(PilotDto dto, CancellationToken cancellationToken)
        {
            var pilot = await this._teilnehmerRepository.FindById(dto.Id, cancellationToken);
            if (pilot is not null)
            {
                pilot.Club = dto.Club;
                pilot.Vorname = dto.Firstname;
                pilot.Nachname = dto.Lastname;
                pilot.License = dto.License;
                return await this._teilnehmerRepository.SaveAsync(pilot, cancellationToken);
            }
            return false;
        }
    }
}