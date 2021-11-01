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
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core
{
    /// <summary>
    /// This represents the helper entity for the classes implementing the <see cref="IDocument"/> interface.
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
        public OpenApiPathItem GetOpenApiPath(string path, OpenApiPaths paths)
        {
            var item = paths.ContainsKey(path) ? paths[path] : new OpenApiPathItem();

            return item;
        }

        /// <inheritdoc />
        public List<OpenApiSecurityRequirement> GetOpenApiSecurityRequirement(MethodInfo element, NamingStrategy namingStrategy = null)
        {
            var attributes = element.GetCustomAttributes<OpenApiSecurityAttribute>(inherit: false);
            if (!attributes.Any())
            {
                return new List<OpenApiSecurityRequirement>();
            }

            var requirements = new List<OpenApiSecurityRequirement>();
            foreach (var attr in attributes)
            {
                var scheme = new OpenApiSecurityScheme()
                {
                    Type = attr.SchemeType,
                    Description = attr.Description,
                    Name = GetSecuritySchemeName(attr),
                    In = GetSecuritySchemeLocation(attr),
                    Scheme = GetSecuritySchemeScheme(attr, namingStrategy),
                    BearerFormat = GetSecurityBearerFormat(attr),
                    Flows = GetSecurityOAuthFlows(attr),
                    OpenIdConnectUrl = GetSecurityOpenIdConnectUrl(attr),
                    Reference = GetSecurityReference(attr),
                };

                var value = GetSecurityOAuthScopes(attr, scheme.Flows);

                var requirement = new OpenApiSecurityRequirement();
                requirement.Add(scheme, value);

                requirements.Add(requirement);
            }

            return requirements;
        }

        /// <inheritdoc />
        public OpenApiRequestBody GetOpenApiRequestBody(MethodInfo element, NamingStrategy namingStrategy, VisitorCollection collection, OpenApiVersionType version = OpenApiVersionType.V2)
        {
            var attributes = element.GetCustomAttributes<OpenApiRequestBodyAttribute>(inherit: false);
            if (!attributes.Any())
            {
                return null;
            }

            var contents = attributes.Where(p => p.Deprecated == false)
                                     .ToDictionary(p => p.ContentType, p => p.ToOpenApiMediaType(namingStrategy, collection, version));

            if (contents.Any())
            {
                return new OpenApiRequestBody()
                {
                    Content = contents,
                    Required = attributes.First().Required,
                    Description = attributes.First().Description
                };
            }

            return null;
        }

        /// <inheritdoc />
        [Obsolete("This method is obsolete from 2.0.0. Use GetOpenApiResponses instead", error: true)]
        public OpenApiResponses GetOpenApiResponseBody(MethodInfo element, NamingStrategy namingStrategy = null)
        {
            return this.GetOpenApiResponses(element, namingStrategy, null);
        }

        /// <inheritdoc />
        public OpenApiResponses GetOpenApiResponses(MethodInfo element, NamingStrategy namingStrategy, VisitorCollection collection, OpenApiVersionType version = OpenApiVersionType.V2)
        {
            var responsesWithBody = element.GetCustomAttributes<OpenApiResponseWithBodyAttribute>(inherit: false)
                                           .Where(p => p.Deprecated == false)
                                           .Select(p => new { StatusCode = p.StatusCode, Response = p.ToOpenApiResponse(namingStrategy, version: version) });

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
                                .Where(p => p.IsReferentialType())
                                .Where(p => !typeof(Array).IsAssignableFrom(p))
                                .ToList();

            var rootSchemas = new Dictionary<string, OpenApiSchema>();
            var schemas = new Dictionary<string, OpenApiSchema>();

            this._acceptor.Types = types.ToDictionary(p => p.GetOpenApiReferenceId(p.IsOpenApiDictionary(), p.IsOpenApiArray(), namingStrategy), p => p);
            this._acceptor.RootSchemas = rootSchemas;
            this._acceptor.Schemas = schemas;

            this._acceptor.Accept(collection, namingStrategy);

            var union = schemas.Concat(rootSchemas.Where(p => !schemas.Keys.Contains(p.Key)))
                               .Distinct()
                               .Where(p => p.Key.ToUpperInvariant() != "OBJECT")
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
        [Obsolete("This method is obsolete from 3.2.0. Use GetOpenApiSecuritySchemes(List<MethodInfo> elements, NamingStrategy namingStrategy = null) instead", error: true)]
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

        /// <inheritdoc />
        public Dictionary<string, OpenApiSecurityScheme> GetOpenApiSecuritySchemes(List<MethodInfo> elements, NamingStrategy namingStrategy = null)
        {
            var schemes = new Dictionary<string, OpenApiSecurityScheme>();
            foreach (var element in elements)
            {
                var attributes = element.GetCustomAttributes<OpenApiSecurityAttribute>(inherit: false);
                if (!attributes.Any())
                {
                    continue;
                }

                foreach (var attr in attributes)
                {
                    if (schemes.ContainsKey(attr.SchemeName))
                    {
                        continue;
                    }

                    var scheme = new OpenApiSecurityScheme()
                    {
                        Type = attr.SchemeType,
                        Description = attr.Description,
                        Name = GetSecuritySchemeName(attr),
                        In = GetSecuritySchemeLocation(attr),
                        Scheme = GetSecuritySchemeScheme(attr, namingStrategy),
                        BearerFormat = GetSecurityBearerFormat(attr),
                        Flows = GetSecurityOAuthFlows(attr),
                        OpenIdConnectUrl = GetSecurityOpenIdConnectUrl(attr),
                        Reference = GetSecurityReference(attr),
                    };

                    schemes.Add(attr.SchemeName, scheme);
                }
            }

            return schemes;
        }

        /// <inheritdoc />
        public string FilterRouteConstraints(string route)
        {
            var segments = route.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(p => this._filter.Filter.Replace(p, this._filter.Replacement));

            return string.Join("/", segments);
        }

        private static string GetSecuritySchemeName(OpenApiSecurityAttribute attr)
        {
            if (attr.SchemeType != SecuritySchemeType.ApiKey)
            {
                return null;
            }

            if (attr.Name.IsNullOrWhiteSpace())
            {
                throw new InvalidOperationException("Scheme name MUST be provided");
            }

            return attr.Name;
        }

        private static ParameterLocation GetSecuritySchemeLocation(OpenApiSecurityAttribute attr)
        {
            if (attr.SchemeType != SecuritySchemeType.ApiKey)
            {
                return ParameterLocation.Query;
            }

            return attr.In == OpenApiSecurityLocationType.None ? ParameterLocation.Query : (ParameterLocation)((int)attr.In - 1);
        }

        private static string GetSecuritySchemeScheme(OpenApiSecurityAttribute attr, NamingStrategy namingStrategy = null)
        {
            if (attr.SchemeType != SecuritySchemeType.Http)
            {
                return null;
            }

            if (attr.Scheme == OpenApiSecuritySchemeType.None)
            {
                throw new InvalidOperationException("Scheme MUST be provided");
            }

            return attr.Scheme.ToDisplayName(namingStrategy);
        }

        private static string GetSecurityBearerFormat(OpenApiSecurityAttribute attr)
        {
            if (attr.SchemeType != SecuritySchemeType.Http)
            {
                return null;
            }

            if (attr.Scheme != OpenApiSecuritySchemeType.Bearer)
            {
                return null;
            }

            if (attr.BearerFormat.IsNullOrWhiteSpace())
            {
                throw new InvalidOperationException("Bearer format MUST be provided");
            }

            return attr.BearerFormat;
        }

        private static OpenApiOAuthFlows GetSecurityOAuthFlows(OpenApiSecurityAttribute attr)
        {
            if (attr.SchemeType != SecuritySchemeType.OAuth2)
            {
                return null;
            }

            if (attr.Flows.IsNullOrDefault())
            {
                throw new InvalidOperationException("OAuth flows MUST be provided");
            }

            return (OpenApiOAuthFlows)Activator.CreateInstance(attr.Flows);
        }

        private static Uri GetSecurityOpenIdConnectUrl(OpenApiSecurityAttribute attr)
        {
            if (attr.SchemeType != SecuritySchemeType.OpenIdConnect)
            {
                return null;
            }

            if (attr.OpenIdConnectUrl.IsNullOrWhiteSpace())
            {
                throw new InvalidOperationException("OpenId Connect URL flows MUST be provided");
            }

            return new Uri(attr.OpenIdConnectUrl);
        }

        private static List<string> GetSecurityOAuthScopes(OpenApiSecurityAttribute attr, OpenApiOAuthFlows flows)
        {
            var value = new List<string>();
            if (attr.SchemeType == SecuritySchemeType.ApiKey)
            {
                return value;
            }

            if (attr.SchemeType == SecuritySchemeType.Http)
            {
                return value;
            }

            if (attr.SchemeType == SecuritySchemeType.OAuth2)
            {
                if (flows.Implicit.IsNullOrDefault() && flows.Password.IsNullOrDefault() && flows.ClientCredentials.IsNullOrDefault() && flows.AuthorizationCode.IsNullOrDefault())
                {
                    throw new InvalidOperationException("Flow MUST be provided");
                }

                if (flows.Implicit?.Scopes?.Keys.Any() == true)
                {
                    value.AddRange(flows.Implicit?.Scopes?.Keys);
                }

                if (flows.Password?.Scopes?.Keys.Any() == true)
                {
                    value.AddRange(flows.Password?.Scopes?.Keys);
                }

                if (flows.ClientCredentials?.Scopes?.Keys.Any() == true)
                {
                    value.AddRange(flows.ClientCredentials?.Scopes?.Keys);
                }

                if (flows.AuthorizationCode?.Scopes?.Keys.Any() == true)
                {
                    value.AddRange(flows.AuthorizationCode?.Scopes?.Keys);
                }
            }

            if (attr.SchemeType == SecuritySchemeType.OpenIdConnect)
            {
                if (!attr.OpenIdConnectScopes.Any())
                {
                    throw new InvalidOperationException("Scope MUST be provided");
                }

                value.AddRange(attr.OpenIdConnectScopes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()));
            }

            return value.Distinct().ToList();
        }

        private static OpenApiReference GetSecurityReference(OpenApiSecurityAttribute attr)
        {
            var reference = new OpenApiReference() { Id = attr.SchemeName, Type = ReferenceType.SecurityScheme };

            return reference;
        }
    }
}
