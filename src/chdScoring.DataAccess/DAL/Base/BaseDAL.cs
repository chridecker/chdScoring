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
        private DbTransaction _currentTransaction;

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

        public Task Commit(CancellationToken cancellationToken)
        => this._currentTransaction.CommitAsync(cancellationToken);


        public async Task CreateTransaction(CancellationToken cancellationToken)
        {
            if (this._currentTransaction == null)
            {
                this._currentTransaction = await this._bebwerbRepository.CreateTransaction(cancellationToken);
                await this.SetTransaction(this._currentTransaction,cancellationToken);
            }
        }

        public Task<DbTransaction> GetTransaction()
        => Task.FromResult(this._currentTransaction);

        public Task RollBack(CancellationToken cancellationToken)
        => this._currentTransaction.RollbackAsync(cancellationToken);

        public async Task SetTransaction(DbTransaction dbTransaction, CancellationToken cancellationToken)
        {
            if (this._currentTransaction == null)
            {
                this._currentTransaction = dbTransaction;
            }
            await this.SetRepositoryTransaction(dbTransaction, cancellationToken);
        }
        protected virtual async Task SetRepositoryTransaction(DbTransaction dbTransaction, CancellationToken cancellationToken)
        {
            await this._wettkampfLeitungRepository.SetTransaction(dbTransaction, cancellationToken);
            await this._teilnehmerRepository.SetTransaction(dbTransaction, cancellationToken);
            await this._judgeRepository.SetTransaction(dbTransaction, cancellationToken);
            await this._figurRepository.SetTransaction(dbTransaction, cancellationToken);
            await this._programmRepository.SetTransaction(dbTransaction, cancellationToken);
            await this._wertungRepository.SetTransaction(dbTransaction, cancellationToken);
            await this._klasseRepository.SetTransaction(dbTransaction, cancellationToken);
            await this._countryImageRepository.SetTransaction(dbTransaction, cancellationToken);
            await this._imageRepository.SetTransaction(dbTransaction, cancellationToken);
            await this._durchgangPanelRepository.SetTransaction(dbTransaction, cancellationToken);
            await this._durchgangProgramRepository.SetTransaction(dbTransaction, cancellationToken);
            await this._figurProgrammRepository.SetTransaction(dbTransaction, cancellationToken);
            await this._judgePanelRepository.SetTransaction(dbTransaction, cancellationToken);
            await this._stammDatenRepository.SetTransaction(dbTransaction, cancellationToken);
            await this._bebwerbRepository.SetTransaction(dbTransaction, cancellationToken);
            await this._durchgangRepository.SetTransaction(dbTransaction, cancellationToken);
            await this._teilnehmerBewerbRepository.SetTransaction(dbTransaction, cancellationToken);
        }
    }
}
