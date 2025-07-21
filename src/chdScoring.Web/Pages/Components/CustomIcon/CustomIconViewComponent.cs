using chdScoring.Web.Models;
using chdScoring.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace chdScoring.Web.Pages.Shared.Components.CustomIcon
{
    public class CustomIconViewComponent : ViewComponent
    {
        private readonly IconStyleService _iconStyleService;

        public CustomIconViewComponent(IconStyleService iconStyleService)
        {
            this._iconStyleService = iconStyleService;
        }
        public IViewComponentResult Invoke(CustomIconModel model)
        {
            model.Style ??= this._iconStyleService.IconStyle;
            return View("Default", model);
        }
    }
}
