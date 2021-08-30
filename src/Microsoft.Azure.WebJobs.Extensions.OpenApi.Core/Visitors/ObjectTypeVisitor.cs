using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors
{
    /// <summary>
    /// This represents the type visitor for <see cref="object"/>.
    /// </summary>
    public class ObjectTypeVisitor : TypeVisitor
    {
        /// <inheritdoc />
        public ObjectTypeVisitor(VisitorCollection visitorCollection)
            : base(visitorCollection)
        {
        }

        /// <inheritdoc />
        public override bool IsVisitable(Type type)
        {
            var isVisitable = this.IsVisitable(type, TypeCode.Object);

            if (type == typeof(Guid))
            {
                isVisitable = false;
            }
            else if (type == typeof(DateTime))
            {
                isVisitable = false;
            }
            else if (type == typeof(DateTimeOffset))
            {
                isVisitable = false;
            }
            else if (type == typeof(Uri))
            {
                isVisitable = false;
            }
            else if (type == typeof(Type))
            {
                isVisitable = false;
            }
            else if (type.IsOpenApiArray())
            {
                isVisitable = false;
            }
            else if (type.IsOpenApiDictionary())
            {
                isVisitable = false;
            }
            else if (type.IsOpenApiNullable())
            {
                isVisitable = false;
            }
            else if (type.IsUnflaggedEnumType())
            {
                isVisitable = false;
            }
            else if (type.IsJObjectType())
            {
                isVisitable = false;
            }
            else if (type.HasRecursiveProperty())
            {
                isVisitable = false;
            }

            return isVisitable;
        }

        /// <inheritdoc />
        public override void Visit(IAcceptor acceptor, KeyValuePair<string, Type> type, NamingStrategy namingStrategy, params Attribute[] attributes)
        {
            var title = type.Value.IsGenericType
                ? namingStrategy.GetPropertyName(type.Value.Name.Split('`').First(), hasSpecifiedName: false) + "_" +
                  string.Join("_",
                      type.Value.GenericTypeArguments.Select(a => namingStrategy.GetPropertyName(a.Name, false)))
                : namingStrategy.GetPropertyName(type.Value.Name, hasSpecifiedName: false);
            var name = this.Visit(acceptor, name: type.Key, title: title, dataType: "object", dataFormat: null, attributes: attributes);

            if (name.IsNullOrWhiteSpace())
            {
                return;
            }

            if (!this.IsNavigatable(type.Value))
            {
                return;
            }

            var instance = acceptor as OpenApiSchemaAcceptor;
            if (instance.IsNullOrDefault())
            {
                return;
            }

            // Processes properties.
            var schemas = this.GetAllSchemas(instance);
            var properties = type.Value
                                 .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                 .Where(p => !p.ExistsCustomAttribute<JsonIgnoreAttribute>())
                                 .Select(p => new KeyValuePair<string, PropertyInfo>(p.GetJsonPropertyName(namingStrategy), p))
                                 //.Where(kv => !schemas.ContainsKey(kv.Key))
                                 .ToDictionary(kv => kv.Key, kv => kv.Value);

            this.ProcessProperties(instance, name, properties, namingStrategy);

            // Adds the reference.
            var reference = new OpenApiReference()
            {
                Type = ReferenceType.Schema,
                Id = type.Value.GetOpenApiReferenceId(isDictionary: false, isList: false, namingStrategy)
            };

            instance.Schemas[name].Reference = reference;

            instance.Schemas[name].Example = this.GetExample(type.Value, namingStrategy);
        }

        /// <inheritdoc />
        public override bool IsNavigatable(Type type)
        {
            if (type.IsJObjectType())
            {
                return false;
            }

            if (type.IsOpenApiNullable())
            {
                return false;
            }

            if (type.IsOpenApiArray())
            {
                return false;
            }

            if (type.IsOpenApiDictionary())
            {
                return false;
            }

            return true;
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
            return this.PayloadVisit(dataType: "object", dataFormat: null);
        }

        private void ProcessProperties(IOpenApiSchemaAcceptor instance, string schemaName, Dictionary<string, PropertyInfo> properties, NamingStrategy namingStrategy)
        {
            var schemas = new Dictionary<string, OpenApiSchema>();

            var subAcceptor = new OpenApiSchemaAcceptor()
            {
                Parent = instance,
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
                                              .Where(p => p.Value.IsOpenApiSchemaObject())
                                              .GroupBy(p => p.Value.Title)
                                              .Select(p => p.First())
                                              .ToDictionary(p => p.Value.Title, p => p.Value);

            foreach (var schema in schemasToBeAdded.Where(p => p.Key != "jObject" && p.Key != "jToken"))
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

        private IOpenApiAny GetExample(Type type, NamingStrategy namingStrategy = null)
        {
            var attr = type.GetCustomAttribute<OpenApiExampleAttribute>(inherit: false);
            if (attr.IsNullOrDefault())
            {
                return null;
            }

            var instance = (dynamic)Activator.CreateInstance(attr.Example);
            var examples = (IDictionary<string, OpenApiExample>)instance.Build(namingStrategy).Examples;
            var example = examples.First().Value;

            return example.Value;
        }
    }
}
