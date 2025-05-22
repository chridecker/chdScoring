using chd.UI.Base.Client.Implementations.Services.Base;
using Microsoft.Extensions.Logging;

namespace chdScoring.App.Services
{
    public class UpdateService : BaseUpdateService
    {
        private readonly IAppInfo _appInfo;

        public UpdateService(ILogger<UpdateService> logger, IAppInfo appInfo) : base(logger)
        {
            this._appInfo = appInfo;
        }

        public override Task<Version> CurrentVersion()
        {
            return Task.FromResult(this._appInfo.Version);
        }

        public override Task UpdateAsync(int timeout)
        {
            return Task.CompletedTask;
        }

    }
}
