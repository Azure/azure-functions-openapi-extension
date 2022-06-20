using System;
using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers
{
    /// <summary>
    /// This represents the resolver entity for <see cref="OpenApiExample"/>.
    /// </summary>
    public static class OpenApiExampleResolver
    {
        private static JsonSerializerSettings settings = new JsonSerializerSettings();

        /// <summary>
        /// Create a new instance of the <see cref="KeyValuePair{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="name">Name of the example</param>
        /// <param name="instance">Example object.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance.</param>
        /// <typeparam name="T">Type of the example object.</typeparam>
        /// <returns>Returns the new instance of the <see cref="KeyValuePair{TKey, TValue}"/> class. </returns>
        public static KeyValuePair<string, OpenApiExample> Resolve<T>(string name, T instance, NamingStrategy namingStrategy = null)
        {
            return Resolve(name, null, instance, namingStrategy);
        }

        /// <summary>
        /// Create a new instance of the <see cref="KeyValuePair{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="name">Name of the example</param>
        /// <param name="summary">Summary of the example</param>
        /// <param name="instance">Example object.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance.</param>
        /// <typeparam name="T">Type of the example object.</typeparam>
        /// <returns>Returns the new instance of the <see cref="KeyValuePair{TKey, TValue}"/> class. </returns>
        public static KeyValuePair<string, OpenApiExample> Resolve<T>(string name, string summary, T instance, NamingStrategy namingStrategy = null)
        {
            return Resolve(name, summary, null, instance, namingStrategy);
        }

        /// <summary>
        /// Create a new instance of the <see cref="KeyValuePair{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="name">Name of the example</param>
        /// <param name="summary">Summary of the example</param>
        /// <param name="description">Description of the example</param>
        /// <param name="instance">Example object.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance.</param>
        /// <typeparam name="T">Type of the example object.</typeparam>
        /// <returns>Returns the new instance of the <see cref="KeyValuePair{TKey, TValue}"/> class. </returns>
        public static KeyValuePair<string, OpenApiExample> Resolve<T>(string name, string summary, string description, T instance, NamingStrategy namingStrategy = null)
        {
            name.ThrowIfNullOrWhiteSpace();
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            var resolver = new DefaultContractResolver() { NamingStrategy = namingStrategy ?? new DefaultNamingStrategy() };
            settings.ContractResolver = resolver;

            var openApiExampleValue = OpenApiExampleFactory.CreateInstance<T>(instance,settings, namingStrategy);
            var example = new OpenApiExample()
            {
                Summary = summary,
                Description = description,
                Value = openApiExampleValue,
            };
            var kvp = new KeyValuePair<string, OpenApiExample>(name, example);

            return kvp;
        }
    }
}
