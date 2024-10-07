using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Services
{
    public class JudgeService : IJudgeService
    {
        private readonly IJudgeRepository _judgeRepository;
        private readonly IFlightCacheService _flightCacheService;

        public JudgeService(IJudgeRepository judgeRepository, IFlightCacheService flightCacheService)
        {
            this._judgeRepository = judgeRepository;
            this._flightCacheService = flightCacheService;
        }
        public Task<CurrentFlight> GetCurrentFlight(CancellationToken cancellationToken) => Task.FromResult(this._flightCacheService.GetCurrentFlight(DateTime.Now));

        public async Task<IEnumerable<JudgeDto>> GetJudges(CancellationToken cancellationToken = default)
        {
            var judges = await _judgeRepository.FindAll(cancellationToken);
            return judges.Select(s => new JudgeDto { Id = s.Id, Name = $"{s.Vorname} {s.Name}", Password = s.Pin.ToString("D4") });
        }
    }
}
