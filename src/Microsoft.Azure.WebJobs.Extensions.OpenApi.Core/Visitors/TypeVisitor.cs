using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors
{
    /// <summary>
    /// This represents the visitor entity for type. This MUST be inherited.
    /// </summary>
    public abstract class TypeVisitor : IVisitor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeVisitor"/> class.
        /// </summary>
        /// <param name="visitorCollection"><see cref="VisitorCollection"/> instance.</param>
        protected TypeVisitor(VisitorCollection visitorCollection)
        {
            this.VisitorCollection = visitorCollection.ThrowIfNullOrDefault();
        }

        /// <summary>
        /// Gets the <see cref="Type"/> object.
        /// </summary>
        protected Type Type { get; private set; }

        /// <summary>
        /// Gets the <see cref="VisitorCollection"/>.
        /// </summary>
        protected VisitorCollection VisitorCollection { get; }

        /// <inheritdoc />
        public virtual bool IsVisitable(Type type)
        {
            return false;
        }

        /// <inheritdoc />
        public virtual void Visit(IAcceptor acceptor, KeyValuePair<string, Type> type, NamingStrategy namingStrategy, params Attribute[] attributes)
        {
            return;
        }

        /// <inheritdoc />
        public virtual bool IsNavigatable(Type type)
        {
            return false;
        }

        /// <inheritdoc />
        public virtual bool IsParameterVisitable(Type type)
        {
            return false;
        }

        /// <inheritdoc />
        public virtual OpenApiSchema ParameterVisit(Type type, NamingStrategy namingStrategy)
        {
            return default;
        }

        /// <inheritdoc />
        public virtual bool IsPayloadVisitable(Type type)
        {
            return false;
        }

        /// <inheritdoc />
        public virtual OpenApiSchema PayloadVisit(Type type, NamingStrategy namingStrategy)
        {
            return default;
        }

        /// <summary>
        /// Checks whether the type is visitable or not.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <param name="code"><see cref="TypeCode"/> value.</param>
        /// <returns>Returns <c>True</c>, if the type is visitable; otherwise returns <c>False</c>.</returns>
        protected bool IsVisitable(Type type, TypeCode code)
        {
            var @enum = Type.GetTypeCode(type);
            var isVisitable = @enum == code;

            if (isVisitable)
            {
                this.Type = type;
            }

            return isVisitable;
        }

        /// <summary>
        /// Checks whether the type can be referenced or not.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>Returns <c>True</c>, if the type can be referenced; otherwise returns <c>False</c>.</returns>
        protected bool IsReferential(Type type)
        {
            var isReferential = type.IsReferentialType();

            return isReferential;
        }

        /// <summary>
        /// Visits and processes the acceptor.
        /// </summary>
        /// <param name="acceptor"><see cref="IAcceptor"/> instance.</param>
        /// <param name="name">Property name.</param>
        /// <param name="title">Type name.</param>
        /// <param name="dataType">Data type.</param>
        /// <param name="dataFormat">Data format.</param>
        /// <param name="attributes">List of attribute instances.</param>
        /// <returns>Returns the name as the schema key.</returns>
        protected string Visit(IAcceptor acceptor, string name, string title, string dataType, string dataFormat, params Attribute[] attributes)
        {
            var instance = acceptor as OpenApiSchemaAcceptor;
            if (instance.IsNullOrDefault())
            {
                return null;
            }

            if (instance.Schemas.ContainsKey(name))
            {
                return null;
            }

            var schema = new OpenApiSchema()
            {
                Title = title,
                Type = dataType,
                Format = dataFormat
            };

            // Adds the extra properties.
            if (attributes.Any())
            {
                Attribute attr = attributes.OfType<OpenApiPropertyAttribute>().SingleOrDefault();
                if (!attr.IsNullOrDefault())
                {
                    if (dataType != "object")
                    {
                        schema.Nullable = this.GetOpenApiPropertyNullable(attr as OpenApiPropertyAttribute);
                        schema.Default = this.GetOpenApiPropertyDefault(attr as OpenApiPropertyAttribute);
                    }
                    schema.Description = this.GetOpenApiPropertyDescription(attr as OpenApiPropertyAttribute);
                    schema.Deprecated = this.GetOpenApiPropertyDeprecated(attr as OpenApiPropertyAttribute);
                }

                attr = attributes.OfType<OpenApiSchemaVisibilityAttribute>().SingleOrDefault();
                if (!attr.IsNullOrDefault())
                {
                    var extension = new OpenApiString((attr as OpenApiSchemaVisibilityAttribute).Visibility.ToDisplayName());

                    schema.Extensions.Add("x-ms-visibility", extension);
                }

                schema.ApplyValidationAttributes(attributes.OfType<ValidationAttribute>());
            }

            instance.Schemas.Add(name, schema);

            return name;
        }

        /// <summary>
        /// Visits and processes the <see cref="OpenApiSchema"/> for parameters.
        /// </summary>
        /// <param name="dataType">Data type.</param>
        /// <param name="dataFormat">Data format.</param>
        /// <returns>Returns <see cref="OpenApiSchema"/> instance.</returns>
        protected OpenApiSchema ParameterVisit(string dataType, string dataFormat)
        {
            var schema = new OpenApiSchema()
            {
                Type = dataType,
                Format = dataFormat
            };

            return schema;
        }

        /// <summary>
        /// Visits and processes the <see cref="OpenApiSchema"/> for payloads.
        /// </summary>
        /// <param name="dataType">Data type.</param>
        /// <param name="dataFormat">Data format.</param>
        /// <returns>Returns <see cref="OpenApiSchema"/> instance.</returns>
        protected OpenApiSchema PayloadVisit(string dataType, string dataFormat)
        {
            var schema = new OpenApiSchema()
            {
                Type = dataType,
                Format = dataFormat
            };

            return schema;
        }

        /// <summary>
        /// Gets the value indicating whether the property is nullable or not.
        /// </summary>
        /// <param name="attr"><see cref="OpenApiPropertyAttribute"/> instance.</param>
        /// <returns>Returns <c>True</c>, if nullable; otherwise returns <c>False</c>.</returns>
        protected bool GetOpenApiPropertyNullable(OpenApiPropertyAttribute attr)
        {
            return attr.Nullable;
        }

        /// <summary>
        /// Gets the default value from the <see cref="OpenApiPropertyAttribute"/> instance.
        /// </summary>
        /// <param name="attr"><see cref="OpenApiPropertyAttribute"/> instance.</param>
        /// <returns>Returns the default data.</returns>
        protected IOpenApiAny GetOpenApiPropertyDefault(OpenApiPropertyAttribute attr)
        {
            var @default = attr.Default;
            if (@default.IsNullOrDefault())
            {
                return null;
            }

            if (@default is bool)
            {
                return new OpenApiBoolean((bool) @default);
            }

            if (@default is DateTime)
            {
                return new OpenApiDateTime((DateTime) @default);
            }

            if (@default is TimeSpan)
            {
                return new OpenApiString(@default.ToString());
            }

            if (@default is DateTimeOffset)
            {
                return new OpenApiDateTime((DateTimeOffset) @default);
            }

            if (@default is float)
            {
                return new OpenApiFloat((float) @default);
            }

            if (@default is double)
            {
                return new OpenApiDouble((double) @default);
            }

            if (@default is decimal)
            {
                return new OpenApiDouble(Convert.ToDouble(@default));
            }

            if (@default is byte[])
            {
                return new OpenApiByte((byte[]) @default);
            }

            if (@default is short)
            {
                return new OpenApiInteger((short) @default);
            }

            if (@default is int)
            {
                return new OpenApiInteger((int) @default);
            }

            if (@default is long)
            {
                return new OpenApiLong((long) @default);
            }

            if (@default is ushort)
            {
                return new OpenApiInteger(Convert.ToInt16(@default));
            }

            if (@default is uint)
            {
                return new OpenApiInteger(Convert.ToInt32(@default));
            }

            if (@default is ulong)
            {
                return new OpenApiLong(Convert.ToInt64(@default));
            }

            if (@default is Guid)
            {
                return new OpenApiString(Convert.ToString(@default));
            }

            return new OpenApiString((string) @default);
        }

        /// <summary>
        /// Gets the default value from the <see cref="OpenApiPropertyAttribute"/> instance.
        /// </summary>
        /// <typeparam name="T">Type to compare.</typeparam>
        /// <param name="attr"><see cref="OpenApiPropertyAttribute"/> instance.</param>
        /// <returns>Returns the default data.</returns>
        protected IOpenApiAny GetOpenApiPropertyDefault<T>(OpenApiPropertyAttribute attr)
        {
            var @default = attr.Default;
            if (@default.IsNullOrDefault())
            {
                return null;
            }

            if (typeof(T) == typeof(short))
            {
                return new OpenApiInteger((short) @default);
            }

            if (typeof(T) == typeof(int))
            {
                return new OpenApiInteger((int) @default);
            }

            if (typeof(T) == typeof(byte))
            {
                return new OpenApiInteger(Convert.ToInt32(@default));
            }

            if (typeof(T) == typeof(long))
            {
                return new OpenApiLong((long) @default);
            }

            if (@default is ushort)
            {
                return new OpenApiInteger(Convert.ToInt16(@default));
            }

            if (@default is uint)
            {
                return new OpenApiInteger(Convert.ToInt32(@default));
            }

            if (@default is ulong)
            {
                return new OpenApiLong(Convert.ToInt64(@default));
            }

            if (typeof(T) == typeof(string) && @default.GetType().IsEnumType())
            {
                var @enum = (Enum)Convert.ChangeType(@default, typeof(Enum));

                return new OpenApiString((string) EnumExtensions.ToDisplayName(@enum));
            }

            return new OpenApiString((string) @default);
        }

        /// <summary>
        /// Gets the property description.
        /// </summary>
        /// <param name="attr"><see cref="OpenApiPropertyAttribute"/> instance.</param>
        /// <returns>Returns the property description.</returns>
        protected string GetOpenApiPropertyDescription(OpenApiPropertyAttribute attr)
        {
            return attr.Description;
        }


        /// <summary>
        /// Gets the property deprecated.
        /// </summary>
        /// <param name="attr"><see cref="OpenApiPropertyAttribute"/> instance.</param>
        /// <returns>Returns the property deprecated.</returns>
        protected bool GetOpenApiPropertyDeprecated(OpenApiPropertyAttribute attr)
        {
            return attr.Deprecated;
        }
    }
}
