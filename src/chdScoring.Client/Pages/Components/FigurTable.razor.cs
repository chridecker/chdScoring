using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using chdScoring.Client;
using chdScoring.Client.Shared;
using chdScoring.Client.Constants;
using chdScoring.Contracts.Dtos;

namespace chdScoring.Client.Pages.Components
{
    public partial class FigurTable
    {
        [Parameter]
        public IEnumerable<ManeouvreDto> Maneouvres { get; set; }

        private string _cssFigur(ManeouvreDto dto) => dto.Current ? "background-color: darkgreen;color:white;" : "";
        private string _cssClass(ManeouvreDto dto) => dto.Id % 2 == 0 ? "grey" : "";
    }
}