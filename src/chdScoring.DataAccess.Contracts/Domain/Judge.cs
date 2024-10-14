namespace chdScoring.DataAccess.Contracts.Domain
{
    public class Judge
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Vorname { get; set; }
        public int Bild { get; set; }
        public string Club { get; set; }
        public int Land { get; set; }
        public string License { get; set; }
        public int Pin { get; set; }
    }
}
