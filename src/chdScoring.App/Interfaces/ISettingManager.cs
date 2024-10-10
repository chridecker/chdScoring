using chd.UI.Base.Contracts.Interfaces.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.Interfaces
{
     public interface ISettingManager : IBaseClientSettingManager
    {
        Task<string> MainUrl { get; }
        Task UpdateMainUrl(string url);

        event EventHandler<string> AutoRedirectToChanged;

        Task<string> GetAutoRedirectTo();
        Task SetAutoRedirectTo(string value);
    }
}
