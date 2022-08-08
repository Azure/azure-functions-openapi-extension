using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// This represents the options entity for OpenAPI metadata configuration.
    /// </summary>
    [OpenApiConfigurationOptionsIgnore]
    public class DefaultOpenApiConfigurationOptions : OpenApiConfigurationOptions
    {
        private const string OpenApiDocVersionKey = "OpenApi__DocVersion";
        private const string OpenApiDocTitleKey = "OpenApi__DocTitle";
        private const string OpenApiDocDescriptionKey = "OpenApi__DocDescription";
        private const string OpenApiHostNamesKey = "OpenApi__HostNames";
        private const string OpenApiVersionKey = "OpenApi__Version";
        private const string FunctionsRuntimeEnvironmentKey = "AZURE_FUNCTIONS_ENVIRONMENT";
        private const string ForceHttpKey = "OpenApi__ForceHttp";
        private const string ForceHttpsKey = "OpenApi__ForceHttps";

        /// <inheritdoc />
        public override OpenApiInfo Info { get; set; } = new OpenApiInfo()
        {
            Version = GetOpenApiDocVersion(),
            Title = GetOpenApiDocTitle(),
            Description = GetOpenApiDocDescription(),
        };

        /// <inheritdoc />
        public override List<OpenApiServer> Servers { get; set; } = GetHostNames();

        /// <inheritdoc />
        public override OpenApiVersionType OpenApiVersion { get; set; } = GetOpenApiVersion();

        /// <inheritdoc />
        public override bool IncludeRequestingHostName { get; set; } = IsFunctionsRuntimeEnvironmentDevelopment();

        /// <inheritdoc />
        public override bool ForceHttp { get; set; } = IsHttpForced();

        /// <inheritdoc />
        public override bool ForceHttps { get; set; } = IsHttpsForced();

        /// <inheritdoc />
        public override List<IDocumentFilter> DocumentFilters { get; set; } = new List<IDocumentFilter>();

        /// <summary>
        /// Gets the OpenAPI document version.
        /// </summary>
        /// <returns>Returns the OpenAPI document version.</returns>
        public static string GetOpenApiDocVersion()
        {
            var version = Environment.GetEnvironmentVariable(OpenApiDocVersionKey) ?? DefaultOpenApiDocVersion();

            return version;
        }

        /// <summary>
        /// Gets the OpenAPI document title.
        /// </summary>
        /// <returns>Returns the OpenAPI document title.</returns>
        public static string GetOpenApiDocTitle()
        {
            var title = Environment.GetEnvironmentVariable(OpenApiDocTitleKey) ?? DefaultOpenApiDocTitle();

            return title;
        }

        /// <summary>
        /// Gets the OpenAPI document description.
        /// </summary>
        /// <returns>Returns the OpenAPI document description.</returns>
        public static string GetOpenApiDocDescription()
        {
            var description = Environment.GetEnvironmentVariable(OpenApiDocDescriptionKey) ?? DefaultOpenApiDocDescription();

            return description;
        }

        /// <summary>
        /// Gets the list of hostnames.
        /// </summary>
        /// <returns>Returns the list of hostnames.</returns>
        public static List<OpenApiServer> GetHostNames()
        {
            var servers = new List<OpenApiServer>();
            var collection = Environment.GetEnvironmentVariable(OpenApiHostNamesKey);
            if (collection.IsNullOrWhiteSpace())
            {
                return servers;
            }

            var hostnames = collection.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                      .Where(h => !string.IsNullOrWhiteSpace(h))
                                      .Select(p => new OpenApiServer() { Url = p.Trim() });

            servers.AddRange(hostnames);

            return servers;
        }

        /// <summary>
        /// Gets the OpenAPI version.
        /// </summary>
        /// <returns>Returns the OpenAPI version.</returns>
        public static OpenApiVersionType GetOpenApiVersion()
        {
            var version = Enum.TryParse<OpenApiVersionType>(
                              Environment.GetEnvironmentVariable(OpenApiVersionKey), ignoreCase: true, out var result)
                            ? result
                            : DefaultOpenApiVersion();

            return version;
        }

        /// <summary>
        /// Checks whether the current Azure Functions runtime environment is "Development" or not.
        /// </summary>
        /// <returns>Returns <c>True</c>, if the current Azure Functions runtime environment is "Development"; otherwise returns <c>False</c></returns>
        public static bool IsFunctionsRuntimeEnvironmentDevelopment()
        {
            var development = Environment.GetEnvironmentVariable(FunctionsRuntimeEnvironmentKey) == "Development";

            return development;
        }

        /// <summary>
        /// Checks whether HTTP is forced or not.
        /// </summary>
        /// <returns>Returns <c>True</c>, if HTTP is forced; otherwise returns <c>False</c>.</returns>
        public static bool IsHttpForced()
        {
            var development = bool.TryParse(Environment.GetEnvironmentVariable(ForceHttpKey), out var result) ? result : false;

            return development;
        }

        /// <summary>
        /// Checks whether HTTPS is forced or not.
        /// </summary>
        /// <returns>Returns <c>True</c>, if HTTPS is forced; otherwise returns <c>False</c>.</returns>
        public static bool IsHttpsForced()
        {
            var development = bool.TryParse(Environment.GetEnvironmentVariable(ForceHttpsKey), out var result) ? result : false;

            return development;
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

        private static string DefaultOpenApiDocDescription()
        {
            return "This is the OpenAPI Document on Azure Functions";
        }
    }
}
