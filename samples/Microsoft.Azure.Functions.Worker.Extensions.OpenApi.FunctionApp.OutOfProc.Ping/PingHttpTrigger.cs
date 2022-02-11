using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using System.Net;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.OutOfProc.Ping
{
    public class PingHttpTrigger
    {
        private readonly ILogger<PingHttpTrigger> _logger;

        public PingHttpTrigger(ILogger<PingHttpTrigger> log)
        {
            _logger = log;
        }

        [Function(nameof(PingHttpTrigger.Ping))]
        [OpenApiOperation(operationId: "ping", tags: new[] { "ping" }, Summary = "Pings for health check", Description = "This pings for health check.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Summary = "Successful operation", Description = "Successful operation")]
        public async Task<HttpResponseData> Ping(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "ping")] HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);

            return await Task.FromResult(response).ConfigureAwait(false);
        }
    }
}
