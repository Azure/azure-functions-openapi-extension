using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for the <see cref="DocumentHelper"/> class.
    /// </summary>
    public static class DocumentHelperExtensions
    {
        /// <summary>
        /// Gets the list of HTTP triggers.
        /// </summary>
        /// <param name="helper"><see cref="IDocumentHelper"/> instance.</param>
        /// <param name="assembly">Assembly of Azure Function instance.</param>
        /// <param name="tags">List of tags to filter methods.</param>
        /// <returns>List of <see cref="MethodInfo"/> instances representing HTTP triggers.</returns>
        public static List<MethodInfo> GetHttpTriggerMethods(this IDocumentHelper helper, Assembly assembly, IEnumerable<string> tags = null)
        {
            var methods = assembly.GetLoadableTypes()
                                  .SelectMany(p => p.GetMethods())
                                  .Where(p => p.ExistsCustomAttribute<FunctionNameAttribute>())
                                  .Where(p => p.ExistsCustomAttribute<OpenApiOperationAttribute>())
                                  .Where(p => !p.ExistsCustomAttribute<OpenApiIgnoreAttribute>())
                                  .Where(p => p.GetParameters().FirstOrDefault(q => q.ExistsCustomAttribute<HttpTriggerAttribute>()) != null)
                                  .ToList();

            if (!tags.Any())
            {
                return methods;
            }

            methods = methods.Where(p => p.GetCustomAttribute<OpenApiOperationAttribute>()
                                          .Tags.Any(q => tags.Contains(q)))
                             .ToList();

            return methods;
        }

        /// <summary>
        /// Gets the <see cref="HttpTriggerAttribute"/> from the parameters of the method.
        /// </summary>
        /// <param name="helper"><see cref="IDocumentHelper"/> instance.</param>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <returns><see cref="HttpTriggerAttribute"/> instance.</returns>
        public static HttpTriggerAttribute GetHttpTriggerAttribute(this IDocumentHelper helper, MethodInfo element)
        {
            var trigger = element.GetHttpTrigger();

            return trigger;
        }

        /// <summary>
        /// Gets the <see cref="FunctionNameAttribute"/> from the method.
        /// </summary>
        /// <param name="helper"><see cref="IDocumentHelper"/> instance.</param>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <returns><see cref="FunctionNameAttribute"/> instance.</returns>
        public static FunctionNameAttribute GetFunctionNameAttribute(this IDocumentHelper helper, MethodInfo element)
        {
            var function = element.GetFunctionName();

            return function;
        }

        /// <summary>
        /// Gets the HTTP trigger endpoint.
        /// </summary>
        /// <param name="helper"><see cref="IDocumentHelper"/> instance.</param>
        /// <param name="function"><see cref="FunctionNameAttribute"/> instance.</param>
        /// <param name="trigger"><see cref="HttpTriggerAttribute"/> instance.</param>
        /// <returns>Function HTTP endpoint.</returns>
        public static string GetHttpEndpoint(this IDocumentHelper helper, FunctionNameAttribute function, HttpTriggerAttribute trigger)
        {
            var endpoint = $"/{(string.IsNullOrWhiteSpace(trigger.Route) ? function.Name : helper.FilterRouteConstraints(trigger.Route)).Trim('/')}";

            return endpoint;
        }

        /// <summary>
        /// Gets the HTTP verb.
        /// </summary>
        /// <param name="helper"><see cref="IDocumentHelper"/> instance.</param>
        /// <param name="trigger"><see cref="HttpTriggerAttribute"/> instance.</param>
        /// <returns><see cref="OperationType"/> value.</returns>
        public static OperationType GetHttpVerb(this IDocumentHelper helper, HttpTriggerAttribute trigger)
        {
            var verb = Enum.TryParse<OperationType>(trigger.Methods.First(), true, out OperationType ot)
                           ? ot
                           : throw new InvalidOperationException();

            return verb;
        }

        /// <summary>
        /// Gets the <see cref="OpenApiOperation"/> instance.
        /// </summary>
        /// <param name="helper"><see cref="IDocumentHelper"/> instance.</param>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <param name="function"><see cref="FunctionNameAttribute"/> instance.</param>
        /// <param name="verb"><see cref="OperationType"/> value.</param>
        /// <returns><see cref="OpenApiOperation"/> instance.</returns>
        public static OpenApiOperation GetOpenApiOperation(this IDocumentHelper helper, MethodInfo element, FunctionNameAttribute function, OperationType verb)
        {
            var op = element.GetOpenApiOperation();
            if (op.IsNullOrDefault())
            {
                return null;
            }

            var operation = new OpenApiOperation()
            {
                OperationId = string.IsNullOrWhiteSpace(op.OperationId) ? $"{function.Name}_{verb}" : op.OperationId,
                Tags = op.Tags.Select(p => new OpenApiTag() { Name = p }).ToList(),
                Summary = op.Summary,
                Description = op.Description,
                Deprecated = op.Deprecated
            };

            if (op.Visibility != OpenApiVisibilityType.Undefined)
            {
                var visibility = new OpenApiString(op.Visibility.ToDisplayName());

                operation.Extensions.Add("x-ms-visibility", visibility);
            }

            return operation;
        }

        /// <summary>
        /// Gets the list of <see cref="OpenApiParameter"/> instances.
        /// </summary>
        /// <param name="helper"><see cref="IDocumentHelper"/> instance.</param>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <param name="trigger"><see cref="HttpTriggerAttribute"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance to create the JSON schema from .NET Types.</param>
        /// <param name="collection"><see cref="VisitorCollection"/> instance to process parameters.</param>
        /// <param name="version"><see cref="OpenApiVersionType"/> value.</param>
        /// <returns>List of <see cref="OpenApiParameter"/> instance.</returns>
        public static List<OpenApiParameter> GetOpenApiParameters(this IDocumentHelper helper, MethodInfo element, HttpTriggerAttribute trigger, NamingStrategy namingStrategy, VisitorCollection collection, OpenApiVersionType version)
        {
            var parameters = element.GetCustomAttributes<OpenApiParameterAttribute>(inherit: false)
                                    .Where(p => p.Deprecated == false)
                                    .Select(p => p.ToOpenApiParameter(namingStrategy, collection, version))
                                    .ToList();

            // This is the interim solution to resolve:
            // https://github.com/Azure/azure-functions-openapi-extension/issues/365
            //
            // It will be removed when the following issue is resolved:
            // https://github.com/microsoft/OpenAPI.NET/issues/747
            if (version == OpenApiVersionType.V3)
            {
                return parameters;
            }

            var attributes = element.GetCustomAttributes<OpenApiRequestBodyAttribute>(inherit: false);
            if (!attributes.Any())
            {
                return parameters;
            }

            var contents = attributes.Where(p => p.Deprecated == false)
                                     .Where(p => p.ContentType == "application/x-www-form-urlencoded" || p.ContentType == "multipart/form-data")
                                     .Select(p => p.ToOpenApiMediaType(namingStrategy, collection, version));
            if (!contents.Any())
            {
                return parameters;
            }

            var @ref = contents.First().Schema.Reference;
            var schemas = helper.GetOpenApiSchemas(new[] { element }.ToList(), namingStrategy, collection);
            var schema = schemas.SingleOrDefault(p => p.Key == @ref.Id);
            if (schema.IsNullOrDefault())
            {
                return parameters;
            }

            var properties = schema.Value.Properties;
            foreach (var property in properties)
            {
                var value = property.Value;
                if ((value.Type == "string" && value.Format == "binary") || (value.Type == "string" && value.Format == "base64"))
                {
                    value.Type = "file";
                    value.Format = null;
                }

                var parameter = new OpenApiParameter()
                {
                    Name = property.Key,
                    Description = $"[formData]{value.Description}",
                    Required = bool.TryParse($"{value.Required}", out var result) ? result : false,
                    Deprecated = value.Deprecated,
                    Schema = value,
                };

                parameters.Add(parameter);
            }

            // // TODO: Should this be forcibly provided?
            // // This needs to be provided separately.
            // if (trigger.AuthLevel != AuthorizationLevel.Anonymous)
            // {
            //     parameters.AddOpenApiParameter<string>("code", @in: ParameterLocation.Query, required: false);
            // }

            return parameters;
        }
    }
}
