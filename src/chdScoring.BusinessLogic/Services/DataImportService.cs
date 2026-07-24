using chdScoring.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using chdScoring.Contracts.Dtos;
using chdScoring.DataAccess.Contracts.DAL;

namespace chdScoring.BusinessLogic.Services
{
    public class DataImportService : IDataImportService
    {
        private readonly IScoreDAL _dal;
        private readonly ITimerDAL _timerDal;

        public DataImportService(IScoreDAL dal, ITimerDAL timerDal)
        {
            _dal = dal;
            _timerDal = timerDal;
        }
        public async Task ImportAsync(ImportRoundScoreDto dto, CancellationToken cancellationToken)
        {
            if (await this._dal.ImportFlight(dto, cancellationToken))
            {
                var kValDict = await this._timerDal.GetKValue(dto.Round, cancellationToken);

                await this._timerDal.SaveImportedRound(new()
                {
                    Pilot = dto.Pilot,
                    Round = dto.Round,
                    Score = dto.Scores.Sum(s => kValDict[s.Figure] * s.Value)
                }, cancellationToken);
            }
        }
    }
}
