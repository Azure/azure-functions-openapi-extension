using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core
{
    /// <summary>
    /// This represents the helper entity for the <see cref="Document"/> class.
    /// </summary>
    public class DocumentHelper : IDocumentHelper
    {
        private readonly RouteConstraintFilter _filter;
        private readonly IOpenApiSchemaAcceptor _acceptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentHelper"/> class.
        /// </summary>
        /// <param name="filter"><see cref="RouteConstraintFilter"/> instance.</param>
        /// <param name="acceptor"><see cref="IAcceptor"/> instance.</param>
        public DocumentHelper(RouteConstraintFilter filter, IOpenApiSchemaAcceptor acceptor)
        {
            this._filter = filter.ThrowIfNullOrDefault();
            this._acceptor = acceptor.ThrowIfNullOrDefault();
        }

        /// <inheritdoc />
        public List<MethodInfo> GetHttpTriggerMethods(Assembly assembly)
        {
            var methods = assembly.GetTypes()
                                  .SelectMany(p => p.GetMethods())
                                  .Where(p => p.ExistsCustomAttribute<FunctionNameAttribute>())
                                  .Where(p => p.ExistsCustomAttribute<OpenApiOperationAttribute>())
                                  .Where(p => !p.ExistsCustomAttribute<OpenApiIgnoreAttribute>())
                                  .Where(p => p.GetParameters().FirstOrDefault(q => q.ExistsCustomAttribute<HttpTriggerAttribute>()) != null)
                                  .ToList();

            return methods;
        }

        /// <inheritdoc />
        public HttpTriggerAttribute GetHttpTriggerAttribute(MethodInfo element)
        {
            var trigger = element.GetHttpTrigger();

            return trigger;
        }

        /// <inheritdoc />
        public FunctionNameAttribute GetFunctionNameAttribute(MethodInfo element)
        {
            var function = element.GetFunctionName();

            return function;
        }

        /// <inheritdoc />
        public string GetHttpEndpoint(FunctionNameAttribute function, HttpTriggerAttribute trigger)
        {
            var endpoint = $"/{(string.IsNullOrWhiteSpace(trigger.Route) ? function.Name : this.FilterRoute(trigger.Route)).Trim('/')}";

            return endpoint;
        }

        /// <inheritdoc />
        public OperationType GetHttpVerb(HttpTriggerAttribute trigger)
        {
            var verb = Enum.TryParse<OperationType>(trigger.Methods.First(), true, out OperationType ot)
                           ? ot
                           : throw new InvalidOperationException();

            return verb;
        }

        /// <inheritdoc />
        public OpenApiPathItem GetOpenApiPath(string path, OpenApiPaths paths)
        {
            var item = paths.ContainsKey(path) ? paths[path] : new OpenApiPathItem();

            return item;
        }

        /// <inheritdoc />
        public OpenApiOperation GetOpenApiOperation(MethodInfo element, FunctionNameAttribute function, OperationType verb)
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
                Description = op.Description
            };

            if (op.Visibility != OpenApiVisibilityType.Undefined)
            {
                var visibility = new OpenApiString(op.Visibility.ToDisplayName());

                operation.Extensions.Add("x-ms-visibility", visibility);
            }

            return operation;
        }

        /// <inheritdoc />
        public List<OpenApiParameter> GetOpenApiParameters(MethodInfo element, HttpTriggerAttribute trigger, NamingStrategy namingStrategy, VisitorCollection collection)
        {
            var parameters = element.GetCustomAttributes<OpenApiParameterAttribute>(inherit: false)
                                    .Select(p => p.ToOpenApiParameter(namingStrategy, collection))
                                    .ToList();

            // TODO: Should this be forcibly provided?
            // This needs to be provided separately.
            if (trigger.AuthLevel != AuthorizationLevel.Anonymous)
            {
                parameters.AddOpenApiParameter<string>("code", @in: ParameterLocation.Query, required: false);
            }

            return parameters;
        }

        /// <inheritdoc />
        public OpenApiRequestBody GetOpenApiRequestBody(MethodInfo element, NamingStrategy namingStrategy, VisitorCollection collection)
        {
            var attributes = element.GetCustomAttributes<OpenApiRequestBodyAttribute>(inherit: false);
            if (!attributes.Any())
            {
                return null;
            }

            var contents = attributes.ToDictionary(p => p.ContentType, p => p.ToOpenApiMediaType(namingStrategy, collection));

            if (contents.Any())
            {
                return new OpenApiRequestBody()
                {
                    Content = contents,
                    Required = attributes.First().Required
                };
            }

            return null;
        }

        /// <inheritdoc />
        [Obsolete("This method is obsolete from 2.0.0. Use GetOpenApiResponses instead", error: true)]
        public OpenApiResponses GetOpenApiResponseBody(MethodInfo element, NamingStrategy namingStrategy = null)
        {
            return this.GetOpenApiResponses(element, namingStrategy, null);

            //var responses = element.GetCustomAttributes<OpenApiResponseBodyAttribute>(inherit: false)
            //                       .ToDictionary(p => ((int)p.StatusCode).ToString(), p => p.ToOpenApiResponse(namingStrategy))
            //                       .ToOpenApiResponses();

            //return responses;
        }

        /// <inheritdoc />
        public OpenApiResponses GetOpenApiResponses(MethodInfo element, NamingStrategy namingStrategy, VisitorCollection collection)
        {
            var responsesWithBody = element.GetCustomAttributes<OpenApiResponseWithBodyAttribute>(inherit: false)
                                    .Select(p => new { StatusCode = p.StatusCode, Response = p.ToOpenApiResponse(namingStrategy) });

            var responsesWithoutBody = element.GetCustomAttributes<OpenApiResponseWithoutBodyAttribute>(inherit: false)
                                       .Select(p => new { StatusCode = p.StatusCode, Response = p.ToOpenApiResponse(namingStrategy) });

            var responses = responsesWithBody.Concat(responsesWithoutBody)
                                             .ToDictionary(p => ((int)p.StatusCode).ToString(), p => p.Response)
                                             .ToOpenApiResponses();

            return responses;
        }

        /// <inheritdoc />
        public Dictionary<string, OpenApiSchema> GetOpenApiSchemas(List<MethodInfo> elements, NamingStrategy namingStrategy, VisitorCollection collection)
        {
            var requests = elements.SelectMany(p => p.GetCustomAttributes<OpenApiRequestBodyAttribute>(inherit: false))
                                   .Select(p => p.BodyType);
            var responses = elements.SelectMany(p => p.GetCustomAttributes<OpenApiResponseWithBodyAttribute>(inherit: false))
                                    .Select(p => p.BodyType);
            var types = requests.Union(responses)
                                .Select(p => p.IsOpenApiArray() || p.IsOpenApiDictionary() ? p.GetOpenApiSubType() : p)
                                .Distinct()
                                .Where(p => !p.IsSimpleType())
                                .Where(p => p != typeof(JObject))
                                .Where(p => p != typeof(JToken))
                                .Where(p => !typeof(Array).IsAssignableFrom(p))
                                .ToList();

            var rootSchemas = new Dictionary<string, OpenApiSchema>();
            var schemas = new Dictionary<string, OpenApiSchema>();

            this._acceptor.Types = types.ToDictionary(p => p.GetOpenApiTypeName(namingStrategy), p => p);
            this._acceptor.RootSchemas = rootSchemas;
            this._acceptor.Schemas = schemas;

            this._acceptor.Accept(collection, namingStrategy);

            var union = schemas.Concat(rootSchemas.Where(p => !schemas.Keys.Contains(p.Key)))
                               .Distinct()
                               .OrderBy(p => p.Key)
                               .ToDictionary(p => p.Key,
                                             p =>
                                             {
                                                 // Title was intentionally added for schema key.
                                                 // It's not necessary when it's added to the root schema.
                                                 // Therefore, it's removed.
                                                 p.Value.Title = null;
                                                 return p.Value;
                                             });

            return union;
        }

        /// <inheritdoc />
        public Dictionary<string, OpenApiSecurityScheme> GetOpenApiSecuritySchemes()
        {
            var scheme = new OpenApiSecurityScheme()
            {
                Name = "x-functions-key",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header
            };
            var schemes = new Dictionary<string, OpenApiSecurityScheme>()
                              {
                                  { "authKey", scheme }
                              };

            return schemes;
        }

        private string FilterRoute(string route)
        {
            var segments = route.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(p => this._filter.Filter.Replace(p, this._filter.Replacement));

            return string.Join("/", segments);
        }
    }
}
