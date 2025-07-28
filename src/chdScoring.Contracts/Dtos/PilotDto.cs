namespace chdScoring.Contracts.Dtos
{
    public class PilotDto
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Name => $"{this.Firstname} {this.Lastname?.ToUpper()}";
        public string Club { get; set; }
        public string License { get; set; }
        public int CountryId { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public ImageDto CountryImage { get; set; }
    }
}
