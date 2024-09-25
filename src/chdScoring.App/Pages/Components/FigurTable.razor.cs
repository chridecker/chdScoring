using Microsoft.AspNetCore.Components;
using chdScoring.Contracts.Dtos;

namespace chdScoring.App.Pages.Components
{
    public partial class FigurTable
    {
        [Parameter]
        public IEnumerable<ManeouvreDto> Maneouvres { get; set; }

        private string _cssFigur(ManeouvreDto dto) => dto.Current ? " current " : "";
    }
}