using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Dtos
{
    public class FCScoringDto
    {
        public string Schedule { get; set; }
        public List<FCScoreDto> Scores { get; set; }
    }

    public class FCScoreDto
    {
        public string Name { get; set; }
        public int  Index { get; set; }
        public decimal Score { get; set; }
        public int K { get; set; }

    }
}
