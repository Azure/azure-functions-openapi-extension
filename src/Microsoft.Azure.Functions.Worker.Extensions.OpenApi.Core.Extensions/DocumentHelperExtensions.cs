using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Core.Extensions
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
        /// <returns>List of <see cref="MethodInfo"/> instances representing HTTP triggers.</returns>
        public static List<MethodInfo> GetHttpTriggerMethods(this IDocumentHelper helper, Assembly assembly)
        {
            var methods = assembly.GetTypes()
                                  .SelectMany(p => p.GetMethods())
                                  .Where(p => p.ExistsCustomAttribute<FunctionAttribute>())
                                  .Where(p => p.ExistsCustomAttribute<OpenApiOperationAttribute>())
                                  .Where(p => !p.ExistsCustomAttribute<OpenApiIgnoreAttribute>())
                                  .Where(p => p.GetParameters().FirstOrDefault(q => q.ExistsCustomAttribute<HttpTriggerAttribute>()) != null)
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
        /// Gets the <see cref="FunctionAttribute"/> from the method.
        /// </summary>
        /// <param name="helper"><see cref="IDocumentHelper"/> instance.</param>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <returns><see cref="FunctionAttribute"/> instance.</returns>
        public static FunctionAttribute GetFunctionNameAttribute(this IDocumentHelper helper, MethodInfo element)
        {
            var function = element.GetFunctionName();

            return function;
        }

        /// <summary>
        /// Gets the HTTP trigger endpoint.
        /// </summary>
        /// <param name="helper"><see cref="IDocumentHelper"/> instance.</param>
        /// <param name="function"><see cref="FunctionAttribute"/> instance.</param>
        /// <param name="trigger"><see cref="HttpTriggerAttribute"/> instance.</param>
        /// <returns>Function HTTP endpoint.</returns>
        public static string GetHttpEndpoint(this IDocumentHelper helper, FunctionAttribute function, HttpTriggerAttribute trigger)
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
        /// <param name="function"><see cref="FunctionAttribute"/> instance.</param>
        /// <param name="verb"><see cref="OperationType"/> value.</param>
        /// <returns><see cref="OpenApiOperation"/> instance.</returns>
        public static OpenApiOperation GetOpenApiOperation(this IDocumentHelper helper, MethodInfo element, FunctionAttribute function, OperationType verb)
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
        /// <returns>List of <see cref="OpenApiParameter"/> instance.</returns>
        public static List<OpenApiParameter> GetOpenApiParameters(this IDocumentHelper helper, MethodInfo element, HttpTriggerAttribute trigger, NamingStrategy namingStrategy, VisitorCollection collection)
        {
            var parameters = element.GetCustomAttributes<OpenApiParameterAttribute>(inherit: false)
                                    .Where(p => p.Deprecated == false)
                                    .Select(p => p.ToOpenApiParameter(namingStrategy, collection))
                                    .ToList();

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
