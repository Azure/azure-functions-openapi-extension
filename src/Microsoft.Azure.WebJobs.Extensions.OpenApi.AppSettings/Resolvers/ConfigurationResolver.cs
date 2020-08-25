using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.Configuration;

using OperatingSystem = Microsoft.Azure.WebJobs.Extensions.OpenApi.Configuration.AppSettings.Extensions.OperationSystem;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Configuration.AppSettings.Resolvers
{
    /// <summary>
    /// This represents the resolver entity for configuration.
    /// </summary>
    public static class ConfigurationResolver
    {
        /// <summary>
        /// Gets the <see cref="IConfiguration"/> instance from the environment variables - either local.settings.json or App Settings blade.
        /// </summary>
        public static IConfiguration Resolve()
        {
            var config = new ConfigurationBuilder()
                             .AddEnvironmentVariables()
                             .Build();

            return config;
        }

        /// <summary>
        /// Gets the configuration value.
        /// </summary>
        /// <typeparam name="T">Type of return value.</typeparam>
        /// <param name="key">Key for lookup.</param>
        /// <param name="config"><see cref="IConfiguration"/> instance from the environment variables - either local.settings.json or App Settings blade.</param>
        /// <returns>Returns the value.</returns>
        public static T GetValue<T>(string key, IConfiguration config = null)
        {
            if (config == null)
            {
                config = Resolve();
            }

            var value = config.GetValue<T>(key);

            return value;
        }

        /// <summary>
        /// Gets the base path of the executing Azure Functions assembly.
        /// </summary>
        /// <param name="environmentVariables"><see cref="IConfiguration"/> instance representing environment variables from either local.settings.json or App Settings blade.</param>
        /// <returns>Returns the base path of the executing Azure Functions assembly.</returns>
        public static string GetBasePath(IConfiguration environmentVariables)
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var segments = location.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var basePath = string.Join(Path.DirectorySeparatorChar.ToString(), segments.Take(CountDirectories(segments)));

            if (!OperatingSystem.IsWindows())
            {
                basePath = $"/{basePath}";
            }
            return basePath;
        }

        private static int CountDirectories(List<string> segments)
        {
            var bin = segments[segments.Count - 2];
            if (bin == "bin")
            {
                return segments.Count - 2;
            }

            return segments.Count - 1;
        }
    }
}
