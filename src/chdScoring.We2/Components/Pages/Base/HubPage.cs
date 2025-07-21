using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.Web.Services;
using Microsoft.AspNetCore.Components;

namespace chdScoring.Web.Components.Pages.Base
{
    public abstract class HubPage : ComponentBase
    {
        [Inject] protected HubClient hubClient { get; set; }
        [Inject] protected IPilotService pilotService { get; set; }

        protected ImageDto _countryImg;
        protected CurrentFlight _dto;
        int? _lastImage;

        protected string _leftTime => (this._dto?.LeftTime.HasValue ?? false) && this._dto.LeftTime.Value <= this._dto.Round.Time ?
                this._dto.LeftTime.Value > TimeSpan.Zero ? this._dto.LeftTime.Value.ToString("mm\\:ss") : TimeSpan.Zero.ToString("mm\\:ss") : this._dto.Round.Time.ToString("mm\\:ss");

        protected string _icon => (this._dto?.LeftTime.HasValue ?? false) && this._dto.LeftTime.Value <= this._dto.Round.Time ?
                this._dto.LeftTime.Value > TimeSpan.Zero ? this._dto.LeftTime.Value.ToString("mm\\:ss") : TimeSpan.Zero.ToString("mm\\:ss") : this._dto.Round.Time.ToString("mm\\:ss");


        protected override async Task OnInitializedAsync()
        {
            if (!this.hubClient.IsConnected)
            {
                await this.hubClient.StartAsync();
            }
            await this.hubClient.RegisterControlCenter();
            this.hubClient.DataReceived += this.HubClient_DataReceived;
            await base.OnInitializedAsync();
        }

        private async void HubClient_DataReceived(object? sender, CurrentFlight dto)
        {
            this._dto = dto;
            if (!this._lastImage.HasValue || this._lastImage.Value != dto?.Pilot?.CountryId)
            {
                this._lastImage = dto?.Pilot?.CountryId;
                if (dto?.Pilot is not null)
                {
                    this._countryImg = await this.pilotService.GetCountryImage(dto.Pilot.CountryId);
                }
                else
                {
                    _countryImg = null;
                }
            }
            await this.InvokeAsync(this.StateHasChanged);
        }


        public virtual void Dispose()
        {
            this.hubClient.DataReceived -= this.HubClient_DataReceived;
        }
    }
}
