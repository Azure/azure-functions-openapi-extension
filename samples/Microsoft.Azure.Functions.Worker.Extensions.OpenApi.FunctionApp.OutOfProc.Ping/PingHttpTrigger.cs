using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using System.Net;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.OutOfProc.Ping
{
    public class PingHttpTrigger
    {
        private readonly ILogger _logger;
        private readonly OpenApiSettings _openapi;

        public PingHttpTrigger(ILoggerFactory loggerFactory, OpenApiSettings openapi)
        {
            this._logger = loggerFactory.ThrowIfNullOrDefault().CreateLogger<PingHttpTrigger>();
            this._openapi = openapi.ThrowIfNullOrDefault();
        }

        [Function(nameof(PingHttpTrigger.Ping))]
        [OpenApiOperation(operationId: "ping", tags: new[] { "ping" }, Summary = "Pings for health check", Description = "This pings for health check.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Summary = "Successful operation", Description = "Successful operation")]
        public async Task<HttpResponseData> Ping(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "ping")] HttpRequestData req)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            await response.WriteStringAsync("pong").ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }
    }
}
