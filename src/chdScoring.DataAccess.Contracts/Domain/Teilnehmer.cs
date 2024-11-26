namespace chdScoring.DataAccess.Contracts.Domain
{
    public class Teilnehmer
    {
        public int Id { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public int Land { get; set; }
        public string Club { get; set; }
        public string License { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Strasse { get; set; }
        public string Ort { get; set; }
        public string Plz { get; set; }
        public int Bild { get; set; }
        public string Info { get; set; }

        public string FullName => $"{this.Vorname} {this.Nachname.ToUpper()}";

        public virtual Country_Images Country_Image { get; set; }
        public virtual Images Image { get; set; }
    }
}
