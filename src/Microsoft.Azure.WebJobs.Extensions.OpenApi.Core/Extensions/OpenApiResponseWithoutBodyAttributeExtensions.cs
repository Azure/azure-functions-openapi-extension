using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="OpenApiResponseWithoutBodyAttribute"/>.
    /// </summary>
    public static class OpenApiResponseWithoutBodyAttributeExtensions
    {
        /// <summary>
        /// Converts <see cref="OpenApiResponseWithoutBodyAttribute"/> to <see cref="OpenApiResponse"/>.
        /// </summary>
        /// <param name="attribute"><see cref="OpenApiResponseWithoutBodyAttribute"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance to create the JSON schema from .NET Types.</param>
        /// <returns><see cref="OpenApiResponse"/> instance.</returns>
        public static OpenApiResponse ToOpenApiResponse(this OpenApiResponseWithoutBodyAttribute attribute, NamingStrategy namingStrategy = null)
        {
            attribute.ThrowIfNullOrDefault();

            var description = string.IsNullOrWhiteSpace(attribute.Description)
                                  ? "No description"
                                  : attribute.Description;
            var response = new OpenApiResponse()
            {
                Description = description,
            };

            if (!string.IsNullOrWhiteSpace(attribute.Summary))
            {
                var summary = new OpenApiString(attribute.Summary);

                response.Extensions.Add("x-ms-summary", summary);
            }

            return response;
        }
    }
}
