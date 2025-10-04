using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors
{
    /// <summary>
    /// This represents the type visitor for <see cref="long"/> type enum.
    /// </summary>
    public class Int64EnumTypeVisitor : TypeVisitor
    {
        /// <inheritdoc />
        public Int64EnumTypeVisitor(VisitorCollection visitorCollection)
            : base(visitorCollection)
        {
        }

        /// <inheritdoc />
        public override bool IsVisitable(Type type)
        {
            var isVisitable = this.IsVisitable(type, TypeCode.Int64) &&
                              type.IsUnflaggedEnumType() &&
                              !type.HasJsonConverterAttribute<StringEnumConverter, JsonStringEnumConverter>() &&
                              Enum.GetUnderlyingType(type) == typeof(long)
                              ;

            return isVisitable;
        }

        /// <inheritdoc />
        public override void Visit(IAcceptor acceptor, KeyValuePair<string, Type> type, NamingStrategy namingStrategy, params Attribute[] attributes)
        {
            var name = type.Key;

            var instance = acceptor as OpenApiSchemaAcceptor;
            if (instance.IsNullOrDefault())
            {
                return;
            }

            // Adds enum values to the schema.
            var enums = type.Value.ToOpenApiInt64Collection();

            var schema = new OpenApiSchema()
            {
                Type = "integer",
                Format = "int64",
                Enum = enums,
                Default = enums.First()
            };

            // Adds the extra properties.
            if (attributes.Any())
            {
                Attribute attr = attributes.OfType<OpenApiPropertyAttribute>().SingleOrDefault();
                if (!attr.IsNullOrDefault())
                {
                    schema.Nullable = this.GetOpenApiPropertyNullable(attr as OpenApiPropertyAttribute);
                    schema.Default = this.GetOpenApiPropertyDefault<long>(attr as OpenApiPropertyAttribute);
                    schema.Description = this.GetOpenApiPropertyDescription(attr as OpenApiPropertyAttribute);
                    schema.Deprecated = this.GetOpenApiPropertyDeprecated(attr as OpenApiPropertyAttribute);
                }

                attr = attributes.OfType<OpenApiSchemaVisibilityAttribute>().SingleOrDefault();
                if (!attr.IsNullOrDefault())
                {
                    var extension = new OpenApiString((attr as OpenApiSchemaVisibilityAttribute).Visibility.ToDisplayName());

                    schema.Extensions.Add("x-ms-visibility", extension);
                }
            }

            instance.Schemas.Add(name, schema);
        }

        /// <inheritdoc />
        public override bool IsParameterVisitable(Type type)
        {
            var isVisitable = this.IsVisitable(type);

            return isVisitable;
        }

        /// <inheritdoc />
        public override OpenApiSchema ParameterVisit(Type type, NamingStrategy namingStrategy)
        {
            var schema = this.ParameterVisit(dataType: "integer", dataFormat: "int64");

            // Adds enum values to the schema.
            var enums = type.ToOpenApiInt64Collection();

            schema.Enum = enums;
            schema.Default = enums.First();

            return schema;
        }

        /// <inheritdoc />
        public override bool IsPayloadVisitable(Type type)
        {
            var isVisitable = this.IsVisitable(type);

            return isVisitable;
        }

        /// <inheritdoc />
        public override OpenApiSchema PayloadVisit(Type type, NamingStrategy namingStrategy)
        {
            var schema = this.PayloadVisit(dataType: "integer", dataFormat: "int64");

            // Adds enum values to the schema.
            var enums = type.ToOpenApiInt64Collection();

            schema.Enum = enums;
            schema.Default = enums.First();

            return schema;
        }
    }
}
