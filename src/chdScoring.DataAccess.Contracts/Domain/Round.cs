using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.DataAccess.Contracts.Domain
{
    public class Round
    {
        public int Teilnehmer { get; set; }
        public int Durchgang { get; set; }
        public int Duration { get; set; }
        public decimal Wert_abs { get; set; }
        public double Wert_prom { get; set; }
    }
}
