using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Extensions.OpenApi;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Resolvers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Azure.WebJobs.Script.Description;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

[assembly: WebJobsStartup(typeof(OpenApiWebJobsStartup))]
namespace Microsoft.Azure.WebJobs.Extensions.OpenApi
{
    /// <summary>
    /// This represents the startup entity for OpenAPI endpoints registration
    /// </summary>
    public class OpenApiWebJobsStartup : IWebJobsStartup
    {
        private const string OpenApiSettingsKey = "OpenApi";

        /// <inheritdoc />
        public void Configure(IWebJobsBuilder builder)
        {
            var config = ConfigurationResolver.Resolve();
            var settings = config.Get<OpenApiSettings>(OpenApiSettingsKey);

            builder.Services.AddSingleton(settings);
            builder.Services.AddSingleton<IFunctionProvider, OpenApiTriggerFunctionProvider>();
            builder.Services.AddSingleton<IOpenApiHttpTriggerContext, OpenApiHttpTriggerContext>();

            builder.AddExtension<OpenApiHttpTriggerContextBinding>();
        }
    }

    /// <summary>
    /// Binding for injecting the OpenApiTriggerContext during InProc function creation
    /// </summary>
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public class OpenApiHttpTriggerContextAttribute : Attribute
    {
    }

    /// <summary>
    /// Extension to register the [OpenApiHttpTriggerContext] attribute to inject IOpenApHttpTriggerContext
    /// </summary>
    [Extension(nameof(OpenApiHttpTriggerContextBinding))]
    public class OpenApiHttpTriggerContextBinding : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var rule = context.AddBindingRule<OpenApiHttpTriggerContextAttribute>();
            rule.BindToInput((OpenApiHttpTriggerContextAttribute attr, ValueBindingContext vbContext) =>
            {
                var httpContext = vbContext.FunctionContext.CreateObjectInstance<OpenApiHttpTriggerContext>();
                
                return Task.FromResult(httpContext);
            });
        }
    }
}
