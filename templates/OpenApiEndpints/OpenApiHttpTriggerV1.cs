#if NET461
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi
{
    /// <summary>
    /// This represents the HTTP trigger entity for Open API documents.
    /// </summary>
    public static class OpenApiHttpTrigger
    {
        private const string V2 = "v2";
        private const string V3 = "v3";
        private const string JSON = "json";
        private const string YAML = "yaml";

        private readonly static IOpenApiHttpTriggerContext context = new OpenApiHttpTriggerContext();

        /// <summary>
        /// Invokes the HTTP trigger endpoint to get Swagger document in a format of JSON.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestMessage"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Swagger document in a format of JSON.</returns>
        [FunctionName(nameof(OpenApiHttpTrigger.RenderSwaggerDocumentInJson))]
        [OpenApiIgnore]
        public static async Task<HttpResponseMessage> RenderSwaggerDocumentInJson(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "swagger.json")] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation($"swagger.json was requested.");

            var result = await context.Document
                                      .InitialiseDocument()
                                      .AddMetadata(context.OpenApiInfo)
                                      .AddServer(req, context.HttpSettings.RoutePrefix)
                                      .AddNamingStrategy(context.NamingStrategy)
                                      .AddVisitors(context.GetVisitorCollection())
                                      .Build(context.GetExecutingAssembly())
                                      .RenderAsync(context.GetOpenApiSpecVersion(V2), context.GetOpenApiFormat(JSON))
                                      .ConfigureAwait(false);

            var content = new StringContent(result, Encoding.UTF8, context.GetOpenApiFormat(JSON).GetContentType());
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

            return response;
        }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to get Swagger document in a format of YAML.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestMessage"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Swagger document in a format in YAML.</returns>
        [FunctionName(nameof(OpenApiHttpTrigger.RenderSwaggerDocumentInYml))]
        [OpenApiIgnore]
        public static async Task<HttpResponseMessage> RenderSwaggerDocumentInYml(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "swagger.yml")] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation($"swagger.yaml was requested.");

            var result = await context.Document
                                      .InitialiseDocument()
                                      .AddMetadata(context.OpenApiInfo)
                                      .AddServer(req, context.HttpSettings.RoutePrefix)
                                      .AddNamingStrategy(context.NamingStrategy)
                                      .AddVisitors(context.GetVisitorCollection())
                                      .Build(context.GetExecutingAssembly())
                                      .RenderAsync(context.GetOpenApiSpecVersion(V2), context.GetOpenApiFormat(YAML))
                                      .ConfigureAwait(false);

            var content = new StringContent(result, Encoding.UTF8, context.GetOpenApiFormat(YAML).GetContentType());
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

            return response;
        }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to get Swagger document in a format of YAML.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestMessage"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Swagger document in a format in YAML.</returns>
        [FunctionName(nameof(OpenApiHttpTrigger.RenderSwaggerDocumentInYaml))]
        [OpenApiIgnore]
        public static async Task<HttpResponseMessage> RenderSwaggerDocumentInYaml(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "swagger.yaml")] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation($"swagger.yaml was requested.");

            var result = await context.Document
                                      .InitialiseDocument()
                                      .AddMetadata(context.OpenApiInfo)
                                      .AddServer(req, context.HttpSettings.RoutePrefix)
                                      .AddNamingStrategy(context.NamingStrategy)
                                      .AddVisitors(context.GetVisitorCollection())
                                      .Build(context.GetExecutingAssembly())
                                      .RenderAsync(context.GetOpenApiSpecVersion(V2), context.GetOpenApiFormat(YAML))
                                      .ConfigureAwait(false);

            var content = new StringContent(result, Encoding.UTF8, context.GetOpenApiFormat(YAML).GetContentType());
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

            return response;
        }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to get Open API document v2 in a format of JSON.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestMessage"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Open API document v2 in a format of JSON.</returns>
        [FunctionName(nameof(OpenApiHttpTrigger.RenderOpenApiDocumentV2InJson))]
        [OpenApiIgnore]
        public static async Task<HttpResponseMessage> RenderOpenApiDocumentV2InJson(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "openapi/v2.json")] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation($"v2.json was requested.");

            var result = await context.Document
                                      .InitialiseDocument()
                                      .AddMetadata(context.OpenApiInfo)
                                      .AddServer(req, context.HttpSettings.RoutePrefix)
                                      .AddNamingStrategy(context.NamingStrategy)
                                      .AddVisitors(context.GetVisitorCollection())
                                      .Build(context.GetExecutingAssembly())
                                      .RenderAsync(context.GetOpenApiSpecVersion(V2), context.GetOpenApiFormat(JSON))
                                      .ConfigureAwait(false);

            var content = new StringContent(result, Encoding.UTF8, context.GetOpenApiFormat(JSON).GetContentType());
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

            return response;
        }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to get Open API document v2 in a format of YAML.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestMessage"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Open API document v2 in a format of YAML.</returns>
        [FunctionName(nameof(OpenApiHttpTrigger.RenderOpenApiDocumentV2InYml))]
        [OpenApiIgnore]
        public static async Task<HttpResponseMessage> RenderOpenApiDocumentV2InYml(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "openapi/v2.yml")] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation($"v2.yaml was requested.");

            var result = await context.Document
                                      .InitialiseDocument()
                                      .AddMetadata(context.OpenApiInfo)
                                      .AddServer(req, context.HttpSettings.RoutePrefix)
                                      .AddNamingStrategy(context.NamingStrategy)
                                      .AddVisitors(context.GetVisitorCollection())
                                      .Build(context.GetExecutingAssembly())
                                      .RenderAsync(context.GetOpenApiSpecVersion(V2), context.GetOpenApiFormat(YAML))
                                      .ConfigureAwait(false);

            var content = new StringContent(result, Encoding.UTF8, context.GetOpenApiFormat(YAML).GetContentType());
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

            return response;
        }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to get Open API document v2 in a format of YAML.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestMessage"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Open API document v2 in a format of YAML.</returns>
        [FunctionName(nameof(OpenApiHttpTrigger.RenderOpenApiDocumentV2InYaml))]
        [OpenApiIgnore]
        public static async Task<HttpResponseMessage> RenderOpenApiDocumentV2InYaml(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "openapi/v2.yaml")] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation($"v2.yaml was requested.");

            var result = await context.Document
                                      .InitialiseDocument()
                                      .AddMetadata(context.OpenApiInfo)
                                      .AddServer(req, context.HttpSettings.RoutePrefix)
                                      .AddNamingStrategy(context.NamingStrategy)
                                      .AddVisitors(context.GetVisitorCollection())
                                      .Build(context.GetExecutingAssembly())
                                      .RenderAsync(context.GetOpenApiSpecVersion(V2), context.GetOpenApiFormat(YAML))
                                      .ConfigureAwait(false);

            var content = new StringContent(result, Encoding.UTF8, context.GetOpenApiFormat(YAML).GetContentType());
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

            return response;
        }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to get Open API document v3 in a format of JSON.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestMessage"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Open API document v2 in a format of JSON.</returns>
        [FunctionName(nameof(OpenApiHttpTrigger.RenderOpenApiDocumentV3InJson))]
        [OpenApiIgnore]
        public static async Task<HttpResponseMessage> RenderOpenApiDocumentV3InJson(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "openapi/v3.json")] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation($"v3.json was requested.");

            var result = await context.Document
                                      .InitialiseDocument()
                                      .AddMetadata(context.OpenApiInfo)
                                      .AddServer(req, context.HttpSettings.RoutePrefix)
                                      .AddNamingStrategy(context.NamingStrategy)
                                      .AddVisitors(context.GetVisitorCollection())
                                      .Build(context.GetExecutingAssembly())
                                      .RenderAsync(context.GetOpenApiSpecVersion(V3), context.GetOpenApiFormat(JSON))
                                      .ConfigureAwait(false);

            var content = new StringContent(result, Encoding.UTF8, context.GetOpenApiFormat(JSON).GetContentType());
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

            return response;
        }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to get Open API document v3 in a format of YAML.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestMessage"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Open API document v3 in a format of YAML.</returns>
        [FunctionName(nameof(OpenApiHttpTrigger.RenderOpenApiDocumentV3InYml))]
        [OpenApiIgnore]
        public static async Task<HttpResponseMessage> RenderOpenApiDocumentV3InYml(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "openapi/v3.yml")] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation($"v3.yaml was requested.");

            var result = await context.Document
                                      .InitialiseDocument()
                                      .AddMetadata(context.OpenApiInfo)
                                      .AddServer(req, context.HttpSettings.RoutePrefix)
                                      .AddNamingStrategy(context.NamingStrategy)
                                      .AddVisitors(context.GetVisitorCollection())
                                      .Build(context.GetExecutingAssembly())
                                      .RenderAsync(context.GetOpenApiSpecVersion(V3), context.GetOpenApiFormat(YAML))
                                      .ConfigureAwait(false);

            var content = new StringContent(result, Encoding.UTF8, context.GetOpenApiFormat(YAML).GetContentType());
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

            return response;
        }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to get Open API document v3 in a format of YAML.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestMessage"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Open API document v3 in a format of YAML.</returns>
        [FunctionName(nameof(OpenApiHttpTrigger.RenderOpenApiDocumentV3InYaml))]
        [OpenApiIgnore]
        public static async Task<HttpResponseMessage> RenderOpenApiDocumentV3InYaml(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "openapi/v3.yaml")] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation($"v3.yaml was requested.");

            var result = await context.Document
                                      .InitialiseDocument()
                                      .AddMetadata(context.OpenApiInfo)
                                      .AddServer(req, context.HttpSettings.RoutePrefix)
                                      .AddNamingStrategy(context.NamingStrategy)
                                      .AddVisitors(context.GetVisitorCollection())
                                      .Build(context.GetExecutingAssembly())
                                      .RenderAsync(context.GetOpenApiSpecVersion(V3), context.GetOpenApiFormat(YAML))
                                      .ConfigureAwait(false);

            var content = new StringContent(result, Encoding.UTF8, context.GetOpenApiFormat(YAML).GetContentType());
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

            return response;
        }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to render Swagger UI in HTML.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestMessage"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Swagger UI in HTML.</returns>
        [FunctionName(nameof(OpenApiHttpTrigger.RenderSwaggerUI))]
        [OpenApiIgnore]
        public static async Task<HttpResponseMessage> RenderSwaggerUI(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "swagger/ui")] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation($"SwaggerUI page was requested.");

            var result = await context.SwaggerUI
                                      .AddMetadata(context.OpenApiInfo)
                                      .AddServer(req, context.HttpSettings.RoutePrefix)
                                      .BuildAsync()
                                      .RenderAsync("swagger.json", context.GetSwaggerAuthKey())
                                      .ConfigureAwait(false);

            var content = new StringContent(result, Encoding.UTF8, "text/html");
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

            return response;
        }
    }
}
#endif
