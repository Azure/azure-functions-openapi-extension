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
    /// This represents the type visitor for <see cref="Dictionary{TKey, TValue}"/>.
    /// </summary>
    public class DictionaryObjectTypeVisitor : TypeVisitor
    {
        private readonly Dictionary<Type, OpenApiSchemaAcceptor> visitedTypes = new Dictionary<Type, OpenApiSchemaAcceptor>();

        /// <inheritdoc />
        public DictionaryObjectTypeVisitor(VisitorCollection visitorCollection)
            : base(visitorCollection)
        {
        }

        /// <inheritdoc />
        public override bool IsVisitable(Type type)
        {
            var isVisitable = this.IsVisitable(type, TypeCode.Object) && type.IsOpenApiDictionary();

            return isVisitable;
        }

        /// <inheritdoc />
        public override void Visit(IAcceptor acceptor, KeyValuePair<string, Type> type, NamingStrategy namingStrategy, params Attribute[] attributes)
        {
            var name = this.Visit(acceptor, name: type.Key, title: null, dataType: "object", dataFormat: null, attributes: attributes);

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

            var properties = subAcceptor.Schemas.First().Value;

            // Forces to remove the title value from the additionalProperties attribute.
            properties.Title = null;

            // Adds the reference to the schema for the underlying type.
            if (this.IsReferential(underlyingType))
            {
                var reference = new OpenApiReference()
                {
                    Type = ReferenceType.Schema,
                    Id = underlyingType.GetOpenApiReferenceId(underlyingType.IsOpenApiDictionary(), underlyingType.IsOpenApiArray(), namingStrategy)
                };

                properties.Reference = reference;
            }

            instance.Schemas[name].AdditionalProperties = properties;

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

            foreach (var schemaToBeAdded in schemasToBeAdded)
            {
                if (instance.RootSchemas.ContainsKey(schemaToBeAdded.Key))
                {
                    continue;
                }

                instance.RootSchemas.Add(schemaToBeAdded.Key, schemaToBeAdded.Value);
            }
        }

        /// <inheritdoc />
        public override bool IsParameterVisitable(Type type)
        {
            return false;
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
            var schema = this.PayloadVisit(dataType: "object", dataFormat: null);

            // Gets the schema for the underlying type.
            var underlyingType = type.GetUnderlyingType();
            var properties = this.VisitorCollection.PayloadVisit(underlyingType, namingStrategy);

            // Adds the reference to the schema for the underlying type.
            if (underlyingType.IsReferentialType())
            {
                var reference = new OpenApiReference()
                {
                    Type = ReferenceType.Schema,
                    Id = underlyingType.GetOpenApiReferenceId(underlyingType.IsOpenApiDictionary(), underlyingType.IsOpenApiArray(), namingStrategy)
                };

                properties.Reference = reference;
            }

            schema.AdditionalProperties = properties;

            return schema;
        }
    }
}
