using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Functions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions
{
    /// <summary>
    /// This represents the extensions entity to configure OpenAPI capability to Azure Functions out-of-process worker.
    /// </summary>
    public static class OpenApiHostBuilderExtensions
    {
        /// <summary>
        /// Configures to use OpenAPI features.
        /// </summary>
        /// <param name="hostBuilder"><see cref="IHostBuilder"/> instance.</param>
        /// <returns>Returns <see cref="IHostBuilder"/> instance.</returns>
        public static IHostBuilder ConfigureOpenApi(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<IOpenApiHttpTriggerContext, OpenApiHttpTriggerContext>();
                services.AddSingleton<IOpenApiTriggerFunction, OpenApiTriggerFunction>();

                var function = services.BuildServiceProvider().GetService<IOpenApiTriggerFunction>();
                var authLevelDoc = Enum.TryParse<OpenApiAuthLevelType>(Environment.GetEnvironmentVariable("OpenApi__AuthLevel__Document"), ignoreCase: true, out var resultDoc)
                                   ? resultDoc
                                   : OpenApiAuthLevelType.Anonymous;
                var authLevelUI =  Enum.TryParse<OpenApiAuthLevelType>(Environment.GetEnvironmentVariable("OpenApi__AuthLevel__UI"), ignoreCase: true, out var resultUI)
                                   ? resultUI
                                   : OpenApiAuthLevelType.Anonymous;

                var types = Assembly.GetAssembly(typeof(OpenApiHostBuilderExtensions)).GetTypes();

                services.AddSingleton<IOpenApiTriggerRenderSwaggerDocumentFunctionProvider>(
                    CreateInstance<IOpenApiTriggerRenderSwaggerDocumentFunctionProvider>(types, "OpenApiTriggerRenderSwaggerDocument{0}FunctionProvider", authLevelDoc, function));
                services.AddSingleton<IOpenApiTriggerRenderOpenApiDocumentFunctionProvider>(
                    CreateInstance<IOpenApiTriggerRenderOpenApiDocumentFunctionProvider>(types, "OpenApiTriggerRenderOpenApiDocument{0}FunctionProvider", authLevelDoc, function));
                services.AddSingleton<IOpenApiTriggerRenderOAuth2RedirectFunctionProvider>(
                    CreateInstance<IOpenApiTriggerRenderOAuth2RedirectFunctionProvider>(types, "OpenApiTriggerRenderOAuth2Redirect{0}FunctionProvider", authLevelUI, function));

                var hideSwaggerUI = bool.TryParse(Environment.GetEnvironmentVariable("OpenApi__HideSwaggerUI"), out var result)
                                    ? result
                                    : false;
                if (!hideSwaggerUI)
                {
                    services.AddSingleton<IOpenApiTriggerRenderSwaggerUIFunctionProvider>(
                        CreateInstance<IOpenApiTriggerRenderSwaggerUIFunctionProvider>(types, "OpenApiTriggerRenderSwaggerUI{0}FunctionProvider", authLevelUI, function));
                }
            });

            return hostBuilder;
        }

        private static T CreateInstance<T>(IEnumerable<Type> types, string typeName, OpenApiAuthLevelType authLevel, IOpenApiTriggerFunction function)
        {
            var instance = (T)Activator.CreateInstance(types.Single(p => p.Name == string.Format(typeName, authLevel)), new[] { function });

            return instance;
        }
    }
}
