using System;
using System.Linq;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers
{
    /// <summary>
    /// This represents the resolver entity for <see cref="IOpenApiHttpTriggerAuthorization"/>.
    /// </summary>
    public static class OpenApiHttpTriggerAuthorizationResolver
    {
        /// <summary>
        /// Property used to allow for dynamic generation of OpenApiHttpTriggerAuthorization
        /// </summary>
        public static IOpenApiHttpTriggerAuthorization HttpTriggerAuthorization { get; set; }

        /// <summary>
        /// Gets the <see cref="IOpenApiHttpTriggerAuthorization"/> instance from the given assembly.
        /// </summary>
        /// <param name="assembly">The executing assembly instance.</param>
        /// <returns>Returns the <see cref="IOpenApiHttpTriggerAuthorization"/> instance resolved.</returns>
        public static IOpenApiHttpTriggerAuthorization Resolve(Assembly assembly)
        {
            // If we have already dynamically configured the <see cref="IOpenApiHttpTriggerAuthorization"/>, then return it
            if (HttpTriggerAuthorization != null)
            {
                return HttpTriggerAuthorization;
            }

            var type = assembly.GetLoadableTypes()
                               .SingleOrDefault(p => p.GetInterface("IOpenApiHttpTriggerAuthorization", ignoreCase: true).IsNullOrDefault() == false);

            if (type.IsNullOrDefault())
            {
                return new DefaultOpenApiHttpTriggerAuthorization();
            }

            var options = Activator.CreateInstance(type);

            return options as IOpenApiHttpTriggerAuthorization;
        }
    }
}
