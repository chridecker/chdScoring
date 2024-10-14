namespace chdScoring.Contracts.Dtos
{
    public class RoundResultDto
    {
        public PilotDto Pilot { get; set; }
        public int StartNumber { get; set; }
        public int Round { get; set; }
        public int Rank { get; set; }
        public decimal Score { get; set; }
        public decimal ScoreProm { get; set; }
    }
}
