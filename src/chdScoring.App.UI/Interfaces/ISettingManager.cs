using chd.UI.Base.Contracts.Interfaces.Services.Base;

namespace chdScoring.App.UI.Interfaces
{
    public interface ISettingManager : IBaseClientSettingManager
    {
        Task<string> MainUrl { get; }
        Task UpdateMainUrl(string url);

        event EventHandler<string> AutoRedirectToChanged;

        Task<string> GetAutoRedirectTo();
        Task SetAutoRedirectTo(string value);
        T? GetNativSetting<T>(string key) where T : class;
        void SetNativSetting<T>(string key, T value) where T : class;
    }
}
