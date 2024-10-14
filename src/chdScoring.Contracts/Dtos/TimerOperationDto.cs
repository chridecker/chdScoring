using chdScoring.Contracts.Enums;

namespace chdScoring.Contracts.Dtos
{
    public class TimerOperationDto
    {
        public ETimerOperation Operation { get; set; }
        public int Airfield { get; set; }
    }
}
