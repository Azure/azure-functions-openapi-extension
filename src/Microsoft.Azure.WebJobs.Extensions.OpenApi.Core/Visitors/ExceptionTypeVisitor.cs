using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors
{
    /// <summary>
    /// This represents the type visitor for <see cref="Exception"/>.
    /// </summary>
    public class ExceptionTypeVisitor : TypeVisitor
    {
        private readonly HashSet<string> _noAddedKeys = new HashSet<string>
        {
            "OBJECT",
            "JTOKEN",
            "JOBJECT",
            "JARRAY",
        };

        private readonly HashSet<string> _serializePropertyNameSet = new HashSet<string>
        {
            nameof(Exception.InnerException),
            nameof(Exception.Message),
            nameof(Exception.StackTrace),
        };

        private readonly HashSet<string> _normalPropertyNameSet;
        private readonly HashSet<string> _recursivePropertyNameSet;

        /// <inheritdoc />
        public ExceptionTypeVisitor(VisitorCollection visitorCollection)
            : base(visitorCollection)
        {
            var exceptionType = typeof(Exception);
            var exceptionProperties = exceptionType.GetProperties()
                                                   .Where(p => this._serializePropertyNameSet.Contains(p.Name))
                                                   .ToArray();

            var normalPropertyNames = exceptionProperties.Where(p => p.PropertyType != exceptionType)
                                                         .Select(p => p.Name);

            var recursivePropertyNames = exceptionProperties.Where(p => p.PropertyType == exceptionType)
                                                         .Select(p => p.Name);

            this._normalPropertyNameSet = new HashSet<string>(normalPropertyNames);
            this._recursivePropertyNameSet = new HashSet<string>(recursivePropertyNames);
        }

        /// <inheritdoc />
        public override bool IsVisitable(Type type)
        {
            var isVisitable = this.IsVisitable(type, TypeCode.Object) && type.IsOpenApiException();

            return isVisitable;
        }

        /// <inheritdoc />
        public override void Visit(IAcceptor acceptor, KeyValuePair<string, Type> type, NamingStrategy namingStrategy, params Attribute[] attributes)
        {
            var title = namingStrategy.GetPropertyName(type.Value.Name, hasSpecifiedName: false);
            var name = this.Visit(acceptor, name: type.Key, title: title, dataType: "object", dataFormat: null, attributes: attributes);

            if (name.IsNullOrWhiteSpace())
            {
                return;
            }

            var instance = acceptor as OpenApiSchemaAcceptor;
            if (instance.IsNullOrDefault())
            {
                return;
            }

            var properties = type.Value
                                 .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                 .Where(p => !p.ExistsCustomAttribute<JsonIgnoreAttribute>())
                                 .Where(p => p.PropertyType != type.Value)
                                 .ToArray();

            // Processes properties.
            var normalProperties = properties.Where(p => this._normalPropertyNameSet.Contains(p.Name))
                                             .ToDictionary(p => p.GetJsonPropertyName(namingStrategy), p => p);

            this.ProcessProperties(instance, name, normalProperties, namingStrategy);

            // Processes recursive properties
            var recursiveProperties = properties.Where(p => this._recursivePropertyNameSet.Contains(p.Name))
                                                .ToDictionary(p => p.GetJsonPropertyName(namingStrategy), p => p);

            var recursiveSchemas = recursiveProperties.ToDictionary(p => p.Key,
                                                                    p => new OpenApiSchema()
                                                                    {
                                                                        Type = "object",
                                                                        Reference = new OpenApiReference()
                                                                        {
                                                                            Type = ReferenceType.Schema,
                                                                            Id = p.Value.PropertyType.GetOpenApiReferenceId(isDictionary: false, isList: false, namingStrategy)
                                                                        }
                                                                    });
            foreach (var recursiveSchema in recursiveSchemas)
            {
                instance.Schemas[name].Properties.Add(recursiveSchema);
            }

            // Adds the reference.
            var reference = new OpenApiReference()
            {
                Type = ReferenceType.Schema,
                Id = type.Value.GetOpenApiReferenceId(isDictionary: false, isList: false, namingStrategy)
            };

            instance.Schemas[name].Reference = reference;
        }

        /// <inheritdoc />
        public override bool IsPayloadVisitable(Type type)
        {
            var isVisitable = this.IsVisitable(type);

            return isVisitable;
        }

        private void ProcessProperties(IOpenApiSchemaAcceptor instance, string schemaName, Dictionary<string, PropertyInfo> properties, NamingStrategy namingStrategy)
        {
            var schemas = new Dictionary<string, OpenApiSchema>();

            var subAcceptor = new OpenApiSchemaAcceptor()
            {
                Properties = properties,
                RootSchemas = instance.RootSchemas,
                Schemas = schemas,
            };

            subAcceptor.Accept(this.VisitorCollection, namingStrategy);

            // Add required properties to schema.
            var jsonPropertyAttributes = properties.Where(p => !p.Value.GetCustomAttribute<JsonPropertyAttribute>(inherit: false).IsNullOrDefault())
                                                   .Select(p => new KeyValuePair<string, JsonPropertyAttribute>(p.Key, p.Value.GetCustomAttribute<JsonPropertyAttribute>(inherit: false)))
                                                   .Where(p => p.Value.Required == Required.Always || p.Value.Required == Required.DisallowNull);
            foreach (var attribute in jsonPropertyAttributes)
            {
                instance.Schemas[schemaName].Required.Add(attribute.Key);
            }

            var jsonRequiredAttributes = properties.Where(p => !p.Value.GetCustomAttribute<JsonRequiredAttribute>(inherit: false).IsNullOrDefault())
                                                   .Select(p => new KeyValuePair<string, JsonRequiredAttribute>(p.Key, p.Value.GetCustomAttribute<JsonRequiredAttribute>(inherit: false)));
            foreach (var attribute in jsonRequiredAttributes)
            {
                var attributeName = namingStrategy.GetPropertyName(attribute.Key, hasSpecifiedName: false);
                if (instance.Schemas[schemaName].Required.Contains(attributeName))
                {
                    continue;
                }

                instance.Schemas[schemaName].Required.Add(attributeName);
            }

            instance.Schemas[schemaName].Properties = subAcceptor.Schemas;

            // Adds schemas to the root.
            var schemasToBeAdded = subAcceptor.Schemas
                                              .Where(p => !instance.Schemas.Keys.Contains(p.Key))
                                              .Where(p => p.Value.IsOpenApiSchemaObject())
                                              .ToDictionary(p => p.Value.Title, p => p.Value);

            foreach (var schema in schemasToBeAdded.Where(p => !this._noAddedKeys.Contains(p.Key.ToUpperInvariant())))
            {
                if (instance.RootSchemas.ContainsKey(schema.Key))
                {
                    continue;
                }

                instance.RootSchemas.Add(schema.Key, schema.Value);
            }

            // Removes title of each property.
            var subSchemas = instance.Schemas[schemaName].Properties;
            subSchemas = subSchemas.Select(p =>
                                    {
                                        p.Value.Title = null;
                                        return new KeyValuePair<string, OpenApiSchema>(p.Key, p.Value);
                                    })
                                   .ToDictionary(p => p.Key, p => p.Value);

            instance.Schemas[schemaName].Properties = subSchemas;
        }
    }
}
