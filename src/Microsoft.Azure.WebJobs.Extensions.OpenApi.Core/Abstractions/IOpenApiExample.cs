using System.Collections.Generic;

using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// This provides interfaces to classes to render example data.
    /// </summary>
    /// <typeparam name="T">Type of the example object.</typeparam>
    public interface IOpenApiExample<T>
    {
        /// <summary>
        /// Gets the collection of the <see cref="OpenApiExample"/> objects.
        /// </summary>
        IDictionary<string, OpenApiExample> Examples { get; }

        /// <summary>
        /// Builds the example.
        /// </summary>
        /// <returns>Returns <see cref="IOpenApiExample{T}"/> instance.</returns>
        IOpenApiExample<T> Build();
    }
}
