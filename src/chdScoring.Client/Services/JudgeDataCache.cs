using chdScoring.Client.Helper;
using chdScoring.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.Client.Services
{
    public class JudgeDataCache : IJudgeDataCache
    {
        private CurrentFlight _dto;
        public JudgeDataCache()
        {

        }
        public void Update(CurrentFlight dto)
        {
            this._dto = dto;
        }

        public CurrentFlight Data => this._dto;
    }

    public interface IJudgeDataCache
    {
        CurrentFlight Data { get; }

        void Update(CurrentFlight dto);
    }
}
