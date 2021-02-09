using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="OpenApiParameterAttribute"/>.
    /// </summary>
    public static class OpenApiParameterAttributeExtensions
    {
        /// <summary>
        /// Converts <see cref="OpenApiParameterAttribute"/> to <see cref="OpenApiParameter"/>.
        /// </summary>
        /// <param name="attribute"><see cref="OpenApiParameterAttribute"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance.</param>
        /// <param name="collection"><see cref="VisitorCollection"/> instance.</param>
        /// <returns><see cref="OpenApiParameter"/> instance.</returns>
        public static OpenApiParameter ToOpenApiParameter(this OpenApiParameterAttribute attribute, NamingStrategy namingStrategy = null, VisitorCollection collection = null)
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

            var type = attribute.Type;

            var schema = collection.ParameterVisit(type, namingStrategy);

            var parameter = new OpenApiParameter()
            {
                Name = attribute.Name,
                Description = attribute.Description,
                Required = attribute.Required,
                Deprecated = attribute.Deprecated,
                In = attribute.In,
                Schema = schema
            };

            if (type.IsOpenApiArray())
            {
                if (attribute.In == ParameterLocation.Path)
                {
                    parameter.Style = ParameterStyle.Simple;
                    parameter.Explode = false;
                }

                if (attribute.In == ParameterLocation.Query)
                {
                    parameter.Style = attribute.CollectionDelimiter == OpenApiParameterCollectionDelimiterType.Comma
                                      ? ParameterStyle.Form
                                      : (attribute.CollectionDelimiter == OpenApiParameterCollectionDelimiterType.Space
                                         ? ParameterStyle.SpaceDelimited
                                         : ParameterStyle.PipeDelimited);
                    parameter.Explode = attribute.CollectionDelimiter == OpenApiParameterCollectionDelimiterType.Comma
                                        ? attribute.Explode
                                        : false;
                }
            }

            if (!string.IsNullOrWhiteSpace(attribute.Summary))
            {
                var summary = new OpenApiString(attribute.Summary);

                parameter.Extensions.Add("x-ms-summary", summary);
            }

            if (attribute.Visibility != OpenApiVisibilityType.Undefined)
            {
                var visibility = new OpenApiString(attribute.Visibility.ToDisplayName());

                parameter.Extensions.Add("x-ms-visibility", visibility);
            }

            return parameter;
        }
    }
}
