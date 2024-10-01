using chdScoring.App.Services;
using Microsoft.AspNetCore.Components;
using chdScoring.Contracts.Interfaces;
using chd.Api.Base.Contracts.Interfaces;
using chd.Api.Base.Client.Extensions;
using chd.UI.Base.Contracts.Interfaces.Authentication;

namespace chdScoring.App
{
    public partial class Main
    {
        [Inject] IchdScoringProfileService _profileService { get; set; }
        [Inject] IHandleUserIdLogin<int> _handleUserIdLogin { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (await this._handleUserIdLogin.IsEnabled)
            {
                var id = await this._handleUserIdLogin.GetIdAsync();
                if (id.HasValue)
                {
                    await this._profileService.LoginAsync(new()
                    {
                        Id = id,
                        StayLoggedIn = true,
                    });
                }
            }

            await base.OnInitializedAsync();
        }
    }
}