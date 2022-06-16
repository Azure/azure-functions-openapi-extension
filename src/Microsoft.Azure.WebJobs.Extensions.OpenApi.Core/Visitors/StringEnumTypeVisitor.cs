using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors
{
    /// <summary>
    /// This represents the type visitor for <see cref="string"/> type enum.
    /// </summary>
    public class StringEnumTypeVisitor : TypeVisitor
    {
        /// <inheritdoc />
        public StringEnumTypeVisitor(VisitorCollection visitorCollection)
            : base(visitorCollection)
        {
        }

        /// <inheritdoc />
        public override bool IsVisitable(Type type)
        {
            var isVisitable = (this.IsVisitable(type, TypeCode.Int16) || this.IsVisitable(type, TypeCode.Int32) || this.IsVisitable(type, TypeCode.Int64) || this.IsVisitable(type, TypeCode.Byte)) &&
                              type.IsUnflaggedEnumType() &&
                              type.HasJsonConverterAttribute<StringEnumConverter>()
                              ;

            return isVisitable;
        }
        private void EnsureRootRefSchema(OpenApiSchemaAcceptor acceptor, Type type, NamingStrategy namingStrategy)
        {
            var title = namingStrategy.GetPropertyName(type.Name, hasSpecifiedName: false);
            if (!acceptor.RootSchemas.ContainsKey(title))
            {
                // Adds enum values to the schema.
                var enums = type.ToOpenApiStringCollection();
                var refSchema = new OpenApiSchema()
                {
                    Type = "string",
                    Format = null,
                    Enum = enums,
                    Default = enums.First()
                };
                var descriptions = type.GetCustomAttributes(false).OfType<DescriptionAttribute>().Select(d => d.Description).ToList();
                foreach (var enumValue in type.GetEnumValues())
                {
                    descriptions.AddRange(type.GetMember(enumValue.ToString()).SelectMany(t => t.GetCustomAttributes(false)).OfType<DescriptionAttribute>().Select(d => $"{enumValue}: {d.Description}"));
                }
                if (descriptions.Any())
                    refSchema.Description = string.Join("\n", descriptions);
                acceptor.RootSchemas.Add(title, refSchema);
            }
        }

        /// <inheritdoc />
        public override void Visit(IAcceptor acceptor, KeyValuePair<string, Type> type, NamingStrategy namingStrategy, params Attribute[] attributes)
        {
            var name = type.Key;
            var title = namingStrategy.GetPropertyName(type.Value.Name, hasSpecifiedName: false);

            var instance = acceptor as OpenApiSchemaAcceptor;
            if (instance.IsNullOrDefault())
            {
                return;
            }
            if (!instance.RootSchemas.ContainsKey(title))
            {
                // Adds enum values to the schema.
                var enums = type.Value.ToOpenApiStringCollection();
                var refSchema = new OpenApiSchema()
                {
                    Type = "string",
                    Format = null,
                    Enum = enums,
                    Default = enums.First()
                };
                var descriptions = type.Value.GetCustomAttributes(false).OfType<DescriptionAttribute>().Select(d=>d.Description).ToList();
                foreach(var enumValue in type.Value.GetEnumValues())
                {
                    descriptions.AddRange(type.Value.GetMember(enumValue.ToString()).SelectMany(t => t.GetCustomAttributes(false)).OfType<DescriptionAttribute>().Select(d => $"{enumValue}: {d.Description}"));
                }
                if(descriptions.Any())
                    refSchema.Description = string.Join("\n", descriptions);
                instance.RootSchemas.Add(title, refSchema);
            }
            var schema = new OpenApiSchema
            {
               // AllOf = new List<OpenApiSchema> {
                 //   new OpenApiSchema{
                    Reference = new OpenApiReference()
                    {
                        Type = ReferenceType.Schema,
                        Id = type.Value.GetOpenApiReferenceId(isDictionary: false, isList: false, namingStrategy)
                    }
                   // },
                //}
            };

            // Adds the extra properties.
            if (attributes.Any())
            {
                Attribute attr = attributes.OfType<OpenApiPropertyAttribute>().SingleOrDefault();
                if (!attr.IsNullOrDefault())
                {
                    schema.Nullable = this.GetOpenApiPropertyNullable(attr as OpenApiPropertyAttribute);
                    schema.Default = this.GetOpenApiPropertyDefault<string>(attr as OpenApiPropertyAttribute);
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
            //TODO: oas v2 does not support reusable enums in url params. We need to know the version.
            var version = OpenApiVersionType.V2;
            if (version == OpenApiVersionType.V2)
            {
                var schema = this.ParameterVisit(dataType: "string", dataFormat: null);

                // Adds enum values to the schema.
                var enums = type.ToOpenApiStringCollection(namingStrategy);

                schema.Enum = enums;
                schema.Default = enums.First();

                return schema;
            }
            return new OpenApiSchema
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.Schema,
                    Id = type.GetOpenApiReferenceId(isDictionary: false, isList: false, namingStrategy)
                }
            };
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
            //TODO: oas v2 does not support reusable enums in Payload. We need to know the version.
            var version = OpenApiVersionType.V3;
            if (version == OpenApiVersionType.V2)
            {
                var schema = this.ParameterVisit(dataType: "string", dataFormat: null);

                // Adds enum values to the schema.
                var enums = type.ToOpenApiStringCollection(namingStrategy);

                schema.Enum = enums;
                schema.Default = enums.First();

                return schema;
            }
            return new OpenApiSchema
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.Schema,
                    Id = type.GetOpenApiReferenceId(isDictionary: false, isList: false, namingStrategy)
                }
            };
        }
    }
}
