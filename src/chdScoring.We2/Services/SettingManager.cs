using chd.UI.Base.Client.Implementations.Services.Base;
using chd.UI.Base.Contracts.Interfaces.Services;
using chd.UI.Base.Contracts.Interfaces.Services.Base;
using Microsoft.AspNetCore.Components;

namespace chdScoring.Web.Services
{
    public class SettingManager : BaseClientSettingManager<int, int>, ISettingManager
    {
        public SettingManager(ILogger<SettingManager> logger, IProtecedLocalStorageHandler protecedLocalStorageHandler, NavigationManager navigationManager) : base(logger, protecedLocalStorageHandler, navigationManager)
        {
        }
    }

    public interface ISettingManager : IBaseClientSettingManager
    {

    }
}
