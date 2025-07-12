using chdScoring.DataAccess.Contracts.DAL.Base;
using chdScoring.DataAccess.Contracts.Repositories;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

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
        protected readonly ICountryImageRepository _countryImageRepository;
        protected readonly IImageRepository _imageRepository;
        protected readonly IDurchgangPanelRepository _durchgangPanelRepository;
        protected readonly IDurchgangProgramRepository _durchgangProgramRepository;
        protected readonly IFigurProgrammRepository _figurProgrammRepository;
        protected readonly IJudgePanelRepository _judgePanelRepository;
        protected readonly IStammDatenRepository _stammDatenRepository;
        protected readonly IBebwerbRepository _bebwerbRepository;
        protected readonly IDurchgangRepository _durchgangRepository;
        protected readonly ITeilnehmerBewerbRepository _teilnehmerBewerbRepository;

        public BaseDAL(ILogger<BaseDAL> logger,
            IWettkampfLeitungRepository wettkampfLeitungRepository,
            ITeilnehmerRepository teilnehmerRepository,
            IJudgeRepository judgeRepository,
            IFigurRepository figurRepository,
            IProgrammRepository programmRepository,
            IWertungRepository wertungRepository,
            IKlasseRepository klasseRepository,
            ICountryImageRepository countryImageRepository,
            IImageRepository imageRepository,
            IDurchgangPanelRepository durchgangPanelRepository,
            IDurchgangProgramRepository durchgangProgramRepository,
            IFigurProgrammRepository figurProgrammRepository,
            IJudgePanelRepository judgePanelRepository,
            IStammDatenRepository stammDatenRepository,
            IBebwerbRepository bebwerbRepository,
            IDurchgangRepository durchgangRepository,
            ITeilnehmerBewerbRepository teilnehmerBewerbRepository
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
            this._figurProgrammRepository = figurProgrammRepository;
            this._judgePanelRepository = judgePanelRepository;
            this._stammDatenRepository = stammDatenRepository;
            this._bebwerbRepository = bebwerbRepository;
            this._durchgangRepository = durchgangRepository;
            this._teilnehmerBewerbRepository = teilnehmerBewerbRepository;
        }
    }
}
