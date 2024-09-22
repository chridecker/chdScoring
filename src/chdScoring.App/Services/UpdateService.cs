using chd.UI.Base.Client.Implementations.Services.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
