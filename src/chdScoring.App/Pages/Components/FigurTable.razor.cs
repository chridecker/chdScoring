using Microsoft.AspNetCore.Components;
using chdScoring.Contracts.Dtos;
using chd.UI.Base.Client.Implementations.Services;

namespace chdScoring.App.Pages.Components
{
    public partial class FigurTable : ComponentBase
    {
        [Inject] IScrollInfoService _scrollInfoService { get; set; }
        [Parameter] public IEnumerable<ManeouvreDto> Maneouvres { get; set; }
        [Parameter] public ManeouvreDto Current { get; set; }

        [Parameter] public bool EditManeouvreEnabled { get; set; }
        [Parameter] public Func<ManeouvreDto, Task> EditManeouvre { get; set; }

        private string _cssFigur(ManeouvreDto dto) => dto?.Id == this.Current?.Id ? " current scroll-to-element " : "";
        private string _score(ManeouvreDto dto) => dto.Score.HasValue ? dto.Score.Value.ToString("n1") : "";
    }
}