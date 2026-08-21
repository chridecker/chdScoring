using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Settings
{
    public class ApiKeyAuthenticationOptions
    {
        /// <summary>
        ///     The section name of the underlying JSON configuration
        /// </summary>
        public const string SECTION_NAME = "ApiKeyAuthentication";

        /// <summary>
        ///     The Api Key
        /// </summary>
        public string? ApiKey { get; set; }
    }
}
