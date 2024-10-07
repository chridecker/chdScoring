
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Enums;
using chdScoring.DataAccess.Contracts.DAL;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.DAL.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public async Task<IEnumerable<OpenRoundDto>> LoadOpenPilots(int round, CancellationToken cancellationToken)
        {
            var retValue = new List<OpenRoundDto>();
            var lst = await this._wettkampfLeitungRepository.Where(x => x.Durchgang == round && x.Status == (int)EFlightState.Loaded)
                .Include(i => i.Pilot).ThenInclude(i => i.Country_Image)
                .Include(i => i.Pilot).ThenInclude(i => i.Image)
                .ToListAsync();
            return lst.OrderBy(o => o.Start).Select(s => new OpenRoundDto
            {
                StartNumber = s.Start,
                Pilot = new PilotDto
                {
                    Id = s.Teilnehmer,
                    Name = $"{s.Pilot.Vorname} {s.Pilot.Nachname.ToUpper()}",
                    Club = s.Pilot.Club,
                    Country = s.Pilot.Country_Image.Name,
                    CountryCode = s.Pilot.Country_Image.Short,
                    CountryImage = s.Pilot.Country_Image.Img_Data
                }
            });
        }

    }
}