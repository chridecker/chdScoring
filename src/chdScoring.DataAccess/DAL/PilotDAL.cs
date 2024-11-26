
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Enums;
using chdScoring.DataAccess.Contracts.DAL;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.DAL.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.DAL
{
    public class PilotDAL : BaseDAL, IPilotDAL
    {
        public PilotDAL(ILogger<PilotDAL> logger, IWettkampfLeitungRepository wettkampfLeitungRepository, ITeilnehmerRepository teilnehmerRepository, IJudgeRepository judgeRepository, IFigurRepository figurRepository, IProgrammRepository programmRepository, IWertungRepository wertungRepository, IKlasseRepository klasseRepository, ICountryImageRepository countryImageRepository, IImageRepository imageRepository, IDurchgangPanelRepository durchgangPanelRepository, IDurchgangProgramRepository durchgangProgramRepository, IFigurProgrammRepository figurProgrammRepository, IJudgePanelRepository judgePanelRepository, IStammDatenRepository stammDatenRepository, IBebwerbRepository bebwerbRepository, IDurchgangRepository durchgangRepository, ITeilnehmerBewerbRepository teilnehmerBewerbRepository) : base(logger, wettkampfLeitungRepository, teilnehmerRepository, judgeRepository, figurRepository, programmRepository, wertungRepository, klasseRepository, countryImageRepository, imageRepository, durchgangPanelRepository, durchgangProgramRepository, figurProgrammRepository, judgePanelRepository, stammDatenRepository, bebwerbRepository, durchgangRepository, teilnehmerBewerbRepository)
        {
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
                    Name = wl.Pilot.FullName,
                    Club = wl.Pilot.Club,
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
                    Name = s.Pilot.FullName,
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
            var lst = await this._wettkampfLeitungRepository.Where(x => x.Durchgang == round.Value && x.Status == (int)EFlightState.Saved)
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
                        Name = wl.Pilot.FullName,
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
    }
}