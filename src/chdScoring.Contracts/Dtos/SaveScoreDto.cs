namespace chdScoring.Contracts.Dtos
{
    public class SaveScoreDto
    {
        public int Figur { get; set; }
        public int Pilot { get; set; }
        public int Round { get; set; }
        public int Judge { get; set; }
        public decimal Value { get; set; }
    }
}
