using System;

namespace chdScoring.Contracts.Dtos
{
    public class ManeouvreHistoryDto
    {
        public float OldScore { get; set; }
        public float Score { get; set; }
        public DateTime Changed { get; set; }
    }
}
