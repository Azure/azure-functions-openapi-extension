using System.Linq;
using System.Reflection;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="HttpTriggerAttribute"/>.
    /// </summary>
    public static class HttpTriggerAttributeExtensions
    {
        /// <summary>
        /// Gets the <see cref="HttpTriggerAttribute"/> instance.
        /// </summary>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <returns><see cref="HttpTriggerAttribute"/> instance.</returns>
        public static HttpTriggerAttribute GetHttpTrigger(this MethodInfo element)
        {
            element.ThrowIfNullOrDefault();

            var trigger = element.GetParameters()
                                 .First()
                                 .GetCustomAttribute<HttpTriggerAttribute>(inherit: false);

            return trigger;
        }
    }
}
