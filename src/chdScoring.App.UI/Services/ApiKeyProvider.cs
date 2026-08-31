using System;
using System.Collections.Generic;
using System.Text;
using chd.Api.Base.Contracts.Interfaces;
using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Interfaces;

namespace chdScoring.App.UI.Services
{
    public class ApiKeyProvider(ISettingManager settingManager) : IApiKeyProvider
    {
        public Task<string> GetApiKeyAsync(CancellationToken cancellationToken = new CancellationToken()) => settingManager.ApiKey;
    }
}
