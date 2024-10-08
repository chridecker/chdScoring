using System;

namespace chdScoring.Contracts.Dtos
{
    public class SaveRoundDto
    {
        public int Round { get; set; }
        public int Pilot { get; set; }
        public decimal Score { get; set; }
        public TimeSpan Duration { get; set; }

    }
}