using System.Collections.Generic;

using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// This provides interfaces to classes to render example data.
    /// </summary>
    /// <typeparam name="T">Type of the example object.</typeparam>
    public interface IExample<T>
    {
        /// <summary>
        /// Gets the collection of the <see cref="OpenApiExample"/> objects.
        /// </summary>
        /// <returns>Returns thecollection of the <see cref="OpenApiExample"/> objects.</returns>
        Dictionary<string, OpenApiExample> GetExamples();
    }
}
