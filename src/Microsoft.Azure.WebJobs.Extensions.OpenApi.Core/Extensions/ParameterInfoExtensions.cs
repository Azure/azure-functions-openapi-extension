using System;
using System.Reflection;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="ParameterInfo"/>.
    /// </summary>
    public static class ParameterInfoExtensions
    {
        /// <summary>
        /// Checks whether the custom attribute exists or not.
        /// </summary>
        /// <typeparam name="T">Type of custom attribute.</typeparam>
        /// <param name="element"><see cref="ParameterInfo"/> instance.</param>
        /// <param name="inherit">Value indicating whether to inspect ancestors or not. Default is <c>False</c>.</param>
        /// <returns><c>True</c>, if custom attribute exists; otherwise returns <c>False</c>.</returns>
        public static bool ExistsCustomAttribute<T>(this ParameterInfo element, bool inherit = false) where T : Attribute
        {
            element.ThrowIfNullOrDefault();

            var exists = element.GetCustomAttribute<T>(inherit) != null;

            return exists;
        }
    }
}
