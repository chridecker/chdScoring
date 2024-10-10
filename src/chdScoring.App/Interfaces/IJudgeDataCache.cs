using chdScoring.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.Interfaces
{
     public interface IJudgeDataCache
    {
        CurrentFlight Data { get; }

        void Update(CurrentFlight dto);
    }
}
