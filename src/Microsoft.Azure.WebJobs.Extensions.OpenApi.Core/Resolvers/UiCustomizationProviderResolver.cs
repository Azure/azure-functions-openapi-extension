using System;
using System.Linq;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers
{
    /// <summary>
    /// This represents the resolver entity for <see cref="IUiCustomizationProvider"/>.
    /// </summary>
    public static class UiCustomizationProviderResolver
    {
        /// <summary>
        /// Gets the <see cref="IUiCustomizationProvider"/> instance from the given assembly.
        /// </summary>
        /// <param name="assembly">The executing assembly instance.</param>
        /// <returns>Returns the <see cref="IUiCustomizationProvider"/> instance resolved.</returns>
        public static IUiCustomizationProvider Resolve(Assembly assembly)
        {
            var type = assembly.GetTypes()
                               .SingleOrDefault(p => p.GetInterface("IUiCustomizationProvider", ignoreCase: true).IsNullOrDefault() == false);
            if (type.IsNullOrDefault())
            {
                return new DefaultUiCustomizationProvider();
            }

            var options = Activator.CreateInstance(type);

            return options as IUiCustomizationProvider;
        }
    }
}
