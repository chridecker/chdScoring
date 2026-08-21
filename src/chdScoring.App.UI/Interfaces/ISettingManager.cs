using chd.UI.Base.Contracts.Interfaces.Services.Base;

namespace chdScoring.App.UI.Interfaces
{
    public interface ISettingManager : IBaseClientSettingManager
    {
        Task<string> MainUrl { get; }
        Task<string> ApiKey { get; }
        Task UpdateMainUrl(string url);
        Task UpdateApiKey(string key);

        event EventHandler<string> AutoRedirectToChanged;

        Task<string> GetAutoRedirectTo();
        Task<int> GetScoringZoom();
        Task<bool> GetUseJudgeConfirmQuestion();
        Task SetAutoRedirectTo(string value);
        T? GetNativSetting<T>(string key) where T : class;
        void SetNativSetting<T>(string key, T value) where T : class;
        bool IsiOS { get; }
        Task ShowToast(string message, CancellationToken cancellationToken = default);
        void CloseApp();
    }
}
