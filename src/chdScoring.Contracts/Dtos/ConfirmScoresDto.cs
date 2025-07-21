using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Dtos
{
    public class ConfirmScoresDto
    {
        public int Pilot { get; set; }
        public int Round { get; set; }
        public int Judge { get; set; }
        public DateTime Time { get; set; }
    }
}
