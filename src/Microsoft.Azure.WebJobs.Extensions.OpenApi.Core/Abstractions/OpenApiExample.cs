using System.Collections.Generic;

using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// This represents the example entity for models.
    /// </summary>
    /// <typeparam name="T">Type of model as an example.</typeparam>
    public abstract class OpenApiExample<T> : IOpenApiExample<T>
    {
        /// <inheritdoc />
        public IDictionary<string, OpenApiExample> Examples { get; } = new Dictionary<string, OpenApiExample>();

        /// <inheritdoc />
        public abstract IOpenApiExample<T> Build(NamingStrategy namingStrategy = null);
    }
}
