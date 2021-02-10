using System;
using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="OpenApiResponseWithBodyAttribute"/>.
    /// </summary>
    public static class OpenApiResponseWithBodyAttributeExtensions
    {
        /// <summary>
        /// Converts <see cref="OpenApiResponseWithBodyAttribute"/> to <see cref="OpenApiResponse"/>.
        /// </summary>
        /// <param name="attribute"><see cref="OpenApiResponseWithBodyAttribute"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance to create the JSON schema from .NET Types.</param>
        /// <param name="collection"><see cref="VisitorCollection"/> instance.</param>
        /// <returns><see cref="OpenApiResponse"/> instance.</returns>
        public static OpenApiResponse ToOpenApiResponse(this OpenApiResponseWithBodyAttribute attribute, NamingStrategy namingStrategy = null, VisitorCollection collection = null)
        {
            attribute.ThrowIfNullOrDefault();

            var description = string.IsNullOrWhiteSpace(attribute.Description)
                                  ? $"Payload of {attribute.BodyType.GetOpenApiDescription()}"
                                  : attribute.Description;
            var mediaType = attribute.ToOpenApiMediaType<OpenApiResponseWithBodyAttribute>(namingStrategy, collection);
            var content = new Dictionary<string, OpenApiMediaType>()
                              {
                                  { attribute.ContentType, mediaType }
                              };
            var response = new OpenApiResponse()
            {
                Description = description,
                Content = content,
            };

            if (attribute.HeaderType.HasInterface<IOpenApiResponseHeaderType>())
            {
                var header = Activator.CreateInstance(attribute.HeaderType) as IOpenApiResponseHeaderType;

                response.Headers = header.Headers;
            }

            if (!string.IsNullOrWhiteSpace(attribute.Summary))
            {
                var summary = new OpenApiString(attribute.Summary);

                response.Extensions.Add("x-ms-summary", summary);
            }

            return response;
        }
    }
}
