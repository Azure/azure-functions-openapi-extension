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
    public static class Get_ApplicationJson_Byte_HttpTrigger
    {
        [FunctionName(nameof(Get_ApplicationJson_Byte_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Get_ApplicationJson_Byte_HttpTrigger.Get_ApplicationJson_Byte), tags: new[] { "greeting" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ByteTypeClass), Description = "The OK response")]
        public static async Task<IActionResult> Get_ApplicationJson_Byte(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-applicationjson-byte")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
