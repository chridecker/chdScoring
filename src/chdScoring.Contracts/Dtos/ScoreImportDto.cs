using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Dtos
{
    public class ScoreImportDto
    {
        public int Figure { get; set; }
        public decimal Value { get; set; }
        public decimal Intra { get; set; }
        public decimal Inter { get; set; }
        public decimal Box { get; set; }
    }
}
