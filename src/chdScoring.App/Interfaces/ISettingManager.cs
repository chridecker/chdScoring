using chd.UI.Base.Contracts.Interfaces.Services.Base;

namespace chdScoring.App.Interfaces
{
    public interface ISettingManager : IBaseClientSettingManager
    {
        Task<string> MainUrl { get; }
        Task UpdateMainUrl(string url);

        event EventHandler<string> AutoRedirectToChanged;

        Task<string> GetAutoRedirectTo();
        Task SetAutoRedirectTo(string value);
        T? GetLocalSetting<T>(string key) where T : class;
        void SetLocalSetting<T>(string key, T value) where T : class;
    }
}
