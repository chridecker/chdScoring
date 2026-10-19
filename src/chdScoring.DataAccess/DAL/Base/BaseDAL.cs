using chdScoring.DataAccess.Contracts.DAL.Base;
using chdScoring.DataAccess.Contracts.Repositories;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories.Base;

namespace chdScoring.DataAccess.DAL.Base
{
    public abstract class BaseDAL : IBaseDAL
    {
        protected readonly ILogger<BaseDAL> _logger;
        protected readonly IWettkampfLeitungRepository _wettkampfLeitungRepository;
        protected readonly ITeilnehmerRepository _teilnehmerRepository;
        protected readonly IJudgeRepository _judgeRepository;
        protected readonly IFigurRepository _figurRepository;
        protected readonly IProgrammRepository _programmRepository;
        protected readonly IWertungRepository _wertungRepository;
        protected readonly IKlasseRepository _klasseRepository;
        protected readonly IBaseEntityRepository<Country_Images> _countryImageRepository;
        protected readonly IDurchgangRepository _durchgangRepository;
        protected readonly IBaseEntityRepository<Images> _imageRepository;
        protected readonly IBaseEntityRepository<Durchgang_Panel> _durchgangPanelRepository;
        protected readonly IBaseEntityRepository<Durchgang_Programm> _durchgangProgramRepository;
        protected readonly IBaseEntityRepository<Judge_Panel> _judgePanelRepository;
        protected readonly IBaseEntityRepository<Stammdaten> _stammDatenRepository;
        protected readonly IBaseEntityRepository<Bewerb> _bebwerbRepository;
        protected readonly IBaseEntityRepository<Teilnehmer_Bewerb> _teilnehmerBewerbRepository;

        public BaseDAL(ILogger<BaseDAL> logger,
            IWettkampfLeitungRepository wettkampfLeitungRepository,
            ITeilnehmerRepository teilnehmerRepository,
            IJudgeRepository judgeRepository,
            IFigurRepository figurRepository,
            IProgrammRepository programmRepository,
            IWertungRepository wertungRepository,
            IKlasseRepository klasseRepository,
            IBaseEntityRepository<Country_Images> countryImageRepository,
            IBaseEntityRepository<Images> imageRepository,
            IBaseEntityRepository<Durchgang_Panel> durchgangPanelRepository,
            IBaseEntityRepository<Durchgang_Programm> durchgangProgramRepository,
            IBaseEntityRepository<Judge_Panel> judgePanelRepository,
            IBaseEntityRepository<Stammdaten> stammDatenRepository,
            IBaseEntityRepository<Bewerb> bebwerbRepository,
            IDurchgangRepository durchgangRepository,
            IBaseEntityRepository<Teilnehmer_Bewerb> teilnehmerBewerbRepository
            )
        {
            this._logger = logger;
            this._wettkampfLeitungRepository = wettkampfLeitungRepository;
            this._teilnehmerRepository = teilnehmerRepository;
            this._judgeRepository = judgeRepository;
            this._figurRepository = figurRepository;
            this._programmRepository = programmRepository;
            this._wertungRepository = wertungRepository;
            this._klasseRepository = klasseRepository;
            this._countryImageRepository = countryImageRepository;
            this._imageRepository = imageRepository;
            this._durchgangPanelRepository = durchgangPanelRepository;
            this._durchgangProgramRepository = durchgangProgramRepository;
            this._judgePanelRepository = judgePanelRepository;
            this._stammDatenRepository = stammDatenRepository;
            this._bebwerbRepository = bebwerbRepository;
            this._durchgangRepository = durchgangRepository;
            this._teilnehmerBewerbRepository = teilnehmerBewerbRepository;
        }
    }
}
