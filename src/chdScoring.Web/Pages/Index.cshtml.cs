using chd.Api.Base.Client.Extensions;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.Web.Models;
using chdScoring.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Transactions;

namespace chdScoring.Web.Pages
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IPilotService _pilotService;
        private readonly ImageCache _imageCache;

        public string HubClientUrl => new UriBuilder($"{this._configuration.GetApiKey("chdScoringApi")}chdscoring/flight-hub").Uri.ToString();

        [BindProperty(SupportsGet = true)]
        public string Mode { get; set; }


        public string RenderSetting => string.IsNullOrWhiteSpace(this.Mode) ? "RenderTimer" 
            : (string.Equals(this.Mode,"live",StringComparison.OrdinalIgnoreCase),string.Equals(this.Mode,"round",StringComparison.OrdinalIgnoreCase)) switch
        {
            (true,_) => "RenderLive",
            (_,true) => "RenderRoundResult",
            _ => "RenderTimer"
        };

        public IndexModel(IConfiguration configuration, IPilotService pilotService, ImageCache imageCache)
        {
            this._configuration = configuration;
            this._pilotService = pilotService;
            this._imageCache = imageCache;
        }

        public void OnGet(string? colorScheme = "light-mode")
        {
            this.ViewData["ColorScheme"] = colorScheme;
        }

        public async Task<IActionResult> OnPostRenderTimer([FromBody] CurrentFlight dto)
        {
            if (dto?.Pilot?.CountryId is not null)
            {
                if (!this._imageCache.CountryImageCache.TryGetValue(dto.Pilot.CountryId, out _))
                {
                    this._imageCache.CountryImageCache[dto.Pilot.CountryId] = await this._pilotService.GetCountryImage(dto.Pilot.CountryId);
                }
            }
            return this.Partial("_Timer", new TimerModel()
            {
                CurrentFlight = dto,
                ImageDto = this._imageCache.CountryImageCache.TryGetValue(dto?.Pilot?.CountryId ?? 0, out var img) ? img : null
            });
        }
        public async Task<IActionResult> OnPostRenderLive([FromBody] CurrentFlight dto)
        {
            if (dto?.Pilot?.CountryId is not null)
            {
                if (!this._imageCache.CountryImageCache.TryGetValue(dto.Pilot.CountryId, out _))
                {
                    this._imageCache.CountryImageCache[dto.Pilot.CountryId] = await this._pilotService.GetCountryImage(dto.Pilot.CountryId);
                }
            }
            return this.Partial("_Live", new LiveModel()
            {
                CurrentFlight = dto,
                ImageDto = this._imageCache.CountryImageCache.TryGetValue(dto?.Pilot?.CountryId ?? 0, out var img) ? img : null
            });
        }

        public async Task<IActionResult> OnPostRenderRoundResult([FromBody] RoundResultContainer container)
        {
            return this.Partial("_RoundResult", container);
        }
    }
}
