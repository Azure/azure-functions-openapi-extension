using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp
{
    public static class Get_ApplicationJson_JsonPropertyObject_HttpTrigger
    {
        [FunctionName(nameof(Get_ApplicationJson_JsonPropertyObject_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Get_ApplicationJson_JsonPropertyObject_HttpTrigger.Get_ApplicationJson_JsonPropertyObject), tags: new[] { "jsonproperty" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(JsonPropertyObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Get_ApplicationJson_JsonPropertyObject(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-applicationjson-jsonproperty")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
