using System;

namespace chdScoring.DataAccess.Contracts.Domain
{
    public class Wertung_History
    {
        public int Teilnehmer { get; set; }
        public int Durchgang { get; set; }
        public int Figur { get; set; }
        public int Judge { get; set; }
        public float Wert_alt { get; set; }
        public float Wert_neu { get; set; }
        public DateTime Time { get; set; }
        public int User { get; set; }

        public virtual Wertung Wertung { get; set; }
    }
}
