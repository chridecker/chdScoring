using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Dtos
{
    public class ImportRoundScoreDto
    {
        public int Pilot { get; set; }
        public int Round { get; set; }
        public int Judge{ get; set; }
        public List<ScoreImportDto> Scores { get; set; } = new List<ScoreImportDto>();
    }
}
