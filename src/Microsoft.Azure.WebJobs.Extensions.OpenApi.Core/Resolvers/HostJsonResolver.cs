using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Resolvers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

using Microsoft.Extensions.Configuration;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers
{
    /// <summary>
    /// This represents the resolver entity for host.json.
    /// </summary>
    public static class HostJsonResolver
    {
        /// <summary>
        /// Gets the <see cref="IConfiguration"/> instance from host.json
        /// </summary>
        /// <param name="config"><see cref="IConfiguration"/> instance from the environment variables - either local.settings.json or App Settings blade.</param>
        /// <param name="basePath">Base path of the executing Azure Functions assembly.</param>
        public static IConfiguration Resolve(IConfiguration config = null, string basePath = null)
        {
            if (config.IsNullOrDefault())
            {
                config = ConfigurationResolver.Resolve();
            }

            if (basePath.IsNullOrWhiteSpace())
            {
                basePath = ConfigurationResolver.GetBasePath(config);
            }

            var host = new ConfigurationBuilder()
                           .SetBasePath(basePath)
                           .AddJsonFile("host.json")
                           .Build();

            return host;
        }

        /// <summary>
        /// Gets the <see cref="HttpSettings"/> instance.
        /// </summary>
        /// <param name="host"><see cref="IConfiguration"/> instance from host.json.</param>
        /// <returns>Returns <see cref="HttpSettings"/> instance.</returns>
        public static HttpSettings GetHttpSettings(this IConfiguration host)
        {
            var version = host.GetSection("version").Value;
            var httpSettings = version.IsNullOrWhiteSpace()
                                    ? host.Get<HttpSettings>("http")
                                    : (version.Equals("2.0", StringComparison.CurrentCultureIgnoreCase)
                                           ? host.Get<HttpSettings>("extensions:http")
                                           : host.Get<HttpSettings>("http"));

            return httpSettings;
        }
    }
}
