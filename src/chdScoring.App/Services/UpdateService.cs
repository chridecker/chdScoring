using chd.UI.Base.Client.Implementations.Services.Base;
using Microsoft.Extensions.Logging;

namespace chdScoring.App.Services
{
    public class UpdateService : BaseUpdateService
    {
        public UpdateService(ILogger<UpdateService> logger) : base(logger)
        {
        }

        public override Task UpdateAsync(int timeout)
        {
            throw new NotImplementedException();
        }

        public override Task<Version> CurrentVersion() => Task.FromResult(AppInfo.Version);

    }
}
