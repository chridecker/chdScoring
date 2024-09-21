using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Enums;
using chdScoring.Contracts.Interfaces;
using Microsoft.Extensions.Logging;

namespace chdScoring.App.Services
{
    public class MainService : IMainService
    {
        private readonly ILogger<MainService> _logger;
        private readonly IJudgeService _judgeService;
        private readonly ITimerService _timerService;
        private readonly IScoringService _scoringService;
        private readonly ISettingManager _settingManager;

        public MainService(ILogger<MainService> logger, IJudgeService judgeService, ITimerService timerService, IScoringService scoringService, ISettingManager settingManager)
        {
            this._logger = logger;
            this._judgeService = judgeService;
            this._timerService = timerService;
            this._scoringService = scoringService;
            this._settingManager = settingManager;
        }
        public async Task<IEnumerable<JudgeDto>> GetJudges(CancellationToken cancellationToken)
        {
            try
            {
                return await this._judgeService.GetJudges(cancellationToken);
            }
            catch
            {

            }
            return Enumerable.Empty<JudgeDto>();
        }

        public Task<CurrentFlight> GetCurrentFlight(CancellationToken cancellationToken)
        => this._judgeService.GetCurrentFlight(cancellationToken);

        public async Task<bool> SaveScore(int id, int figur, int judge, int round, decimal value, CancellationToken token)
        {
            try
            {
                var dto = new SaveScoreDto
                {
                    Pilot = id,
                    Figur = figur,
                    Judge = judge,
                    Round = round,
                    Value = value
                };

                return await this._scoringService.SaveScore(dto, token);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> StartStop(ETimerOperation timerOperation, CancellationToken token)
        {
            try
            {
                var dto = new TimerOperationDto
                {
                    Operation = timerOperation
                };
                return await this._timerService.HandleOperation(dto, token);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
    public interface IMainService
    {
        Task<IEnumerable<JudgeDto>> GetJudges(CancellationToken cancellationToken);
        Task<CurrentFlight> GetCurrentFlight(CancellationToken cancellationToken);
        Task<bool> SaveScore(int id, int figur, int judge, int round, decimal value, CancellationToken token);
        Task<bool> StartStop(ETimerOperation timerOperation, CancellationToken token);
    }
}
