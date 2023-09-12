using System;
using System.Linq;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

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
                               .SingleOrDefault(p => p.HasInterface<IOpenApiConfigurationOptions>() == true
                                                  && p.IsAbstract == false
                                                  && p.HasCustomAttribute<ObsoleteAttribute>() == false
                                                  && p.HasCustomAttribute<OpenApiConfigurationOptionsIgnoreAttribute>() == false);
            if (type.IsNullOrDefault())
            {
                return new DefaultOpenApiConfigurationOptions();
            }

            var options = Activator.CreateInstance(type);

            return options as IOpenApiConfigurationOptions;
        }
    
        /// <summary>
        /// Gets the <see cref="IOpenApiConfigurationOptions"/> instance from the given strategyType.
        /// </summary>
        /// <param name="strategyType">The naming strategy type.</param>
        /// <returns>Returns the NamingStrategy instance resolved.(Override)</returns>
        public static NamingStrategy Resolve(OpenApiNamingStrategy strategyType)
        {
            switch (strategyType)
            {
                case OpenApiNamingStrategy.CamelCase:
                    return new CamelCaseNamingStrategy();
                case OpenApiNamingStrategy.PascalCase:
                    return new DefaultNamingStrategy();
                case OpenApiNamingStrategy.SnakeCase:
                    return new SnakeCaseNamingStrategy();
                case OpenApiNamingStrategy.KebabCase:
                    return new KebabCaseNamingStrategy();
                default:
                    return new CamelCaseNamingStrategy();
            }
        }
    }
}
