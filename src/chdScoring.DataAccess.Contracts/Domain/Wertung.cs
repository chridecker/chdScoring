namespace chdScoring.DataAccess.Contracts.Domain
{
    public class Wertung
    {
        public int Teilnehmer { get; set; }
        public int Durchgang{ get; set; }
        public int Figur{ get; set; }
        public int Judge{ get; set; }
        public decimal Wert{ get; set; }
    }
}
