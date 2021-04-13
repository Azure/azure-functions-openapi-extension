using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;
using Microsoft.OpenApi;

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
        /// Initializes a new instance of the <see cref="OpenApiTriggerFunctionProvider"/> class.
        /// </summary>
        public OpenApiHttpTriggerContext()
        {
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
        public virtual IOpenApiHttpTriggerContext SetApplicationAssembly(string functionAppDirectory, bool appendBin = true)
        {
            if (!this._dllpath.IsNullOrWhiteSpace())
            {
                return this;
            }

            var file = Directory.GetFiles(functionAppDirectory, "*.deps.json", SearchOption.TopDirectoryOnly).FirstOrDefault();
            if (file.IsNullOrWhiteSpace())
            {
                throw new InvalidOperationException("Invalid function app directory");
            }

            var pattern = functionAppDirectory;
            var replacement = $"{functionAppDirectory.TrimEnd(Path.DirectorySeparatorChar)}";
            if (appendBin)
            {
                replacement += $"{Path.DirectorySeparatorChar}bin";
            }

            var dllpath = file.Replace(pattern, replacement)
                              .Replace("deps.json", "dll");

            this._dllpath = dllpath;

            return this;
        }

        /// <inheritdoc />
        public virtual VisitorCollection GetVisitorCollection()
        {
            var collection = VisitorCollection.CreateInstance();

            return collection;
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
        public virtual AuthorizationLevel GetDocumentAuthLevel(string key = "OpenApi__AuthLevel__Document")
        {
            var value = Environment.GetEnvironmentVariable(key);
            var parsed = Enum.TryParse<AuthorizationLevel>(value, out var result) ? result : AuthorizationLevel.Anonymous;

            return parsed;
        }

        /// <inheritdoc />
        public virtual AuthorizationLevel GetUIAuthLevel(string key = "OpenApi__AuthLevel__UI")
        {
            var value = Environment.GetEnvironmentVariable(key);
            var parsed = Enum.TryParse<AuthorizationLevel>(value, out var result) ? result : AuthorizationLevel.Anonymous;

            return parsed;
        }

        /// <inheritdoc />
        public virtual string GetSwaggerAuthKey(string key = "OpenApi__ApiKey")
        {
            var value = Environment.GetEnvironmentVariable(key);

            return value ?? string.Empty;
        }

        /// <inheritdoc />
        private Assembly GetAssembly(object instance)
        {
            return this.GetAssembly(instance.GetType());
        }

        /// <inheritdoc />
        private Assembly GetAssembly<T>()
        {
            return this.GetAssembly(typeof(T));
        }

        /// <inheritdoc />
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
