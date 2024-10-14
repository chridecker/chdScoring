using chdScoring.Contracts.Dtos;
using chdScoring.App.UI.Interfaces;
namespace chdScoring.App.UI.Services
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
