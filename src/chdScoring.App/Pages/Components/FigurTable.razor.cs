using Microsoft.AspNetCore.Components;
using chdScoring.Contracts.Dtos;

namespace chdScoring.App.Pages.Components
{
    public partial class FigurTable
    {
        [Parameter]
        public IEnumerable<ManeouvreDto> Maneouvres { get; set; }

        private string _cssFigur(ManeouvreDto dto) => dto.Current ? "background-color: darkgreen;color:white;" : "";
        private string _cssClass(ManeouvreDto dto) => dto.Id % 2 == 0 ? "grey" : "";
    }
}