using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors
{
    /// <summary>
    /// This represents the type visitor for <see cref="List{T}"/>.
    /// </summary>
    public class ListObjectTypeVisitor : TypeVisitor
    {
        private readonly Dictionary<Type, OpenApiSchemaAcceptor> visitedTypes = new Dictionary<Type, OpenApiSchemaAcceptor>();

        /// <inheritdoc />
        public ListObjectTypeVisitor(VisitorCollection visitorCollection)
            : base(visitorCollection)
        {
        }

        /// <inheritdoc />
        public override bool IsVisitable(Type type)
        {
            var isVisitable = this.IsVisitable(type, TypeCode.Object) && type.IsOpenApiArray();

            return isVisitable;
        }

        /// <inheritdoc />
        public override void Visit(IAcceptor acceptor, KeyValuePair<string, Type> type, NamingStrategy namingStrategy, params Attribute[] attributes)
        {
            var name = this.Visit(acceptor, name: type.Key, title: null, dataType: "array", dataFormat: null, attributes: attributes);

            if (name.IsNullOrWhiteSpace())
            {
                return;
            }

            var instance = acceptor as OpenApiSchemaAcceptor;
            if (instance.IsNullOrDefault())
            {
                return;
            }

            // Gets the schema for the underlying type.
            var underlyingType = type.Value.GetUnderlyingType();
            var types = new Dictionary<string, Type>()
            {
                { underlyingType.GetOpenApiReferenceId(underlyingType.IsOpenApiDictionary(), underlyingType.IsOpenApiArray(), namingStrategy), underlyingType }
            };
            var schemas = new Dictionary<string, OpenApiSchema>();

            OpenApiSchemaAcceptor subAcceptor;
            if (!this.visitedTypes.ContainsKey(underlyingType))
            {
                subAcceptor = new OpenApiSchemaAcceptor()
                {
                    Types = types, RootSchemas = instance.RootSchemas, Schemas = schemas,
                };
                this.visitedTypes.Add(underlyingType, subAcceptor);
                subAcceptor.Accept(this.VisitorCollection, namingStrategy);

            }
            else
            {
                subAcceptor = this.visitedTypes[underlyingType];
            }

            var items = subAcceptor.Schemas.First().Value;

            // Forces to remove the title value from the items attribute.
            items.Title = null;

            // Adds the reference to the schema for the underlying type.
            if (this.IsReferential(underlyingType))
            {
                var reference = new OpenApiReference()
                {
                    Type = ReferenceType.Schema,
                    Id = underlyingType.GetOpenApiReferenceId(underlyingType.IsOpenApiDictionary(), underlyingType.IsOpenApiArray(), namingStrategy)
                };

                items.Reference = reference;
            }

            instance.Schemas[name].Items = items;

            // Adds schemas to the root.
            var schemasToBeAdded = subAcceptor.Schemas
                                              .Where(p => p.Value.IsOpenApiSchemaObject()
                                                       || p.Value.IsOpenApiSchemaArray()
                                                       || p.Value.IsOpenApiSchemaDictionary()
                                                    )
                                              .ToDictionary(p => p.Key, p => p.Value);

            if (!schemasToBeAdded.Any())
            {
                return;
            }

            foreach (var schema in schemasToBeAdded)
            {
                if (instance.RootSchemas.ContainsKey(schema.Key))
                {
                    continue;
                }

                instance.RootSchemas.Add(schema.Key, schema.Value);
            }
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
            var schema = this.ParameterVisit(dataType: "array", dataFormat: null);

            var underlyingType = type.GetUnderlyingType();
            var items = this.VisitorCollection.ParameterVisit(underlyingType, namingStrategy);

            schema.Items = items;

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
            var schema = this.PayloadVisit(dataType: "array", dataFormat: null);

            // Gets the schema for the underlying type.
            var underlyingType = type.GetUnderlyingType();
            var items = this.VisitorCollection.PayloadVisit(underlyingType, namingStrategy);

            // Adds the reference to the schema for the underlying type.
            if (underlyingType.IsReferentialType())
            {
                var reference = new OpenApiReference()
                {
                    Type = ReferenceType.Schema,
                    Id = underlyingType.GetOpenApiReferenceId(underlyingType.IsOpenApiDictionary(), underlyingType.IsOpenApiArray(), namingStrategy)
                };

                items.Reference = reference;
            }

            schema.Items = items;

            return schema;
        }
    }
}
