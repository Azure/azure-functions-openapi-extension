using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// This represents the options entity for OpenAPI metadata configuration.
    /// </summary>
    public class DefaultOpenApiConfigurationOptions : IOpenApiConfigurationOptions
    {
        private const string OpenApiDocVersionKey = "OpenApi__DocVersion";
        private const string OpenApiDocTitleKey = "OpenApi__DocTitle";
        private const string OpenApiHostNamesKey = "OpenApi__HostNames";
        private const string OpenApiVersionKey = "OpenApi__Version";
        private const string FunctionsRuntimeEnvironmentKey = "AZURE_FUNCTIONS_ENVIRONMENT";
        private const string ExcludeRequestingHostKey = "OpenApi__ExcludeRequestingHost";
        private const string ForceHttpKey = "OpenApi__ForceHttp";
        private const string ForceHttpsKey = "OpenApi__ForceHttps";

        /// <inheritdoc />
        public virtual OpenApiInfo Info { get; set; } = new OpenApiInfo()
        {
            Version = GetOpenApiDocVersion(),
            Title = GetOpenApiDocTitle(),
        };

        /// <inheritdoc />
        public virtual List<OpenApiServer> Servers { get; set; } = GetHostNames();

        /// <inheritdoc />
        public virtual OpenApiVersionType OpenApiVersion { get; set; } = GetOpenApiVersion();

        /// <inheritdoc />
        public virtual bool ExcludeRequestingHost { get; set; } = IsRequestingHostExcluded();

        /// <inheritdoc />
        public virtual bool ForceHttp { get; set; } = IsHttpForced();

        /// <inheritdoc />
        public virtual bool ForceHttps { get; set; } = IsHttpsForced();

        /// <inheritdoc />
        public virtual List<IDocumentFilter> DocumentFilters { get; set; } = new List<IDocumentFilter>();

        /// <summary>
        /// Gets the OpenAPI document version.
        /// </summary>
        /// <returns>Returns the OpenAPI document version.</returns>
        protected static string GetOpenApiDocVersion()
        {
            var version = Environment.GetEnvironmentVariable(OpenApiDocVersionKey) ?? DefaultOpenApiDocVersion();

            return version;
        }

        /// <summary>
        /// Gets the OpenAPI document title.
        /// </summary>
        /// <returns>Returns the OpenAPI document title.</returns>
        protected static string GetOpenApiDocTitle()
        {
            var title = Environment.GetEnvironmentVariable(OpenApiDocTitleKey) ?? DefaultOpenApiDocTitle();

            return title;
        }

        /// <summary>
        /// Gets the list of hostnames.
        /// </summary>
        /// <returns>Returns the list of hostnames.</returns>
        protected static List<OpenApiServer> GetHostNames()
        {
            var servers = new List<OpenApiServer>();
            var collection = Environment.GetEnvironmentVariable(OpenApiHostNamesKey);
            if (collection.IsNullOrWhiteSpace())
            {
                return servers;
            }

            var hostnames = collection.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(p => new OpenApiServer() { Url = p });

            servers.AddRange(hostnames);

            return servers;
        }

        /// <summary>
        /// Gets the OpenAPI version.
        /// </summary>
        /// <returns>Returns the OpenAPI version.</returns>
        protected static OpenApiVersionType GetOpenApiVersion()
        {
            var version = Enum.TryParse<OpenApiVersionType>(
                              Environment.GetEnvironmentVariable(OpenApiVersionKey), ignoreCase: true, out var result)
                            ? result
                            : DefaultOpenApiVersion();

            return version;
        }

        /// <summary>
        /// Checks whether to exclude the requesting host as the server URL or not.
        /// </summary>
        /// <returns>Returns <c>True</c>, if the requesting host is excluded; otherwise returns <c>False</c></returns>
        protected static bool IsRequestingHostExcluded()
        {
            var requestingHostExcluded = bool.TryParse(Environment.GetEnvironmentVariable(ExcludeRequestingHostKey), out var result) ? result : (bool?)null;
            if (requestingHostExcluded.HasValue)
            {
                return requestingHostExcluded.Value;
            }

            var development = IsFunctionsRuntimeEnvironmentDevelopment();

            return development;
        }

        /// <summary>
        /// Checks whether HTTP is forced or not.
        /// </summary>
        /// <returns>Returns <c>True</c>, if HTTP is forced; otherwise returns <c>False</c>.</returns>
        protected static bool IsHttpForced()
        {
            var forceHttp = bool.TryParse(Environment.GetEnvironmentVariable(ForceHttpKey), out var result) ? result : false;

            return forceHttp;
        }

        /// <summary>
        /// Checks whether HTTPS is forced or not.
        /// </summary>
        /// <returns>Returns <c>True</c>, if HTTPS is forced; otherwise returns <c>False</c>.</returns>
        protected static bool IsHttpsForced()
        {
            var forceHttps = bool.TryParse(Environment.GetEnvironmentVariable(ForceHttpsKey), out var result) ? result : false;

            return forceHttps;
        }

        private static OpenApiVersionType DefaultOpenApiVersion()
        {
            return OpenApiVersionType.V2;
        }

        private static string DefaultOpenApiDocVersion()
        {
            return "1.0.0";
        }

        private static string DefaultOpenApiDocTitle()
        {
            return "OpenAPI Document on Azure Functions";
        }

        private static bool IsFunctionsRuntimeEnvironmentDevelopment()
        {
            var development = Environment.GetEnvironmentVariable(FunctionsRuntimeEnvironmentKey) == "Development";

            return development;
        }
    }
}
