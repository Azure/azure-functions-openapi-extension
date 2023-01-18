using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors
{
    /// <summary>
    /// This represents the acceptor entity for <see cref="OpenApiSchema"/>.
    /// </summary>
    public class OpenApiSchemaAcceptor : IOpenApiSchemaAcceptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiSchemaAcceptor"/> class.
        /// </summary>
        public OpenApiSchemaAcceptor()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiSchemaAcceptor"/> class.
        /// </summary>
        /// <param name="rootSchemas">List of <see cref="OpenApiSchema"/> instances as key/value pair representing the root schemas.</param>
        /// <param name="schemas">List of <see cref="OpenApiSchema"/> instances as key/value pair.</param>
        /// <param name="types">List of <see cref="Type"/> objects.</param>
        public OpenApiSchemaAcceptor(Dictionary<string, OpenApiSchema> rootSchemas, Dictionary<string, OpenApiSchema> schemas, Dictionary<string, Type> types)
        {
            this.RootSchemas = rootSchemas.ThrowIfNullOrDefault();
            this.Schemas = schemas.ThrowIfNullOrDefault();
            this.Types = types.ThrowIfNullOrDefault();
        }

        /// <inheritdoc />
        public Dictionary<string, OpenApiSchema> RootSchemas { get; set; } = new Dictionary<string, OpenApiSchema>();

        /// <inheritdoc />
        public Dictionary<string, OpenApiSchema> Schemas { get; set; } = new Dictionary<string, OpenApiSchema>();

        /// <inheritdoc />
        public Dictionary<string, Type> Types { get; set; } = new Dictionary<string, Type>();

        /// <inheritdoc />
        public Dictionary<string, PropertyInfo> Properties { get; set; } = new Dictionary<string, PropertyInfo>();

        /// <inheritdoc />
        public void Accept(VisitorCollection collection, NamingStrategy namingStrategy)
        {
            // Checks the properties only.
            if (this.Properties.Any())
            {
                foreach (var property in this.Properties)
                {
                    var attributes = new List<Attribute>
                    {
                        property.Value.GetCustomAttribute<OpenApiSchemaVisibilityAttribute>(inherit: false),
                        property.Value.GetCustomAttribute<OpenApiPropertyAttribute>(inherit: false),
                    };
                    attributes.AddRange(property.Value.GetCustomAttributes<ValidationAttribute>(inherit: false));
                    attributes.AddRange(property.Value.GetCustomAttributes<JsonPropertyAttribute>(inherit: false));

                    foreach (var visitor in collection.Visitors)
                    {
                        if (!visitor.IsVisitable(property.Value.PropertyType))
                        {
                            continue;
                        }

                        var type = new KeyValuePair<string, Type>(property.Key, property.Value.PropertyType);
                        visitor.Visit(this, type, namingStrategy, attributes.ToArray());
                    }
                }

                return;
            }

            // Checks the types only.
            foreach (var type in this.Types)
            {
                foreach (var visitor in collection.Visitors)
                {
                    if (!visitor.IsVisitable(type.Value))
                    {
                        continue;
                    }

                    visitor.Visit(this, type, namingStrategy);
                }
            }
        }
    }
}
