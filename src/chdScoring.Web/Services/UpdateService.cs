using chd.UI.Base.Client.Implementations.Services.Base;

namespace chdScoring.Web.Services
{
    public class UpdateService : BaseUpdateService
    {
        public UpdateService(ILogger<BaseUpdateService> logger) : base(logger)
        {
        }

        public override Task UpdateAsync(int timeout)
        {
            throw new NotImplementedException();
        }
    }
}
