using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.DataAccess.Contracts.Domain
{
    public class Tbl_result
    {
        public int Teilnehmer { get; set; }
        public int Durchgang { get; set; }
        public decimal Wert { get; set; }
    }
}
