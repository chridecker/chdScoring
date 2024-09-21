using chdScoring.Contracts.Dtos;

namespace chdScoring.App.Services
{
    public class JudgeDataCache : IJudgeDataCache
    {
        private CurrentFlight _dto;
        public JudgeDataCache()
        {

        }
        public void Update(CurrentFlight dto)
        {
            this._dto = dto;
        }

        public CurrentFlight Data => this._dto;
    }

    public interface IJudgeDataCache
    {
        CurrentFlight Data { get; }

        void Update(CurrentFlight dto);
    }
}
