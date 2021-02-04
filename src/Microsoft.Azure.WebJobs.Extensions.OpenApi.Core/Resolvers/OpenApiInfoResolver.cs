using System;
using System.Linq;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers
{
    /// <summary>
    /// This represents the resolver entity for <see cref="OpenApiInfo"/> from one of host.json, openapisettings.json and environment variables.
    /// </summary>
    public static class OpenApiInfoResolver
    {
        /// <summary>
        /// Gets the <see cref="OpenApiInfo"/> instance from one of host.json, openapisettings.json and environment variables.
        /// </summary>
        /// <param name="assembly">The executing assembly instance.</param>
        /// <returns>Returns <see cref="OpenApiInfo"/> instance resolved.</returns>
        public static OpenApiInfo Resolve(Assembly assembly)
        {
            var type = assembly.GetTypes()
                               .SingleOrDefault(p => p.GetInterface("IOpenApiConfigurationOptions", ignoreCase: true).IsNullOrDefault() == false);
            if (type.IsNullOrDefault())
            {
                var settings = new OpenApiSettings();

                return settings.Info;
            }

            var options = Activator.CreateInstance(type);

            return (options as IOpenApiConfigurationOptions).Info;
        }
    }
}
