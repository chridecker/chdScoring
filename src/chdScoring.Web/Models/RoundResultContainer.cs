using chdScoring.Contracts.Dtos;

namespace chdScoring.Web.Models
{
    public class RoundResultContainer
    {
        public List<RoundResultDto> Dtos { get; set; } = new List<RoundResultDto>();
        public string GetImageUrl(ImageDto dto) => $"data:{dto.Type};base64,{Convert.ToBase64String(dto.Data)}";
    }
}
