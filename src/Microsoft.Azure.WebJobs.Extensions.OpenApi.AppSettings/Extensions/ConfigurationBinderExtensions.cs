using System;

using Microsoft.Extensions.Configuration;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Configuration.AppSettings.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="ConfigurationBinder"/> class.
    /// </summary>
    public static class ConfigurationBinderExtensions
    {
        /// <summary>
        /// Gets the instance from the configuration.
        /// </summary>
        /// <typeparam name="T">Type of instance.</typeparam>
        /// <param name="configuration"><see cref="IConfiguration"/> instance.</param>
        /// <param name="key">Configuration key.</param>
        /// <returns>Returns the instance from the configuration.</returns>
        public static T Get<T>(this IConfiguration configuration, string key = null)
        {
            var instance = Activator.CreateInstance<T>();

            if (string.IsNullOrWhiteSpace(key))
            {
                configuration.Bind(instance);

                return instance;
            }

            configuration.Bind(key, instance);

            return instance;
        }
    }
}