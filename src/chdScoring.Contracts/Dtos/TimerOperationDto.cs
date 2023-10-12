using chdScoring.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Dtos
{
    public class TimerOperationDto
    {
        public ETimerOperation Operation { get; set; }
        public int Airfield { get; set; }
    }
}
