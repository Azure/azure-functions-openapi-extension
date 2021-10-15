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

#if NETSTANDARD2_0
using FA = Microsoft.Azure.WebJobs.FunctionNameAttribute;
using TA = Microsoft.Azure.WebJobs.HttpTriggerAttribute;
#endif

#if NET5_0
using FA = Microsoft.Azure.Functions.Worker.FunctionAttribute;
using TA = Microsoft.Azure.Functions.Worker.HttpTriggerAttribute;
#endif

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
                                  .Where(p => p.ExistsCustomAttribute<FA>())
                                  .Where(p => p.ExistsCustomAttribute<OpenApiOperationAttribute>())
                                  .Where(p => !p.ExistsCustomAttribute<OpenApiIgnoreAttribute>())
                                  .Where(p => p.GetParameters().FirstOrDefault(q => q.ExistsCustomAttribute<TA>()) != null)
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
        /// Gets the <see cref="TA"/> from the parameters of the method.
        /// </summary>
        /// <param name="helper"><see cref="IDocumentHelper"/> instance.</param>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <returns><see cref="TA"/> instance.</returns>
        public static TA GetHttpTriggerAttribute(this IDocumentHelper helper, MethodInfo element)
        {
            element.ThrowIfNullOrDefault();

            var trigger = element.GetParameters()
                .First()
                .GetCustomAttribute<TA>(inherit: false);

            return trigger;
        }

        /// <summary>
        /// Gets the <see cref="FA"/> from the method.
        /// </summary>
        /// <param name="helper"><see cref="IDocumentHelper"/> instance.</param>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <returns><see cref="FA"/> instance.</returns>
        public static FA GetFunctionNameAttribute(this IDocumentHelper helper, MethodInfo element)
        {
            element.ThrowIfNullOrDefault();

            var function = element.GetCustomAttribute<FA>(inherit: false);

            return function;
        }

        /// <summary>
        /// Gets the HTTP trigger endpoint.
        /// </summary>
        /// <param name="helper"><see cref="IDocumentHelper"/> instance.</param>
        /// <param name="function"><see cref="FA"/> instance.</param>
        /// <param name="trigger"><see cref="TA"/> instance.</param>
        /// <returns>Function HTTP endpoint.</returns>
        public static string GetHttpEndpoint(this IDocumentHelper helper, FA function, TA trigger)
        {
            var endpoint = $"/{(string.IsNullOrWhiteSpace(trigger.Route) ? function.Name : helper.FilterRouteConstraints(trigger.Route)).Trim('/')}";

            return endpoint;
        }

        /// <summary>
        /// Gets the HTTP verb.
        /// </summary>
        /// <param name="helper"><see cref="IDocumentHelper"/> instance.</param>
        /// <param name="trigger"><see cref="TA"/> instance.</param>
        /// <returns><see cref="OperationType"/> value.</returns>
        public static OperationType GetHttpVerb(this IDocumentHelper helper, TA trigger)
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
        /// <param name="function"><see cref="FA"/> instance.</param>
        /// <param name="verb"><see cref="OperationType"/> value.</param>
        /// <returns><see cref="OpenApiOperation"/> instance.</returns>
        public static OpenApiOperation GetOpenApiOperation(this IDocumentHelper helper, MethodInfo element, FA function, OperationType verb)
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
        /// <param name="trigger"><see cref="TA"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance to create the JSON schema from .NET Types.</param>
        /// <param name="collection"><see cref="VisitorCollection"/> instance to process parameters.</param>
        /// <returns>List of <see cref="OpenApiParameter"/> instance.</returns>
        public static List<OpenApiParameter> GetOpenApiParameters(this IDocumentHelper helper, MethodInfo element, TA trigger, NamingStrategy namingStrategy, VisitorCollection collection)
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
