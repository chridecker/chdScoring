using chdScoring.Contracts.Dtos;

namespace chdScoring.App.UI.Interfaces
{
    public interface IJudgeDataCache
    {
        CurrentFlight Data { get; }

        void Update(CurrentFlight dto);
    }
}
