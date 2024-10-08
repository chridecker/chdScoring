
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Enums;
using chdScoring.DataAccess.Contracts.DAL;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.DAL.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
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

            var retValue = new List<OpenRoundDto>();
            var lst = await this._wettkampfLeitungRepository.Where(x => x.Durchgang == round.Value && x.Status == (int)EFlightState.Loaded)
                .Include(i => i.Pilot)
                .AsSplitQuery()
                .ToListAsync();

            foreach (var wl in lst)
            {
                var country = await this._countryImageRepository.FirstOrDefaultAsync(x => x.Img_Id == wl.Pilot.Land);
                var dto = new OpenRoundDto
                {
                    StartNumber = wl.Start,
                    Round = round.Value,
                    Pilot = new PilotDto
                    {
                        Id = wl.Teilnehmer,
                        Name = $"{wl.Pilot.Vorname} {wl.Pilot.Nachname.ToUpper()}",
                        Club = wl.Pilot.Club,
                        Country = country.Name,
                        CountryCode = country.Short,
                        CountryImage = new ImageDto
                        {
                            Data = country.Img_Data,
                            Type = country.Img_Type
                        }
                    }
                };
                retValue.Add(dto);
            }
            return retValue.OrderBy(o => o.StartNumber);
        }

        public async Task<IEnumerable<RoundResultDto>> LoadRoundResults(int? round, CancellationToken cancellationToken)
        {
            if (!round.HasValue)
            {
                round = (await this._wettkampfLeitungRepository.Where(x => x.Status == (int)EFlightState.Loaded).OrderBy(o => o.Durchgang).FirstOrDefaultAsync())?.Durchgang ?? 0;
            }

            var retValue = new List<RoundResultDto>();
            var lst = await this._wettkampfLeitungRepository.Where(x => x.Durchgang == round.Value && x.Status == (int)EFlightState.Saved)
                .Include(i => i.Pilot)
                .Include(i => i.Round)
                .AsSplitQuery()
                .ToListAsync();

            var rank = 1;
            foreach (var wl in lst.OrderByDescending(o => o.Round.Wert_abs))
            {
                var country = await this._countryImageRepository.FirstOrDefaultAsync(x => x.Img_Id == wl.Pilot.Land);
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
                        Name = $"{wl.Pilot.Vorname} {wl.Pilot.Nachname.ToUpper()}",
                        Club = wl.Pilot.Club,
                        Country = country.Name,
                        CountryCode = country.Short,
                        CountryImage = new ImageDto
                        {
                            Data = country.Img_Data,
                            Type = country.Img_Type
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