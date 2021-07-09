using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        public virtual bool IncludeRequestingHostName { get; set; } = IsFunctionsRuntimeEnvironmentDevelopment();

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
        /// <param name="type">Type to get the title.</param>
        /// <returns>Returns the OpenAPI document title.</returns>
        protected static string GetOpenApiDocTitle(Type type = null)
        {
            var title = Environment.GetEnvironmentVariable(OpenApiDocTitleKey) ?? DefaultOpenApiDocTitle(type);

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
        /// Checks whether the current Azure Functions runtime environment is "Development" or not.
        /// </summary>
        /// <returns>Returns <c>True</c>, if the current Azure Functions runtime environment is "Development"; otherwise returns <c>False</c></returns>
        protected static bool IsFunctionsRuntimeEnvironmentDevelopment()
        {
            var development = Environment.GetEnvironmentVariable(FunctionsRuntimeEnvironmentKey) == "Development";

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

        private static string DefaultOpenApiDocTitle(Type type = null)
        {
            if (type == null)
            {
                type = typeof(DefaultOpenApiConfigurationOptions);
            }

            var assembly = Assembly.GetAssembly(type);

            return assembly.GetName().Name;
        }
    }
}
