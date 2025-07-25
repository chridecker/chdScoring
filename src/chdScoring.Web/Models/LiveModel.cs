using chdScoring.Contracts.Dtos;

namespace chdScoring.Web.Models
{
    public class LiveModel
    {
        public CurrentFlight CurrentFlight { get; set; }
        public ImageDto ImageDto { get; set; }

        public string GetImageUrl => $"data:{ImageDto.Type};base64,{Convert.ToBase64String(ImageDto.Data)}";

        public decimal? ScoreValue(int jugdeId, ManeouvreDto maneouvre) => this.CurrentFlight.ManeouvreLst[jugdeId].FirstOrDefault(x => x.Id == maneouvre.Id)?.Score;
        public string ScoreValueText(int jugdeId, ManeouvreDto maneouvre) => this.ScoreValue(jugdeId, maneouvre).HasValue && this.ScoreValue(jugdeId, maneouvre).Value < 0 ? "NO" : (ScoreValue(jugdeId, maneouvre) ?? 0).ToString("0.#");
    }
}
