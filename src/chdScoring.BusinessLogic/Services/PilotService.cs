using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Services
{
    public class PilotService : IPilotService
    {
        private readonly IPilotDAL _dal;

        public PilotService(IPilotDAL dal)
        {
            this._dal = dal;
        }

        public Task<IEnumerable<OpenRoundDto>> GetOpenRound(int round, CancellationToken cancellationToken)
        => this._dal.LoadOpenPilots(round, cancellationToken);
    }
}
