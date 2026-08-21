using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using chdScoring.Contracts.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace chdScoring.Main.WebServer.Auth
{
    public class ApiKeyAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IOptions<ApiKeyAuthenticationOptions> apiKeyOptions)
        : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        private readonly ApiKeyAuthenticationOptions _apiKeyOptions = apiKeyOptions.Value;

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.HttpContext.User.Identity is { IsAuthenticated: true })
            {
                return await Task.FromResult(AuthenticateResult.NoResult());
            }

            var apiKey = _apiKeyOptions.ApiKey;
            const string HEADER_KEY = "X-API-KEY";
            var headerValue = Request.Headers
                .SingleOrDefault(h => h.Key.Equals(HEADER_KEY, StringComparison.InvariantCultureIgnoreCase))
                .Value.SingleOrDefault();

            if (string.IsNullOrEmpty(headerValue) || !headerValue.Equals(apiKey, StringComparison.InvariantCultureIgnoreCase))
            {
                return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }

            var claims = new Claim[] { new("sub", "API KEY") };
            var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(claimsIdentity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            var user = new GenericPrincipal(claimsIdentity,
                claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray());
            Request.HttpContext.User = user;
            return AuthenticateResult.Success(ticket);
        }
    }
}
