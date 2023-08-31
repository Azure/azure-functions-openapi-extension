using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="OpenApiPayloadAttribute"/>.
    /// </summary>
    public static class OpenApiPayloadAttributeExtensions
    {
        /// <summary>
        /// Converts <see cref="OpenApiPayloadAttribute"/> to <see cref="OpenApiMediaType"/>.
        /// </summary>
        /// <typeparam name="T">Type of payload attribute inheriting <see cref="OpenApiPayloadAttribute"/>.</typeparam>
        /// <param name="attribute">OpenApi payload attribute.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance to create the JSON schema from .NET Types.</param>
        /// <param name="collection"><see cref="VisitorCollection"/> instance.</param>
        /// <param name="version">OpenAPI spec version.</param>
        /// <param name="useFullName">Value indicating whether to use Fullname or not</param>
        /// <returns><see cref="OpenApiMediaType"/> instance.</returns>
        public static OpenApiMediaType ToOpenApiMediaType<T>(this T attribute, NamingStrategy namingStrategy = null, VisitorCollection collection = null, OpenApiVersionType version = OpenApiVersionType.V2 , bool useFullName = default) where T : OpenApiPayloadAttribute
        {
            attribute.ThrowIfNullOrDefault();

            if (namingStrategy.IsNullOrDefault())
            {
                namingStrategy = new DefaultNamingStrategy();
            }

            if (collection.IsNullOrDefault())
            {
                collection = VisitorCollection.CreateInstance();
            }

            var type = attribute.BodyType;

            // Generate schema based on the type.
            var schema = collection.PayloadVisit(type, namingStrategy, useFullName);

            // Add deprecated attribute.
            if (attribute is OpenApiRequestBodyAttribute)
            {
                schema.Deprecated = (attribute as OpenApiRequestBodyAttribute).Deprecated;
            }
            if (attribute is OpenApiResponseWithBodyAttribute)
            {
                schema.Deprecated = (attribute as OpenApiResponseWithBodyAttribute).Deprecated;
            }

            // For array and dictionary object, the reference has already been added by the visitor.
            if (type.IsReferentialType() && !type.IsOpenApiNullable() && !type.IsOpenApiArray() && !type.IsOpenApiDictionary())
            {
                var reference = new OpenApiReference()
                {
                    Type = ReferenceType.Schema,
                    Id = attribute.BodyType.GetOpenApiReferenceId(isDictionary: false, isList: false, namingStrategy, useFullName)
                };

                schema.Reference = reference;
            }

            var mediaType = new OpenApiMediaType() { Schema = schema };

            if (attribute.Example.IsNullOrDefault())
            {
                return mediaType;
            }

            if (!attribute.Example.HasInterface("IOpenApiExample`1"))
            {
                return mediaType;
            }

            var example = (dynamic)Activator.CreateInstance(attribute.Example);
            var examples = (IDictionary<string, OpenApiExample>)example.Build(namingStrategy).Examples;

            mediaType.Examples = examples;
            if (version == OpenApiVersionType.V2)
            {
                mediaType.Example = examples.First().Value.Value;
            }

            return mediaType;
        }
    }
}
