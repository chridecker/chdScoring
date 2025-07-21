using chd.Api.Base.Client.Extensions;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.Web.Models;
using chdScoring.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace chdScoring.Web.Pages
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly IPilotService _pilotService;
        private readonly ImageCache _imageCache;

        public string HubClientUrl => new UriBuilder($"{this._configuration.GetApiKey("chdScoringApi")}chdscoring/flight-hub").Uri.ToString();
        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration, IPilotService pilotService, ImageCache imageCache)
        {
            _logger = logger;
            this._configuration = configuration;
            this._pilotService = pilotService;
            this._imageCache = imageCache;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostRenderPartial([FromBody] CurrentFlight dto)
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
    }
}
