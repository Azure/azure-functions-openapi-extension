using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Filters;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using YamlDotNet.Serialization;

using GenericExtensions = Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions.GenericExtensions;
using HttpRequestDataObjectExtensions = Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions.HttpRequestDataObjectExtensions;
using OpenApiDocumentExtensions = Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions.OpenApiDocumentExtensions;
using StringExtensions = Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions.StringExtensions;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi
{
    /// <summary>
    /// This represents the document entity handling OpenAPI document.
    /// </summary>
    public class Document : IDocument
    {
        private readonly IDocumentHelper _helper;

        private NamingStrategy _strategy;
        private VisitorCollection _collection;
        private IHttpRequestDataObject _req;

        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        public Document(IDocumentHelper helper)
        {
            this._helper = GenericExtensions.ThrowIfNullOrDefault(helper);
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
            this._req = req;

            var prefix = string.IsNullOrWhiteSpace(routePrefix) ? string.Empty : $"/{routePrefix}";
            var baseUrl = $"{HttpRequestDataObjectExtensions.GetScheme(this._req, options)}://{this._req.Host}{prefix}";

            var server = new OpenApiServer { Url = baseUrl };

            if (GenericExtensions.IsNullOrDefault(options))
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
            this._strategy = GenericExtensions.ThrowIfNullOrDefault(strategy);

            return this;
        }

        /// <inheritdoc />
        public IDocument AddVisitors(VisitorCollection collection)
        {
            this._collection = GenericExtensions.ThrowIfNullOrDefault(collection);

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
            if (GenericExtensions.IsNullOrDefault(this._strategy))
            {
                this._strategy = new DefaultNamingStrategy();
            }

            var paths = new OpenApiPaths();

            var tags = StringExtensions.ToArray(this._req.Query["tag"], ",");
            var methods = this._helper.GetHttpTriggerMethods(assembly, tags);
            foreach (var method in methods)
            {
                var trigger = this._helper.GetHttpTriggerAttribute(method);
                if (GenericExtensions.IsNullOrDefault(trigger))
                {
                    continue;
                }

                var function = this._helper.GetFunctionNameAttribute(method);
                if (GenericExtensions.IsNullOrDefault(function))
                {
                    continue;
                }

                var path = this._helper.GetHttpEndpoint(function, trigger);
                if (StringExtensions.IsNullOrWhiteSpace(path))
                {
                    continue;
                }

                var verb = this._helper.GetHttpVerb(trigger);

                var item = this._helper.GetOpenApiPath(path, paths);
                var operations = item.Operations;

                var operation = this._helper.GetOpenApiOperation(method, function, verb);
                if (GenericExtensions.IsNullOrDefault(operation))
                {
                    continue;
                }

                operation.Security = this._helper.GetOpenApiSecurityRequirement(method, this._strategy);
                operation.Parameters = this._helper.GetOpenApiParameters(method, trigger, this._strategy, this._collection, version);
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
        public IDocument ApplyDocumentFilters(DocumentFilterCollection collection)
        {
            foreach (var filter in GenericExtensions.ThrowIfNullOrDefault(collection).DocumentFilters)
            {
                filter.Apply(this._req, this.OpenApiDocument);
            }

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
            //var serialised = default(string);
            //using (var sw = new StringWriter())
            //{
            //    OpenApiDocumentExtensions.Serialise(this.OpenApiDocument, sw, version, format);
            //    serialised = sw.ToString();
            //}

            //return serialised;

            // This is the interim solution to resolve:
            // https://github.com/Azure/azure-functions-openapi-extension/issues/365
            //
            // It will be removed when the following issue is resolved:
            // https://github.com/microsoft/OpenAPI.NET/issues/747
            var jserialised = default(string);
            using (var sw = new StringWriter())
            {
                OpenApiDocumentExtensions.Serialise(this.OpenApiDocument, sw, version, OpenApiFormat.Json);
                jserialised = sw.ToString();
            }

            var yserialised = default(string);
            using (var sw = new StringWriter())
            {
                OpenApiDocumentExtensions.Serialise(this.OpenApiDocument, sw, version, OpenApiFormat.Yaml);
                yserialised = sw.ToString();
            }

            if (version != OpenApiSpecVersion.OpenApi2_0)
            {
                return format == OpenApiFormat.Json ? jserialised : yserialised;
            }

            var jo = JsonConvert.DeserializeObject<JObject>(jserialised);
            var jts = jo.DescendantsAndSelf()
                        .Where(p => p.Type == JTokenType.Property && (p as JProperty).Name == "parameters")
                        .SelectMany(p => p.Values<JArray>().SelectMany(q => q.Children<JObject>()))
                        .Where(p => p.Value<string>("in") == null)
                        .Where(p => p.Value<string>("description") != null)
                        .Where(p => p.Value<string>("description").Contains("[formData]"))
                        .ToList();
            foreach (var jt in jts)
            {
                jt["in"] = "formData";
                jt["description"] = jt.Value<string>("description").Replace("[formData]", string.Empty);
            }

            var serialised = JsonConvert.SerializeObject(jo, Formatting.Indented);
            if (format == OpenApiFormat.Json)
            {
                return serialised;
            }

            var converter = new ExpandoObjectConverter();
            var deserialised = JsonConvert.DeserializeObject<ExpandoObject>(serialised, converter);
            serialised = new SerializerBuilder().Build().Serialize(deserialised);

            return serialised;
        }
    }
}
