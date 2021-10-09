using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.V3Net5
{
    public class PetStoreHttpTrigger
    {
        private readonly HealthCheckService healthCheckService;

        public PetStoreHttpTrigger(HealthCheckService healthCheckService)
        {
            this.healthCheckService = healthCheckService;
        }

        //[Function("Health")]
        //[OpenApiOperation(operationId: "health", Description = "Verify the health of this API")]
        //[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(HealthReport), Description = "The OK response")]
        //[OpenApiResponseWithBody(statusCode: HttpStatusCode.ServiceUnavailable, contentType: "application/json", bodyType: typeof(HealthReport), Description = "Service unavailable response")]
        //public async Task<IActionResult> Health(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequest req)
        //{
        //    var status = await this.healthCheckService.CheckHealthAsync();

        //    return status.Status == HealthStatus.Healthy
        //        ? new OkObjectResult(status)
        //        : new ObjectResult(status) { StatusCode = StatusCodes.Status503ServiceUnavailable };
        //}
    }
}
