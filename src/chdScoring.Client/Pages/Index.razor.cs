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
using chdScoring.Client.Services;
using chdScoring.Contracts.Dtos;

namespace chdScoring.Client.Pages
{

    public partial class Index
    {
        private CancellationTokenSource _cts = new();
        private CurrentFlight _dto;
        private JudgeDto Judge => this._dto.Judges.FirstOrDefault(x => x.Id == this._judge);

        private bool _panelDisabled => this._dto?.LeftTime.HasValue ?? false ? this._dto.LeftTime <= TimeSpan.Zero : !this.Maneouvres.Any(x => x.Current);

        private string _buttonCss => this._panelDisabled ? "grey" : "";
        private string _cssFigur(ManeouvreDto dto) => dto.Current ? "background-color: darkgreen;color:white;" : "";

        private string _scoreValueText => this._scoreValue.HasValue ? this._scoreValue.Value < 0 ? "NO" : this._scoreValue.Value.ToString("n1") : "";

        private decimal? _scoreValue;

        private IEnumerable<ManeouvreDto> Maneouvres
        {
            get
            {
                if (this._dto.ManeouvreLst.TryGetValue(this._judge, out var lst))
                {
                    return lst;
                }
                return Enumerable.Empty<ManeouvreDto>();
            }
        }

        private int _judge;

        [Inject] private IMainService _mainService { get; set; }
        [Inject] private ISettingManager _settingManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this._judge = await this._settingManager.Judge;
            this._dto = await this._mainService.GetCurrentFlight(this._cts.Token);
            this.Reload(this._cts.Token);
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
        }
        private async Task Del()
        {
            this._scoreValue = null;
        }

        private async Task NotObserved()
        {
            this._scoreValue = -1;
        }
        private async Task Calc(decimal i)
        {
            if (this._scoreValue.HasValue && _scoreValue < 10 && i == 5)
            {
                this._scoreValue += i / 10;
            }
            else if (!this._scoreValue.HasValue)
            {
                this._scoreValue = i;
            }
        }
        private async Task Save()
        {
            if (this._scoreValue.HasValue)
            {
                if (!(await this._mainService.SaveScore(this._dto.Pilot.Id, this.Maneouvres.FirstOrDefault(x => x.Current).Id, this._judge, this._dto.Round, this._scoreValue.Value, this._cts.Token)))
                {
                    
                }
                else
                {
                    this._scoreValue = null;
                }
            }
        }


        private void Reload(CancellationToken cancellationToken) => Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                this._dto = await this._mainService.GetCurrentFlight(cancellationToken);
                await this.InvokeAsync(this.StateHasChanged);
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }, cancellationToken);
    }

}