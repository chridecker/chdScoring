using chdScoring.Main.WebServer.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using chdScoring.Contracts.Settings;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace chdScoring.Main.WebServer.Extensions
{
    public static class ApiAuthKeyExtensions
    {
        public static IServiceCollection AddApiKeyAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiKeyAuthenticationOptions>(configuration.GetSection(nameof(ApiKeyAuthenticationOptions)));

            services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, ApiKeyAuthHandler>(ApiKeyAuthenticationOptions.SECTION_NAME, _ => { });
            services.AddAuthorization();

            return services;
        }

        public static TBuilder RequireApiKeyAuth<TBuilder>(this TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
        {
            builder.RequireAuthorization(a =>
            {
                a.AddAuthenticationSchemes(ApiKeyAuthenticationOptions.SECTION_NAME);
                a.RequireAssertion(s => s.User.Identity?.IsAuthenticated ?? false);
            });

            return builder;
        }
    }
}
