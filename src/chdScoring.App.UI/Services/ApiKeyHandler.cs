using System;
using System.Collections.Generic;
using System.Text;
using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Interfaces;

namespace chdScoring.App.UI.Services
{
    public class ApiKeyHandler(ISettingManager settingManager) : IApiKeyHandler
    {
        public async Task<string> ApiKey() => await settingManager.ApiKey;
    }
}
