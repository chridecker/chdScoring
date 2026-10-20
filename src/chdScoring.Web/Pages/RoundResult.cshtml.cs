using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Web.Pages
{
    [IgnoreAntiforgeryToken]
    public class RoundResultModel(IPilotService pilotService, IStammdatenService stammdatenService) : PageModel
    {

        public RoundDataDto Data { get; set; }
        public ImageDto Logo { get; set; }
        public StammdatenDto Stammdaten { get; set; }

        public decimal JudgeResult(int id) => Data.ManeouvreLst[id].Select(s => (s.Score ?? 0) * s.Value).Sum();
        public decimal RoundResult => Data.ManeouvreLst.Select(s => s.Value.Select(ss => (ss.Score ?? 0) * ss.Value).Sum()).Average();

        public async Task OnGet(int teilnehmer, int round, string? colorScheme = "light-mode")
        {
            ViewData["ColorScheme"] = colorScheme;
            Data = await pilotService.GetRoundData(teilnehmer, round, this.HttpContext.RequestAborted);
            Logo = await stammdatenService.GetImage(2, HttpContext.RequestAborted);
            Stammdaten = await stammdatenService.Get(HttpContext.RequestAborted);
        }
    }
}
