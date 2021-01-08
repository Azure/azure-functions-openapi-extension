using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
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
        /// <returns><see cref="OpenApiMediaType"/> instance.</returns>
        public static OpenApiMediaType ToOpenApiMediaType<T>(this T attribute, NamingStrategy namingStrategy = null, VisitorCollection collection = null) where T : OpenApiPayloadAttribute
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
            var schema = collection.PayloadVisit(type, namingStrategy);

            // For array and dictionary object, the reference has already been added by the visitor.
            if (type.IsReferentialType() && !type.IsOpenApiNullable() && !type.IsOpenApiArray() && !type.IsOpenApiDictionary())
            {
                var reference = new OpenApiReference()
                {
                    Type = ReferenceType.Schema,
                    Id = attribute.BodyType.GetOpenApiReferenceId(isDictionary: false, isList: false, namingStrategy)
                };

                schema.Reference = reference;
            }

            var mediaType = new OpenApiMediaType() { Schema = schema };

            return mediaType;
        }
    }
}
