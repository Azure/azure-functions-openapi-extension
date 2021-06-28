using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="FunctionAttribute"/>.
    /// </summary>
    public static class FunctionAttributeExtensions
    {
        /// <summary>
        /// Gets the <see cref="FunctionAttribute"/> instance.
        /// </summary>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <returns><see cref="FunctionAttribute"/> instance.</returns>
        public static FunctionAttribute GetFunctionName(this MethodInfo element)
        {
            element.ThrowIfNullOrDefault();

            var function = element.GetCustomAttribute<FunctionAttribute>(inherit: false);

            return function;
        }
    }
}
