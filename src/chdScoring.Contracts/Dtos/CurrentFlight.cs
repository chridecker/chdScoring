using System;
using System.Collections.Generic;

namespace chdScoring.Contracts.Dtos
{
    public class CurrentFlight : RoundDataDto
    {
        public bool EditScoreEnabled { get; set; }
        public TimeSpan? LeftTime { get; set; }
        public TimeSpan? StartTime { get; set; }
       
    }
}
