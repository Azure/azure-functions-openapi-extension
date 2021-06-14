using Azure.Core.Serialization;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions
{
    /// <summary>
    /// This represents the extension entity for the instance implementing <see cref="IFunctionsWorkerApplicationBuilder"/>.
    /// </summary>
    public static class FunctionsWorkerApplicationBuilderExtensions
    {
        /// <summary>
        /// Activate Newtonsoft.Json.
        /// </summary>
        /// <param name="builder"><see cref="IFunctionsWorkerApplicationBuilder"/> instance.</param>
        /// <returns>Returns <see cref="IFunctionsWorkerApplicationBuilder"/> instance.</returns>
        /// <remarks>
        /// The functions worker uses the Azure SDK's ObjectSerializer to abstract away all JSON serialization. This allows you to
        /// swap out the default System.Text.Json implementation for the Newtonsoft.Json implementation.
        /// To do so, add the Microsoft.Azure.Core.NewtonsoftJson nuget package and then update the WorkerOptions.Serializer property.
        /// This method updates the Serializer to use Newtonsoft.Json. Call /api/HttpFunction to see the changes.
        ///
        /// Reference: https://github.com/Azure/azure-functions-dotnet-worker/blob/main/samples/Configuration/Program.cs
        /// </remarks>
        public static IFunctionsWorkerApplicationBuilder UseNewtonsoftJson(this IFunctionsWorkerApplicationBuilder builder)
        {
            builder.Services.Configure<WorkerOptions>(workerOptions =>
            {
                var settings = NewtonsoftJsonObjectSerializer.CreateJsonSerializerSettings();
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                settings.NullValueHandling = NullValueHandling.Ignore;

                workerOptions.Serializer = new NewtonsoftJsonObjectSerializer(settings);
            });

            return builder;
        }
    }

}
