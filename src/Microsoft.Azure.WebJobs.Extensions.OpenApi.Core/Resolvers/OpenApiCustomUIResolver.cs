using System;
using System.Linq;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers
{
    /// <summary>
    /// This represents the resolver entity for <see cref="IOpenApiCustomUIOptions"/>.
    /// </summary>
    public static class OpenApiCustomUIResolver
    {
        /// <summary>
        /// Gets the <see cref="IOpenApiCustomUIOptions"/> instance from the given assembly.
        /// </summary>
        /// <param name="assembly">The executing assembly instance.</param>
        /// <returns>Returns the <see cref="IOpenApiCustomUIOptions"/> instance resolved.</returns>
        public static IOpenApiCustomUIOptions Resolve(Assembly assembly)
        {
            var type = assembly.GetLoadableTypes()
                               .SingleOrDefault(p => p.HasInterface<IOpenApiCustomUIOptions>() == true
                                                  && p.HasCustomAttribute<ObsoleteAttribute>() == false
                                                  && p.HasCustomAttribute<OpenApiCustomUIOptionsIgnoreAttribute>() == false);
            if (type.IsNullOrDefault())
            {
                return new DefaultOpenApiCustomUIOptions(assembly);
            }

            var options = Activator.CreateInstance(type, assembly);

            return options as IOpenApiCustomUIOptions;
        }
    }
}
