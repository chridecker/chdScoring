using chdScoring.Contracts.Dtos;

namespace chdScoring.Web.Models
{
    public class LiveModel
    {
        public CurrentFlight CurrentFlight { get; set; }
        public ImageDto ImageDto { get; set; }

        public string GetImageUrl => $"data:{ImageDto.Type};base64,{Convert.ToBase64String(ImageDto.Data)}";
    }
}
