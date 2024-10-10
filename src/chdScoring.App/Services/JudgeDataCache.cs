using chdScoring.Contracts.Dtos;
using chdScoring.App.Interfaces;
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

   
}
