using System;
using System.Linq;
using System.Reflection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers
{
    /// <summary>
    /// This represents the resolver entity for <see cref="OpenApiServer"/>.
    /// </summary>
    public static class OpenApiConfigurationResolver
    {
        /// <summary>
        /// Gets the <see cref="IOpenApiConfigurationOptions"/> instance from the given assembly.
        /// </summary>
        /// <param name="assembly">The executing assembly instance.</param>
        /// <returns>Returns the <see cref="IOpenApiConfigurationOptions"/> instance resolved.</returns>
        public static IOpenApiConfigurationOptions Resolve(Assembly assembly)
        {
            var type = assembly.GetLoadableTypes()
                               .SingleOrDefault(p => p.GetInterface("IOpenApiConfigurationOptions", ignoreCase: true).IsNullOrDefault() == false
                                                  && p.IsAbstract == false
                                                  && p.GetCustomAttribute<ObsoleteAttribute>(inherit: false).IsNullOrDefault() == true);
           if (type.IsNullOrDefault())
            {
                var settings = new DefaultOpenApiConfigurationOptions();

                return settings;
            }

            var options = Activator.CreateInstance(type);

            return options as IOpenApiConfigurationOptions;
        }
    }
}
