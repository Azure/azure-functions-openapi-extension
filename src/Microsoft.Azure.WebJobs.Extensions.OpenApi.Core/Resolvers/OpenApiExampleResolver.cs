using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers
{
    /// <summary>
    /// This represents the resolver entity for <see cref="OpenApiExample"/>.
    /// </summary>
    public static class OpenApiExampleResolver
    {
        /// <summary>
        /// Create a new instance of the <see cref="KeyValuePair{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="name">Name of the example</param>
        /// <param name="instance">Example object.</param>
        /// <typeparam name="T">Type of the example object.</typeparam>
        /// <returns>Returns the new instance of the <see cref="KeyValuePair{TKey, TValue}"/> class. </returns>
        public static KeyValuePair<string, OpenApiExample> Resolve<T>(string name, T instance)
        {
            return Resolve(name, null, instance);
        }

        /// <summary>
        /// Create a new instance of the <see cref="KeyValuePair{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="name">Name of the example</param>
        /// <param name="summary">Summary of the example</param>
        /// <param name="instance">Example object.</param>
        /// <typeparam name="T">Type of the example object.</typeparam>
        /// <returns>Returns the new instance of the <see cref="KeyValuePair{TKey, TValue}"/> class. </returns>
        public static KeyValuePair<string, OpenApiExample> Resolve<T>(string name, string summary, T instance)
        {
            return Resolve(name, summary, null, instance);
        }

        /// <summary>
        /// Create a new instance of the <see cref="KeyValuePair{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="name">Name of the example</param>
        /// <param name="summary">Summary of the example</param>
        /// <param name="description">Description of the example</param>
        /// <param name="instance">Example object.</param>
        /// <typeparam name="T">Type of the example object.</typeparam>
        /// <returns>Returns the new instance of the <see cref="KeyValuePair{TKey, TValue}"/> class. </returns>
        public static KeyValuePair<string, OpenApiExample> Resolve<T>(string name, string summary, string description, T instance)
        {
            name.ThrowIfNullOrWhiteSpace();
            instance.ThrowIfNullOrDefault();

            var example = new OpenApiExample()
            {
                Summary = summary,
                Description = description,
                Value = new OpenApiString(JsonConvert.SerializeObject(instance)),
            };
            var kvp = new KeyValuePair<string, OpenApiExample>(name, example);

            return kvp;
        }
    }
}
