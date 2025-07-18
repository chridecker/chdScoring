using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Dtos
{
    public class SetStartNumberDto
    {
        public PilotDto Pilot { get; set; }
        public int NewStartId { get; set; }
    }
}
