using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Dtos
{
    public class StammdatenDto
    {
        public string TournamentName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
