using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using AnnotationsDataType = System.ComponentModel.DataAnnotations.DataType;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="OpenApiSchema"/>.
    /// </summary>
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
                var visibility = property.GetCustomAttribute<OpenApiSchemaVisibilityAttribute>(inherit: false);
                var propertyName = property.GetJsonPropertyName(namingStrategy);

                var ts = property.DeclaringType.GetGenericArguments();
                if (!ts.Any())
                {
                    if (property.PropertyType.IsUnflaggedEnumType() && !returnSingleSchema)
                    {
                        var recur1 = property.PropertyType.ToOpenApiSchemas(namingStrategy, visibility, false, depth);
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
                        schema.Properties[namingStrategy.GetPropertyName(propertyName, false)] = property.PropertyType.ToOpenApiSchemas(namingStrategy, visibility, true, depth).Single().Value;
                    }
                    else if (property.PropertyType.IsOpenApiDictionary())
                    {
                        var elementType = property.PropertyType.GetGenericArguments()[1];
                        if (elementType.IsSimpleType() || elementType.IsOpenApiDictionary() || elementType.IsOpenApiArray())
                        {
                            schema.Properties[namingStrategy.GetPropertyName(propertyName, false)] = property.PropertyType.ToOpenApiSchemas(namingStrategy, visibility, true, depth).Single().Value;
                        }
                        else
                        {
                            var recur1 = elementType.ToOpenApiSchemas(namingStrategy, visibility, false, depth);
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
                            schema.Properties[namingStrategy.GetPropertyName(propertyName, false)] = property.PropertyType.ToOpenApiSchemas(namingStrategy, visibility, true, depth).Single().Value;
                        }
                        else
                        {
                            var elementReference = elementType.ToOpenApiSchemas(namingStrategy, visibility, false, depth);
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
                        var recur1 = property.PropertyType.ToOpenApiSchemas(namingStrategy, visibility, false, depth);
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

                    schema.Properties[namingStrategy.GetPropertyName(propertyName, false)] = property.PropertyType.ToOpenApiSchemas(namingStrategy, visibility, true, depth).Single().Value;

                    continue;
                }

                schema.Properties[namingStrategy.GetPropertyName(propertyName, false)] = referenceSchema;
            }

            retVal[schemeName] = schema;

            return retVal;
        }

        /// <summary>
        /// Checks whether the OpenAPI schema is the object type or not.
        /// </summary>
        /// <param name="schema"><see cref="OpenApiSchema"/> instance.</param>
        /// <returns>Returns <c>True</c>, if the OpenAPI schema is the object type; otherwise, returns <c>False</c>.</returns>
        public static bool IsOpenApiSchemaObject(this OpenApiSchema schema)
        {
            if (schema.Type != "object")
            {
                return false;
            }

            if (!schema.Format.IsNullOrWhiteSpace())
            {
                return false;
            }

            if (!schema.Items.IsNullOrDefault())
            {
                return false;
            }

            if (!schema.AdditionalProperties.IsNullOrDefault())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks whether the OpenAPI schema is the array type or not.
        /// </summary>
        /// <param name="schema"><see cref="OpenApiSchema"/> instance.</param>
        /// <returns>Returns <c>True</c>, if the OpenAPI schema is the array type; otherwise, returns <c>False</c>.</returns>
        public static bool IsOpenApiSchemaArray(this OpenApiSchema schema)
        {
            if (schema.Type != "array")
            {
                return false;
            }

            if (!schema.Format.IsNullOrWhiteSpace())
            {
                return false;
            }

            if (!schema.Items.IsNullOrDefault())
            {
                return true;
            }

            if (!schema.AdditionalProperties.IsNullOrDefault())
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Checks whether the OpenAPI schema is the dictionary type or not.
        /// </summary>
        /// <param name="schema"><see cref="OpenApiSchema"/> instance.</param>
        /// <returns>Returns <c>True</c>, if the OpenAPI schema is the dictionary type; otherwise, returns <c>False</c>.</returns>
        public static bool IsOpenApiSchemaDictionary(this OpenApiSchema schema)
        {
            if (schema.Type != "object")
            {
                return false;
            }

            if (!schema.Format.IsNullOrWhiteSpace())
            {
                return false;
            }

            if (!schema.Items.IsNullOrDefault())
            {
                return false;
            }

            if (!schema.AdditionalProperties.IsNullOrDefault())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Applying <see cref="ValidationAttribute"/> to <see cref="OpenApiSchema"/>
        /// </summary>
        /// <param name="schema"><see cref="OpenApiSchema"/> instance.</param>
        /// <param name="customAttributes">DataAnnotation attributes</param>
        public static void ApplyValidationAttributes(this OpenApiSchema schema, IEnumerable<ValidationAttribute> customAttributes)
        {
            foreach (var attribute in customAttributes)
            {
                if (attribute is DataTypeAttribute dataTypeAttribute)
                {
                    ApplyDataTypeAttribute(schema, dataTypeAttribute);

                    continue;
                }

                if (attribute is MinLengthAttribute minLengthAttribute)
                {
                    ApplyMinLengthAttribute(schema, minLengthAttribute);

                    continue;
                }

                if (attribute is MaxLengthAttribute maxLengthAttribute)
                {
                    ApplyMaxLengthAttribute(schema, maxLengthAttribute);

                    continue;
                }

                if (attribute is RangeAttribute rangeAttribute)
                {
                    ApplyRangeAttribute(schema, rangeAttribute);

                    continue;
                }

                if (attribute is RegularExpressionAttribute regularExpressionAttribute)
                {
                    ApplyRegularExpressionAttribute(schema, regularExpressionAttribute);

                    continue;
                }

                if (attribute is StringLengthAttribute stringLengthAttribute)
                {
                    ApplyStringLengthAttribute(schema, stringLengthAttribute);

                    continue;
                }

                if (attribute is RequiredAttribute requiredAttribute)
                {
                    ApplyRequiredAttribute(schema, requiredAttribute);

                    continue;
                }
            }
        }

        private static void ApplyDataTypeAttribute(OpenApiSchema schema, DataTypeAttribute dataTypeAttribute)
        {
            var formats = new Dictionary<AnnotationsDataType, string>
            {
                { AnnotationsDataType.DateTime, "date-time" },
                { AnnotationsDataType.Date, "date" },
                { AnnotationsDataType.Time, "time" },
                { AnnotationsDataType.Duration, "duration" },
                { AnnotationsDataType.PhoneNumber, "tel" },
                { AnnotationsDataType.Currency, "currency" },
                { AnnotationsDataType.Text, "string" },
                { AnnotationsDataType.Html, "html" },
                { AnnotationsDataType.MultilineText, "multiline" },
                { AnnotationsDataType.EmailAddress, "email" },
                { AnnotationsDataType.Password, "password" },
                { AnnotationsDataType.Url, "uri" },
                { AnnotationsDataType.ImageUrl, "uri" },
                { AnnotationsDataType.CreditCard, "credit-card" },
                { AnnotationsDataType.PostalCode, "postal-code" }
            };

            if (formats.TryGetValue(dataTypeAttribute.DataType, out string format))
            {
                schema.Format = format;
            }
        }

        private static void ApplyMinLengthAttribute(OpenApiSchema schema, MinLengthAttribute minLengthAttribute)
        {
            if (schema.Type == "array")
            {
                schema.MinItems = minLengthAttribute.Length;
            }
            else
            {
                schema.MinLength = minLengthAttribute.Length;
            }
        }

        private static void ApplyMaxLengthAttribute(OpenApiSchema schema, MaxLengthAttribute maxLengthAttribute)
        {
            if (schema.Type == "array")
            {
                schema.MaxItems = maxLengthAttribute.Length;
            }
            else
            {
                schema.MaxLength = maxLengthAttribute.Length;
            }
        }

        private static void ApplyRangeAttribute(OpenApiSchema schema, RangeAttribute rangeAttribute)
        {
            schema.Maximum = decimal.TryParse(rangeAttribute.Maximum.ToString(), out decimal maximum)
                ? maximum
                : schema.Maximum;

            schema.Minimum = decimal.TryParse(rangeAttribute.Minimum.ToString(), out decimal minimum)
                ? minimum
                : schema.Minimum;
        }

        private static void ApplyRegularExpressionAttribute(OpenApiSchema schema, RegularExpressionAttribute regularExpressionAttribute)
        {
            schema.Pattern = regularExpressionAttribute.Pattern;
        }

        private static void ApplyStringLengthAttribute(OpenApiSchema schema, StringLengthAttribute stringLengthAttribute)
        {
            schema.MinLength = stringLengthAttribute.MinimumLength;
            schema.MaxLength = stringLengthAttribute.MaximumLength;
        }

        private static void ApplyRequiredAttribute(OpenApiSchema schema, RequiredAttribute requiredAttribute)
        {
            if (schema.Type == "string" && !requiredAttribute.AllowEmptyStrings && !IsMinLengthSet(schema))
            {
                schema.MinLength = 1;
            }
        }

        private static bool IsMinLengthSet(OpenApiSchema schema)
        {
            return schema.MinLength != null && schema.MinLength > 0;
        }
    }
}
