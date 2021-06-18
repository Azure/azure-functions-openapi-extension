using System.Net;
using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.V3Net5
{
    public class DefaultIoCHttpTrigger
    {
        private readonly ISample _sample;
        public DefaultIoCHttpTrigger(ISample sample)
        {
            this._sample = sample;
        }

        [Function(nameof(DefaultIoCHttpTrigger.IoCRunAsync))]
        [OpenApiOperation(operationId: "iocRun", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<HttpResponseData> IoCRunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(DefaultIoCHttpTrigger));
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var value = this._sample.GetValue();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            await response.WriteStringAsync("Welcome to Azure Functions!").ConfigureAwait(false);

            return response;
        }
    }
}
