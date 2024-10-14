using System;

namespace chdScoring.DataAccess.Contracts.Domain
{
    public class Wettkampf_Leitung
    {
        public int Start { get; set; }
        public int Teilnehmer { get; set; }
        public int Durchgang { get; set; }
        public int Status { get; set; }
        public TimeSpan Start_Time { get; set; }

        public virtual Teilnehmer Pilot { get; set; }
        public virtual Round Round { get; set; }
    }
}
