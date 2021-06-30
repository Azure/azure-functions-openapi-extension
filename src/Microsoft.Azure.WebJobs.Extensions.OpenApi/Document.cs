using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi
{
    /// <summary>
    /// This represents the document entity handling OpenAPI document.
    /// </summary>
    public class Document : IDocument
    {
        private readonly IDocumentHelper _helper;

        private NamingStrategy _strategy;
        private VisitorCollection _collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        public Document(IDocumentHelper helper)
        {
            this._helper = helper.ThrowIfNullOrDefault();
        }

        /// <inheritdoc />
        public Document(OpenApiDocument openApiDocument)
        {
            this.OpenApiDocument = openApiDocument;

        }

        /// <inheritdoc />
        public OpenApiDocument OpenApiDocument { get; private set; }

        /// <inheritdoc />
        public IDocument InitialiseDocument()
        {
            this.OpenApiDocument = new OpenApiDocument()
            {
                Components = new OpenApiComponents()
            };

            return this;
        }

        /// <inheritdoc />
        public IDocument AddMetadata(OpenApiInfo info)
        {
            this.OpenApiDocument.Info = info;

            return this;
        }

        /// <inheritdoc />
        public IDocument AddServer(IHttpRequestDataObject req, string routePrefix, IOpenApiConfigurationOptions options = null)
        {
            var prefix = string.IsNullOrWhiteSpace(routePrefix) ? string.Empty : $"/{routePrefix}";
            var baseUrl = $"{req.Scheme}://{req.Host}{prefix}";

            var server = new OpenApiServer { Url = baseUrl };

            if (options.IsNullOrDefault())
            {
                this.OpenApiDocument.Servers = new List<OpenApiServer>() { server };

                return this;
            }

            // Filters out the existing base URLs that are the same as the current host URL.
            var servers = options.Servers
                                 .Where(p => p.Url.TrimEnd('/') != baseUrl.TrimEnd('/'))
                                 .ToList();
            if (!servers.Any())
            {
                servers.Insert(0, server);
            }

            if (options.IncludeRequestingHostName
                && !servers.Any(p => p.Url.TrimEnd('/') == baseUrl.TrimEnd('/')))
            {
                servers.Insert(0, server);
            }

            this.OpenApiDocument.Servers = servers;

            return this;
        }

        /// <inheritdoc />
        public IDocument AddNamingStrategy(NamingStrategy strategy)
        {
            this._strategy = strategy.ThrowIfNullOrDefault();

            return this;
        }

        /// <inheritdoc />
        public IDocument AddVisitors(VisitorCollection collection)
        {
            this._collection = collection.ThrowIfNullOrDefault();

            return this;
        }

        /// <inheritdoc />
        public IDocument Build(string assemblyPath, OpenApiVersionType version = OpenApiVersionType.V2)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);

            return this.Build(assembly, version);
        }

        /// <inheritdoc />
        public IDocument Build(Assembly assembly, OpenApiVersionType version = OpenApiVersionType.V2)
        {
            if (this._strategy.IsNullOrDefault())
            {
                this._strategy = new DefaultNamingStrategy();
            }

            var paths = new OpenApiPaths();

            var methods = this._helper.GetHttpTriggerMethods(assembly);
            foreach (var method in methods)
            {
                var trigger = this._helper.GetHttpTriggerAttribute(method);
                if (trigger.IsNullOrDefault())
                {
                    continue;
                }

                var function = this._helper.GetFunctionNameAttribute(method);
                if (function.IsNullOrDefault())
                {
                    continue;
                }

                var path = this._helper.GetHttpEndpoint(function, trigger);
                if (path.IsNullOrWhiteSpace())
                {
                    continue;
                }

                var verb = this._helper.GetHttpVerb(trigger);

                var item = this._helper.GetOpenApiPath(path, paths);
                var operations = item.Operations;

                var operation = this._helper.GetOpenApiOperation(method, function, verb);
                if (operation.IsNullOrDefault())
                {
                    continue;
                }

                operation.Security = this._helper.GetOpenApiSecurityRequirement(method, this._strategy);
                operation.Parameters = this._helper.GetOpenApiParameters(method, trigger, this._strategy, this._collection);
                operation.RequestBody = this._helper.GetOpenApiRequestBody(method, this._strategy, this._collection, version);
                operation.Responses = this._helper.GetOpenApiResponses(method, this._strategy, this._collection, version);

                operations[verb] = operation;
                item.Operations = operations;

                paths[path] = item;
            }

            this.OpenApiDocument.Paths = paths;
            this.OpenApiDocument.Components.Schemas = this._helper.GetOpenApiSchemas(methods, this._strategy, this._collection);
            this.OpenApiDocument.Components.SecuritySchemes = this._helper.GetOpenApiSecuritySchemes(methods, this._strategy);
            // this.OpenApiDocument.SecurityRequirements = this.OpenApiDocument
            //                                                 .Paths
            //                                                 .SelectMany(p => p.Value.Operations.SelectMany(q => q.Value.Security))
            //                                                 .Where(p => !p.IsNullOrDefault())
            //                                                 .Distinct(new OpenApiSecurityRequirementComparer())
            //                                                 .ToList();

            return this;
        }

        /// <inheritdoc />
        public async Task<string> RenderAsync(OpenApiSpecVersion version, OpenApiFormat format)
        {
            var result = await Task.Factory
                                   .StartNew(() => this.Render(version, format))
                                   .ConfigureAwait(false);

            return result;
        }

        private string Render(OpenApiSpecVersion version, OpenApiFormat format)
        {
            using (var sw = new StringWriter())
            {
                this.OpenApiDocument.Serialise(sw, version, format);

                return sw.ToString();
            }
        }
    }
}
