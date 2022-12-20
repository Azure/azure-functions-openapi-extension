using System;
using System.Reflection;
using System.Threading.Tasks;

using AutoFixture;

using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.OutOfProc
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
                .ConfigureServices(services =>
                {
                    services.AddSingleton<Fixture>(new Fixture())
                            .AddSingleton<IOpenApiConfigurationOptions>(_ =>
                            {
                                var options = new OpenApiConfigurationOptions()
                                {
                                    Info = new OpenApiInfo()
                                    {
                                        Version = DefaultOpenApiConfigurationOptions.GetOpenApiDocVersion(),
                                        Title = $"{DefaultOpenApiConfigurationOptions.GetOpenApiDocTitle()} (Injected)",
                                        Description = DefaultOpenApiConfigurationOptions.GetOpenApiDocDescription(),
                                        TermsOfService = new Uri("https://github.com/Azure/azure-functions-openapi-extension"),
                                        Contact = new OpenApiContact()
                                        {
                                            Name = "Enquiry",
                                            Email = "azfunc-openapi@microsoft.com",
                                            Url = new Uri("https://github.com/Azure/azure-functions-openapi-extension/issues"),
                                        },
                                        License = new OpenApiLicense()
                                        {
                                            Name = "MIT",
                                            Url = new Uri("http://opensource.org/licenses/MIT"),
                                        }
                                    },
                                    Servers = DefaultOpenApiConfigurationOptions.GetHostNames(),
                                    OpenApiVersion = DefaultOpenApiConfigurationOptions.GetOpenApiVersion(),
                                    IncludeRequestingHostName = DefaultOpenApiConfigurationOptions.IsFunctionsRuntimeEnvironmentDevelopment(),
                                    ForceHttps = DefaultOpenApiConfigurationOptions.IsHttpsForced(),
                                    ForceHttp = DefaultOpenApiConfigurationOptions.IsHttpForced(),
                                };

                                return options;
                            })
                            .AddSingleton<IOpenApiHttpTriggerAuthorization>(_ =>
                            {
                                var auth = new OpenApiHttpTriggerAuthorization(req =>
                                {
                                    var result = default(OpenApiAuthorizationResult);

                                    // ⬇️⬇️⬇️ Add your custom authorisation logic ⬇️⬇️⬇️
                                    //
                                    // CUSTOM AUTHORISATION LOGIC
                                    //
                                    // ⬆️⬆️⬆️ Add your custom authorisation logic ⬆️⬆️⬆️

                                    return Task.FromResult(result);
                                });

                                return auth;
                            })
                            .AddSingleton<IOpenApiCustomUIOptions>(_ =>
                            {
                                var assembly = Assembly.GetExecutingAssembly();
                                var options = new OpenApiCustomUIOptions(assembly)
                                {
                                    GetStylesheet = () =>
                                    {
                                        var result = string.Empty;

                                        // ⬇️⬇️⬇️ Add your logic to get your custom stylesheet ⬇️⬇️⬇️
                                        //
                                        // CUSTOM LOGIC TO GET STYLESHEET
                                        //
                                        // ⬆️⬆️⬆️ Add your logic to get your custom stylesheet ⬆️⬆️⬆️

                                        return Task.FromResult(result);
                                    },
                                    GetJavaScript = () =>
                                    {
                                        var result = string.Empty;

                                        // ⬇️⬇️⬇️ Add your logic to get your custom JavaScript ⬇️⬇️⬇️
                                        //
                                        // CUSTOM LOGIC TO GET JAVASCRIPT
                                        //
                                        // ⬆️⬆️⬆️ Add your logic to get your custom JavaScript ⬆️⬆️⬆️

                                        return Task.FromResult(result);
                                    }
                                };

                                return options;
                            })
                            ;
                })
                .Build();

            host.Run();
        }
    }
}
