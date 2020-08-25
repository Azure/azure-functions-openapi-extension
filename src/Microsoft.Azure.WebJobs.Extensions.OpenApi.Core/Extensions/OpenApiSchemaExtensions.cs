using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="OpenApiSchema"/>.
    /// </summary>
    [Obsolete("This extension class is now obsolete", error: true)]
    public static class OpenApiSchemaExtensions
    {
        /// <summary>
        /// Converts <see cref="Type"/> to <see cref="OpenApiSchema"/>.
        /// </summary>
        /// <param name="type"><see cref="Type"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance to create the JSON schema from .NET Types.</param>
        /// <param name="attribute"><see cref="OpenApiSchemaVisibilityAttribute"/> instance. Default is <c>null</c>.</param>
        /// <returns><see cref="OpenApiSchema"/> instance.</returns>
        /// <remarks>
        /// It runs recursively to build the entire object type. It only takes properties without <see cref="JsonIgnoreAttribute"/>.
        /// </remarks>
        [Obsolete("This method is now obsolete", error: true)]
        public static OpenApiSchema ToOpenApiSchema(this Type type, NamingStrategy namingStrategy, OpenApiSchemaVisibilityAttribute attribute = null)
        {
            return ToOpenApiSchemas(type, namingStrategy, attribute, true).Single().Value;
        }

        /// <summary>
        /// Converts <see cref="Type"/> to <see cref="OpenApiSchema"/>.
        /// </summary>
        /// <param name="type"><see cref="Type"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance to create the JSON schema from .NET Types.</param>
        /// <param name="attribute"><see cref="OpenApiSchemaVisibilityAttribute"/> instance. Default is <c>null</c>.</param>
        /// <param name="returnSingleSchema">Value indicating whether to return single schema or not.</param>
        /// <param name="depth">Recurring depth.</param>
        /// <returns>Returns <see cref="Dictionary{String, OpenApiSchema}"/> instance.</returns>
        [Obsolete("This method is now obsolete", error: true)]
        public static Dictionary<string, OpenApiSchema> ToOpenApiSchemas(this Type type, NamingStrategy namingStrategy, OpenApiSchemaVisibilityAttribute attribute = null, bool returnSingleSchema = false, int depth = 0)
        {
            type.ThrowIfNullOrDefault();

            var schema = (OpenApiSchema)null;
            var schemeName = type.GetOpenApiTypeName(namingStrategy);

            if (depth == 8)
            {
                schema = new OpenApiSchema()
                {
                    Type = type.ToDataType(),
                    Format = type.ToDataFormat()
                };

                return new Dictionary<string, OpenApiSchema>() { { schemeName, schema } };
            }

            depth++;

            if (type.IsJObjectType())
            {
                schema = typeof(object).ToOpenApiSchemas(namingStrategy, null, true, depth).Single().Value;

                return new Dictionary<string, OpenApiSchema>() { { schemeName, schema } };
            }

            if (type.IsOpenApiNullable(out var unwrappedValueType))
            {
                schema = unwrappedValueType.ToOpenApiSchemas(namingStrategy, null, true, depth).Single().Value;
                schema.Nullable = true;

                return new Dictionary<string, OpenApiSchema>() { { schemeName, schema } };
            }

            schema = new OpenApiSchema()
            {
                Type = type.ToDataType(),
                Format = type.ToDataFormat()
            };

            if (!attribute.IsNullOrDefault())
            {
                var visibility = new OpenApiString(attribute.Visibility.ToDisplayName());

                schema.Extensions.Add("x-ms-visibility", visibility);
            }

            if (type.IsUnflaggedEnumType())
            {
                var converterAttribute = type.GetCustomAttribute<JsonConverterAttribute>();
                if (!converterAttribute.IsNullOrDefault()
                    && typeof(StringEnumConverter).IsAssignableFrom(converterAttribute.ConverterType))
                {
                    var enums = type.ToOpenApiStringCollection(namingStrategy);

                    schema.Type = "string";
                    schema.Format = null;
                    schema.Enum = enums;
                    schema.Default = enums.First();
                }
                else
                {
                    var enums = type.ToOpenApiIntegerCollection();

                    schema.Enum = enums;
                    schema.Default = enums.First();
                }
            }

            if (type.IsSimpleType())
            {
                return new Dictionary<string, OpenApiSchema>() { { schemeName, schema } };
            }

            if (type.IsOpenApiDictionary())
            {
                schema.AdditionalProperties = type.GetGenericArguments()[1].ToOpenApiSchemas(namingStrategy, null, true, depth).Single().Value;

                return new Dictionary<string, OpenApiSchema>() { { schemeName, schema } };
            }

            if (type.IsOpenApiArray())
            {
                schema.Type = "array";
                schema.Items = (type.GetElementType() ?? type.GetGenericArguments()[0]).ToOpenApiSchemas(namingStrategy, null, true, depth).Single().Value;

                return new Dictionary<string, OpenApiSchema>() { { schemeName, schema } };
            }

            var allProperties = type.IsInterface
                                    ? new[] { type }.Concat(type.GetInterfaces()).SelectMany(i => i.GetProperties())
                                    : type.GetProperties();
            var properties = allProperties.Where(p => !p.ExistsCustomAttribute<JsonIgnoreAttribute>());

            var retVal = new Dictionary<string, OpenApiSchema>();
            foreach (var property in properties)
            {
                var visiblity = property.GetCustomAttribute<OpenApiSchemaVisibilityAttribute>(inherit: false);
                var propertyName = property.GetJsonPropertyName(namingStrategy);

                var ts = property.DeclaringType.GetGenericArguments();
                if (!ts.Any())
                {
                    if (property.PropertyType.IsUnflaggedEnumType() && !returnSingleSchema)
                    {
                        var recur1 = property.PropertyType.ToOpenApiSchemas(namingStrategy, visiblity, false, depth);
                        retVal.AddRange(recur1);

                        var enumReference = new OpenApiReference()
                        {
                            Type = ReferenceType.Schema,
                            Id = property.PropertyType.GetOpenApiReferenceId(false, false)
                        };

                        var schema1 = new OpenApiSchema() { Reference = enumReference };
                        schema.Properties[namingStrategy.GetPropertyName(propertyName, false)] = schema1;
                    }
                    else if (property.PropertyType.IsSimpleType() || Nullable.GetUnderlyingType(property.PropertyType) != null || returnSingleSchema)
                    {
                        schema.Properties[namingStrategy.GetPropertyName(propertyName, false)] = property.PropertyType.ToOpenApiSchemas(namingStrategy, visiblity, true, depth).Single().Value;
                    }
                    else if (property.PropertyType.IsOpenApiDictionary())
                    {
                        var elementType = property.PropertyType.GetGenericArguments()[1];
                        if (elementType.IsSimpleType() || elementType.IsOpenApiDictionary() || elementType.IsOpenApiArray())
                        {
                            schema.Properties[namingStrategy.GetPropertyName(propertyName, false)] = property.PropertyType.ToOpenApiSchemas(namingStrategy, visiblity, true, depth).Single().Value;
                        }
                        else
                        {
                            var recur1 = elementType.ToOpenApiSchemas(namingStrategy, visiblity, false, depth);
                            retVal.AddRange(recur1);

                            var elementReference = new OpenApiReference()
                            {
                                Type = ReferenceType.Schema,
                                Id = elementType.GetOpenApiReferenceId(false, false)
                            };

                            var dictionarySchema = new OpenApiSchema()
                            {
                                Type = "object",
                                AdditionalProperties = new OpenApiSchema() { Reference = elementReference }
                            };

                            schema.Properties[namingStrategy.GetPropertyName(propertyName, false)] = dictionarySchema;
                        }
                    }
                    else if (property.PropertyType.IsOpenApiArray())
                    {
                        var elementType = property.PropertyType.GetElementType() ?? property.PropertyType.GetGenericArguments()[0];
                        if (elementType.IsSimpleType() || elementType.IsOpenApiDictionary() || elementType.IsOpenApiArray())
                        {
                            schema.Properties[namingStrategy.GetPropertyName(propertyName, false)] = property.PropertyType.ToOpenApiSchemas(namingStrategy, visiblity, true, depth).Single().Value;
                        }
                        else
                        {
                            var elementReference = elementType.ToOpenApiSchemas(namingStrategy, visiblity, false, depth);
                            retVal.AddRange(elementReference);

                            var reference1 = new OpenApiReference()
                            {
                                Type = ReferenceType.Schema,
                                Id = elementType.GetOpenApiReferenceId(false, false)
                            };
                            var arraySchema = new OpenApiSchema()
                            {
                                Type = "array",
                                Items = new OpenApiSchema()
                                {
                                    Reference = reference1
                                }
                            };

                            schema.Properties[namingStrategy.GetPropertyName(propertyName, false)] = arraySchema;
                        }

                    }
                    else
                    {
                        var recur1 = property.PropertyType.ToOpenApiSchemas(namingStrategy, visiblity, false, depth);
                        retVal.AddRange(recur1);

                        var reference1 = new OpenApiReference()
                        {
                            Type = ReferenceType.Schema,
                            Id = property.PropertyType.GetOpenApiReferenceId(false, false)
                        };

                        var schema1 = new OpenApiSchema() { Reference = reference1 };

                        schema.Properties[namingStrategy.GetPropertyName(propertyName, false)] = schema1;
                    }

                    continue;
                }

                var reference = new OpenApiReference()
                {
                    Type = ReferenceType.Schema,
                    Id = property.PropertyType.GetOpenApiRootReferenceId()
                };

                var referenceSchema = new OpenApiSchema() { Reference = reference };

                if (!ts.Contains(property.PropertyType))
                {
                    if (property.PropertyType.IsOpenApiDictionary())
                    {
                        reference.Id = property.PropertyType.GetOpenApiReferenceId(true, false);
                        var dictionarySchema = new OpenApiSchema()
                        {
                            Type = "object",
                            AdditionalProperties = referenceSchema
                        };

                        schema.Properties[namingStrategy.GetPropertyName(propertyName, false)] = dictionarySchema;

                        continue;
                    }

                    if (property.PropertyType.IsOpenApiArray())
                    {
                        reference.Id = property.PropertyType.GetOpenApiReferenceId(false, true);
                        var arraySchema = new OpenApiSchema()
                        {
                            Type = "array",
                            Items = referenceSchema
                        };

                        schema.Properties[namingStrategy.GetPropertyName(propertyName, false)] = arraySchema;

                        continue;
                    }

                    schema.Properties[namingStrategy.GetPropertyName(propertyName, false)] = property.PropertyType.ToOpenApiSchemas(namingStrategy, visiblity, true, depth).Single().Value;

                    continue;
                }

                schema.Properties[namingStrategy.GetPropertyName(propertyName, false)] = referenceSchema;
            }

            retVal[schemeName] = schema;

            return retVal;
        }
    }
}
