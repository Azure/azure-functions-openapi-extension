using System.Reflection;
using System.Runtime.Serialization;

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
        /// Checks whether the given <see cref="PropertyInfo"/> object has <see cref="DataMemberAttribute"/> or not.
        /// </summary>
        /// <param name="element"><see cref="PropertyInfo"/> instance.</param>
        /// <returns>Returns <c>True</c>, if the given <see cref="PropertyInfo"/> object has <see cref="DataMemberAttribute"/>; otherwise, returns <c>False</c>.</returns>
        public static bool HasDataMemberAttribute(this PropertyInfo element)
        {
            var exists = element.ExistsCustomAttribute<DataMemberAttribute>();

            return exists;
        }

        /// <summary>
        /// Gets the name from <see cref="JsonPropertyAttribute"/> or <see cref="DataMemberAttribute"/> instance.
        /// </summary>
        /// <param name="element"><see cref="PropertyInfo"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance.</param>
        /// <returns>Returns the name from <see cref="JsonPropertyAttribute"/> or <see cref="DataMemberAttribute"/> instance. If both <see cref="JsonPropertyAttribute"/> and <see cref="DataMemberAttribute"/> are declared, <see cref="JsonPropertyAttribute"/> takes precedence.</returns>
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

            if (element.HasDataMemberAttribute())
            {
                var name = element.GetCustomAttribute<DataMemberAttribute>().Name ?? namingStrategy.GetPropertyName(element.Name, hasSpecifiedName: false);

                return name;

            }

            return namingStrategy.GetPropertyName(element.Name, hasSpecifiedName: false);
        }
    }
}
