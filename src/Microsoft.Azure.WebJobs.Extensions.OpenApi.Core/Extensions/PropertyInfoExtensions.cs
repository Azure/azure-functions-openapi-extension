using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="PropertyInfo"/> class.
    /// </summary>
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Checks whether the given <see cref="PropertyInfo"/> object has <see cref="JsonPropertyAttribute"/> or not.
        /// </summary>
        /// <param name="element"><see cref="PropertyInfo"/> instance.</param>
        /// <returns>Returns <c>True</c>, if the given <see cref="PropertyInfo"/> object has <see cref="JsonPropertyAttribute"/>; otherwise, returns <c>False</c>.</returns>
        public static bool HasJsonPropertyAttribute(this PropertyInfo element)
        {
            var exists = element.ExistsCustomAttribute<JsonPropertyAttribute>();

            return exists;
        }

        /// <summary>
        /// Gets the name from <see cref="JsonPropertyAttribute"/> instance.
        /// </summary>
        /// <param name="element"><see cref="PropertyInfo"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance.</param>
        /// <returns>Returns the name from <see cref="JsonPropertyAttribute"/> instance.</returns>
        public static string GetJsonPropertyName(this PropertyInfo element, NamingStrategy namingStrategy = null)
        {
            if (namingStrategy.IsNullOrDefault())
            {
                namingStrategy = new DefaultNamingStrategy();
            }

            if (element.HasJsonPropertyAttribute())
            {
                var name = element.GetCustomAttribute<JsonPropertyAttribute>().PropertyName ?? namingStrategy.GetPropertyName(element.Name, hasSpecifiedName: false);

                return name;

            }

            return namingStrategy.GetPropertyName(element.Name, hasSpecifiedName: false);
        }
    }
}
