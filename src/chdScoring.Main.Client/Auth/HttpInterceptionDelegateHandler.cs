using chdScoring.Contracts.Interfaces;
using chdScoring.Contracts.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

public class HttpInterceptionDelegateHandler(IApiKeyHandler keyHandler) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!request.Headers.Contains("X-API-KEY"))
        {
            var key = await keyHandler.ApiKey();
            request.Headers.Add("X-API-KEY", key);
        }

        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        return response;
    }
}
