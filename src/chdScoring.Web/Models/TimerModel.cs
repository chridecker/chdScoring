using chdScoring.Contracts.Dtos;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace chdScoring.Web.Models
{
    public class TimerModel
    {
        public CurrentFlight CurrentFlight { get; set; }
        public ImageDto ImageDto { get; set; }

        public string GetImageUrl => $"data:{ImageDto.Type};base64,{Convert.ToBase64String(ImageDto.Data)}";
    }
}
