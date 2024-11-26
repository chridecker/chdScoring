using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Dtos
{
    public class FinishedRoundDto
    {
        public PilotDto Pilot { get; set; }
        public RoundDto Round { get; set; }
        public int Start { get; set; }
    }
}
