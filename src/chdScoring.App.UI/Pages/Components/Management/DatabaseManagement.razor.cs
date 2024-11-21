using chdScoring.App.UI.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Components;

namespace chdScoring.App.UI.Pages.Components.Management
{
    public partial class DatabaseManagement
    {
        [Inject] IDatabaseService _databaseService { get; set; }
        [Parameter] public IEnumerable<string> DatabaseConnections { get; set; }
        [Parameter] public string CurrentDatabaseConnection { get; set; }


        private int[] _rights => new int[] { RightConstants.Database };

       
        private async void OnDatabaseChanged(string db)
        {
            if (await this._databaseService.SetDatabaseConnection(db))
            {
                this.CurrentDatabaseConnection = db;
            }
            await this.InvokeAsync(this.StateHasChanged);
        }
    }
}