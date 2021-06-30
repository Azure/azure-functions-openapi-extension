using System.Reflection;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="FunctionNameAttribute"/>.
    /// </summary>
    public static class FunctionNameAttributeExtensions
    {
        /// <summary>
        /// Gets the <see cref="FunctionNameAttribute"/> instance.
        /// </summary>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <returns><see cref="FunctionNameAttribute"/> instance.</returns>
        public static FunctionNameAttribute GetFunctionName(this MethodInfo element)
        {
            element.ThrowIfNullOrDefault();

            var function = element.GetCustomAttribute<FunctionNameAttribute>(inherit: false);

            return function;
        }
    }
}
