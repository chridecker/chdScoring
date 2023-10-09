using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Dtos
{
    public class ManeouvreDto
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string Name { get; set; }
        public decimal? Score { get; set; }
        public bool Current { get; set; }
        public bool Saved => this.Score.HasValue;

    }
}
