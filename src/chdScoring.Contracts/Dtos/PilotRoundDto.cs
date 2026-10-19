using System;
using System.Collections.Generic;
using System.Text;
using chdScoring.Contracts.Enums;

namespace chdScoring.Contracts.Dtos
{
    public class PilotRoundDto
    {
        public PilotDto Pilot { get; set; }
        public int Round { get; set; }
        public int StartNr { get; set; }
        public EFlightState State { get; set; }
    }
}
