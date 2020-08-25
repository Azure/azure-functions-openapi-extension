using System.Collections.Generic;

using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="OpenApiResponses"/>.
    /// </summary>
    public static class OpenApiResponsesExtensions
    {
        /// <summary>
        /// Converts dictionary of <see cref="OpenApiResponse"/> instances to <see cref="OpenApiResponses"/>.
        /// </summary>
        /// <param name="collection">Dictionary of <see cref="OpenApiResponse"/> instances.</param>
        /// <returns><see cref="OpenApiResponses"/> instance.</returns>
        public static OpenApiResponses ToOpenApiResponses(this Dictionary<string, OpenApiResponse> collection)
        {
            collection.ThrowIfNullOrDefault();

            var responses = new OpenApiResponses();
            foreach (var item in collection)
            {
                responses[item.Key] = item.Value;
            }

            return responses;
        }
    }
}
