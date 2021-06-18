using System.Net;
using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.V3Net5
{
    public static class DefaultStaticHttpTrigger
    {
        [Function(nameof(DefaultStaticHttpTrigger.StaticRunAsync))]
        [OpenApiOperation(operationId: "staticRun", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async static Task<HttpResponseData> StaticRunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "names/{name}")] HttpRequestData req,
            string name,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(DefaultStaticHttpTrigger));
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            await response.WriteStringAsync($"Welcome, {name}, to Azure Functions!").ConfigureAwait(false);

            return response;
        }
    }
}
