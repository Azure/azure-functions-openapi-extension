using System;
using System.Reflection;
using System.Runtime.Serialization;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="MemberInfo"/>.
    /// </summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// Checks whether the custom attribute exists or not.
        /// </summary>
        /// <typeparam name="T">Type of custom attribute.</typeparam>
        /// <param name="element"><see cref="MemberInfo"/> instance.</param>
        /// <param name="inherit">Value indicating whether to inspect ancestors or not. Default is <c>false</c>.</param>
        /// <returns><c>true</c>, if custom attribute exists; otherwise returns <c>false</c>.</returns>
        public static bool ExistsCustomAttribute<T>(this MemberInfo element, bool inherit = false) where T : Attribute
        {
            element.ThrowIfNullOrDefault();

            var exists = element.GetCustomAttribute<T>(inherit) != null;

            return exists;
        }

        /// <summary>
        /// Gets the display name of the enum value.
        /// </summary>
        /// <param name="element"><see cref="MemberInfo"/> instance.</param>
        /// 
        /// <returns>Display name of the enum value.</returns>
        public static string ToDisplayName(this MemberInfo element)
        {
            _ = element.ThrowIfNullOrDefault();

            var displayAttribute = element.GetCustomAttribute<DisplayAttribute>(inherit: false);
            var enumMemberAttribute = element.GetCustomAttribute<EnumMemberAttribute>(inherit: false);

            // EnumMemberAttribute takes precedence to DisplayAttribute
            return !enumMemberAttribute.IsNullOrDefault()
                       ? enumMemberAttribute.Value
                       : (!displayAttribute.IsNullOrDefault()
                          ? displayAttribute.Name
                          : element.Name);
        }
    }
}
