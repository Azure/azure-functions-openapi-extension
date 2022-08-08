using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Filters;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;
using Microsoft.OpenApi;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi
{
    /// <summary>
    /// This represents the context entity for <see cref="OpenApiTriggerFunctionProvider"/>.
    /// </summary>
    [SuppressMessage("Design", "CA1823", Justification = "")]
    [SuppressMessage("Design", "MEN002", Justification = "")]
    [SuppressMessage("Design", "SA1206", Justification = "")]
    [SuppressMessage("Layout Rules", "SA1311", Justification = "")]
    [SuppressMessage("Layout Rules", "SA1500", Justification = "")]
    [SuppressMessage("Readability Rules", "SX1101", Justification = "")]
    public class OpenApiHttpTriggerContext : IOpenApiHttpTriggerContext
    {
        private string _dllpath;
        private Assembly _appAssembly;
        private IOpenApiConfigurationOptions _configOptions;
        private IOpenApiCustomUIOptions _uiOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiHttpTriggerContext"/> class.
        /// </summary>
        /// <param name="configOptions"><see cref="IOpenApiConfigurationOptions"/> instance.</param>
        public OpenApiHttpTriggerContext(IOpenApiConfigurationOptions configOptions = null)
        {
            this._configOptions = configOptions;
            this.PackageAssembly = this.GetAssembly<ISwaggerUI>();

            var host = HostJsonResolver.Resolve();
            this.HttpSettings = host.GetHttpSettings();

            var filter = new RouteConstraintFilter();
            var acceptor = new OpenApiSchemaAcceptor();
            var helper = new DocumentHelper(filter, acceptor);

            this.Document = new Document(helper);
            this.SwaggerUI = new SwaggerUI();
        }

        /// <inheritdoc />
        public virtual Assembly ApplicationAssembly
        {
            get
            {
                if (this._appAssembly.IsNullOrDefault())
                {
                    this._appAssembly = this.GetAssembly(this._dllpath);
                }

                return this._appAssembly;
            }
        }

        /// <inheritdoc />
        public virtual Assembly PackageAssembly { get; }

        /// <inheritdoc />
        public virtual IOpenApiConfigurationOptions OpenApiConfigurationOptions
        {
            get
            {
                if (this._configOptions.IsNullOrDefault())
                {
                    this._configOptions = OpenApiConfigurationResolver.Resolve(this.ApplicationAssembly);
                }

                return this._configOptions;
            }
        }

        /// <inheritdoc />
        public virtual IOpenApiCustomUIOptions OpenApiCustomUIOptions
        {
            get
            {
                if (this._uiOptions.IsNullOrDefault())
                {
                    this._uiOptions = OpenApiCustomUIResolver.Resolve(this.ApplicationAssembly);
                }

                return this._uiOptions;
            }
        }

        /// <inheritdoc />
        public virtual HttpSettings HttpSettings { get; }

        /// <inheritdoc />
        public virtual IDocument Document { get; }

        /// <inheritdoc />
        public virtual ISwaggerUI SwaggerUI { get; }

        /// <inheritdoc />
        public virtual NamingStrategy NamingStrategy { get; } = new CamelCaseNamingStrategy();

        /// <inheritdoc />
        public virtual bool IsDevelopment { get; } = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Development";

        /// <inheritdoc />
        [Obsolete("This method is obsolete. Use GetAssembly<T>() or GetAssembly(object) instead", error: true)]
        public virtual Assembly GetExecutingAssembly()
        {
            var assembly = Assembly.GetExecutingAssembly();

            return assembly;
        }

        /// <inheritdoc />
        public virtual async Task<IOpenApiHttpTriggerContext> SetApplicationAssemblyAsync(string functionAppDirectory, bool appendBin = true)
        {
            if (!this._dllpath.IsNullOrWhiteSpace())
            {
                return this;
            }

            var runtimepath = this.GetRuntimePath(functionAppDirectory, appendBin);
            var runtimename = await this.GetRuntimeFilenameAsync(functionAppDirectory);
            var dllpath = $"{runtimepath}{Path.DirectorySeparatorChar}{runtimename}";

            this._dllpath = dllpath;

            return this;
        }

        /// <inheritdoc />
        public virtual async Task<OpenApiAuthorizationResult> AuthorizeAsync(IHttpRequestDataObject req)
        {
            var result = default(OpenApiAuthorizationResult);
            var type = this.ApplicationAssembly
                           .GetLoadableTypes()
                           .SingleOrDefault(p => p.HasInterface<IOpenApiHttpTriggerAuthorization>());
            if (type.IsNullOrDefault())
            {
                return result;
            }

            var auth = Activator.CreateInstance(type) as IOpenApiHttpTriggerAuthorization;
            result = await auth.AuthorizeAsync(req).ConfigureAwait(false);

            return result;
        }

        /// <inheritdoc />
        public virtual VisitorCollection GetVisitorCollection()
        {
            var collection = VisitorCollection.CreateInstance();

            return collection;
        }

        /// <inheritdoc />
        public virtual OpenApiVersionType GetOpenApiVersionType(string version = "v2")
        {
            var parsed = Enum.TryParse(version, true, out OpenApiVersionType output)
                             ? output
                             : throw new InvalidOperationException("Invalid OpenAPI version");

            return parsed;
        }

        /// <inheritdoc />
        public virtual OpenApiSpecVersion GetOpenApiSpecVersion(string version = "v2")
        {
            var parsed = Enum.TryParse(version, true, out OpenApiVersionType output)
                             ? output
                             : throw new InvalidOperationException("Invalid OpenAPI version");

            return this.GetOpenApiSpecVersion(parsed);
        }

        /// <inheritdoc />
        public virtual OpenApiSpecVersion GetOpenApiSpecVersion(OpenApiVersionType version = OpenApiVersionType.V2)
        {
            return version.ToOpenApiSpecVersion();
        }

        /// <inheritdoc />
        public virtual OpenApiFormat GetOpenApiFormat(string format = "json")
        {
            if (format.Equals("yml", StringComparison.InvariantCultureIgnoreCase))
            {
                format = "yaml";
            }

            var parsed = Enum.TryParse(format, true, out OpenApiFormatType output)
                             ? output
                             : throw new InvalidOperationException("Invalid OpenAPI format");

            return this.GetOpenApiFormat(parsed);
        }

        /// <inheritdoc />
        public virtual OpenApiFormat GetOpenApiFormat(OpenApiFormatType format = OpenApiFormatType.Json)
        {
            return format.ToOpenApiFormat();
        }

        /// <inheritdoc />
        public virtual OpenApiAuthLevelType GetDocumentAuthLevel(string key = "OpenApi__AuthLevel__Document")
        {
            var value = Environment.GetEnvironmentVariable(key);
            var parsed = Enum.TryParse<OpenApiAuthLevelType>(value, out var result) ? result : OpenApiAuthLevelType.Anonymous;

            return parsed;
        }

        /// <inheritdoc />
        public virtual OpenApiAuthLevelType GetUIAuthLevel(string key = "OpenApi__AuthLevel__UI")
        {
            var value = Environment.GetEnvironmentVariable(key);
            var parsed = Enum.TryParse<OpenApiAuthLevelType>(value, out var result) ? result : OpenApiAuthLevelType.Anonymous;

            return parsed;
        }

        /// <inheritdoc />
        public virtual string GetSwaggerAuthKey(string key = "OpenApi__ApiKey")
        {
            var value = Environment.GetEnvironmentVariable(key);

            return value ?? string.Empty;
        }

        /// <inheritdoc />
        public virtual DocumentFilterCollection GetDocumentFilterCollection()
        {
            var collection = new DocumentFilterCollection(this.OpenApiConfigurationOptions.DocumentFilters);

            return collection;
        }

        private string GetRuntimePath(string functionAppDirectory, bool appendBin)
        {
            var path = functionAppDirectory;
            if (appendBin)
            {
                path += $"{Path.DirectorySeparatorChar}bin";
            }

            return path;
        }

        // **NOTE**:
        // This method relies on the dependency manifest file to find the function app runtime dll file.
        // It can be either <your_runtime>.deps.json or function.deps.json. In most cases, at least the
        // function.deps.json should exist, but in case no manifest exists, it will throw the exception.
        // In case there are multiple .deps.json files, the root project will be picked, based on the
        // dependencies mentioned in the .deps.json files.
        private async Task<string> GetRuntimeFilenameAsync(string functionAppDirectory)
        {
            var files = Directory.GetFiles(functionAppDirectory, "*.deps.json", SearchOption.AllDirectories);
            if (!files.Any())
            {
                throw new InvalidOperationException("Invalid function app directory");
            }

            var dependencyManifests = new List<DependencyManifest>();
            foreach (var file in files)
            {
                dependencyManifests.Add(await GetDependencyManifestAsync(file));
            }

            var runtimes = dependencyManifests
                .Select(manifest => manifest.Targets[manifest.RuntimeTarget.Name].First())
                .Where(manifest => manifest.Value.Dependencies != null)
                .Select(target => new
                {
                    Name = target.Key.Split('/').First(),
                    FileName = target.Value.Runtime.First().Key,
                    Dependencies = target.Value.Dependencies.Keys
                });

            var referencedRuntimes = runtimes.SelectMany(d => d.Dependencies);
            return runtimes.FirstOrDefault(r => !referencedRuntimes.Contains(r.Name))?.FileName;
        }

        private static async Task<DependencyManifest> GetDependencyManifestAsync(string file)
        {
            var serialised = default(string);
            using (var reader = File.OpenText(file))
            {
                serialised = await reader.ReadToEndAsync();
            }

            return JsonConvert.DeserializeObject<DependencyManifest>(serialised);
        }

        private Assembly GetAssembly(object instance)
        {
            return this.GetAssembly(instance.GetType());
        }

        private Assembly GetAssembly<T>()
        {
            return this.GetAssembly(typeof(T));
        }

        private Assembly GetAssembly(Type type)
        {
            var assembly = type.Assembly;

            return assembly;
        }

        private Assembly GetAssembly(string dllpath)
        {
            var assembly = Assembly.LoadFile(dllpath);

            return assembly;
        }
    }
}
