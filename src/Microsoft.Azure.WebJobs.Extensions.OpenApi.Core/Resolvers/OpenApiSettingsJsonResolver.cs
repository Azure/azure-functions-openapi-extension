using System.IO;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configuration.AppSettings.Resolvers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

using Microsoft.Extensions.Configuration;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers
{
    /// <summary>
    /// This represents the resolver entity for openapisettings.json.
    /// </summary>
    public static class OpenApiSettingsJsonResolver
    {
        /// <summary>
        /// Gets the <see cref="IConfiguration"/> instance from openapisettings.json
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

            var builder = new ConfigurationBuilder();

            if (!File.Exists($"{basePath.TrimEnd('/')}/openapisettings.json"))
            {
                return builder.Build();
            }

            var openapi = builder.SetBasePath(basePath)
                                 .AddJsonFile("openapisettings.json")
                                 .Build();

            return openapi;
        }
    }
}
