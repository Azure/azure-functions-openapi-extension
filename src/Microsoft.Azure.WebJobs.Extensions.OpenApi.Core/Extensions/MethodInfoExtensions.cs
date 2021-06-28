using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="MethodInfo"/>.
    /// </summary>
    public static class MethodInfoExtensions
    {
        /// <summary>
        /// Gets the <see cref="OpenApiOperationAttribute"/> instance.
        /// </summary>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <returns><see cref="OpenApiOperationAttribute"/> instance.</returns>
        public static OpenApiOperationAttribute GetOpenApiOperation(this MethodInfo element)
        {
            element.ThrowIfNullOrDefault();

            var operation = element.GetCustomAttribute<OpenApiOperationAttribute>(inherit: false);

            return operation;
        }
    }
}
