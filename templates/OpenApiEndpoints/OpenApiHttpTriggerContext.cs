using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;

using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi
{
    /// <summary>
    /// This represents the context entity for <see cref="OpenApiHttpTrigger"/>.
    /// </summary>
    [SuppressMessage("Design", "CA1823", Justification = "")]
    [SuppressMessage("Design", "MEN002", Justification = "")]
    [SuppressMessage("Design", "SA1206", Justification = "")]
    [SuppressMessage("Layout Rules", "SA1311", Justification = "")]
    [SuppressMessage("Layout Rules", "SA1500", Justification = "")]
    [SuppressMessage("Readability Rules", "SX1101", Justification = "")]
    public class OpenApiHttpTriggerContext : IOpenApiHttpTriggerContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiHttpTrigger"/> class.
        /// </summary>
        public OpenApiHttpTriggerContext()
        {
            var host = HostJsonResolver.Resolve();

            this.OpenApiConfiguration = OpenApiConfigurationResolver.Resolve(this.GetExecutingAssembly());
            this.OpenApiCustomUIOptions = OpenApiCustomUIResolver.Resolve(this.GetExecutingAssembly());
            this.HttpSettings = host.GetHttpSettings();

            var filter = new RouteConstraintFilter();
            var acceptor = new OpenApiSchemaAcceptor();
            var helper = new DocumentHelper(filter, acceptor);

            this.Document = new Document(helper);
            this.SwaggerUI = new SwaggerUI();
        }

        /// <inheritdoc />
        public virtual IOpenApiConfigurationOptions OpenApiConfiguration { get; }

        /// <inheritdoc />
        public virtual IOpenApiCustomUIOptions OpenApiCustomUIOptions { get; }

        /// <inheritdoc />
        public virtual HttpSettings HttpSettings { get; }

        /// <inheritdoc />
        public virtual IDocument Document { get; }

        /// <inheritdoc />
        public virtual ISwaggerUI SwaggerUI { get; }

        /// <inheritdoc />
        public virtual NamingStrategy NamingStrategy { get; } = new CamelCaseNamingStrategy();

        /// <inheritdoc />
        public virtual Assembly GetExecutingAssembly()
        {
            return Assembly.GetExecutingAssembly();
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
        public virtual string GetSwaggerAuthKey(string key = "OpenApi__ApiKey")
        {
            var value = Environment.GetEnvironmentVariable(key);

            return value ?? string.Empty;
        }
    }
}
