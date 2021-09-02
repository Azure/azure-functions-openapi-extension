using System;
using System.Reflection;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="FunctionNameAttribute"/>.
    /// </summary>
    internal static class FunctionNameAttributeExtensions
    {
        /// <summary>
        /// Gets the <see cref="FunctionNameAttribute"/> instance.
        /// </summary>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <returns><see cref="FunctionNameAttribute"/> instance.</returns>
        public static T GetFunctionName<T>(this MethodInfo element)
            where T : Attribute
        {
            element.ThrowIfNullOrDefault();

            var function = element.GetCustomAttribute<T>(inherit: false);

            return function;
        }
    }
}
