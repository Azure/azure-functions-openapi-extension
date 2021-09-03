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
    public static class Get_ApplicationOctetStream_ByteArray_HttpTrigger
    {
        [FunctionName(nameof(Get_ApplicationOctetStream_ByteArray_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Get_ApplicationOctetStream_ByteArray_HttpTrigger.Get_ApplicationOctetStream_ByteArray), tags: new[] { "bytearray" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/octet-stream", bodyType: typeof(SingleByteArrayObject), Description = "The OK response")]
        public static async Task<IActionResult> Get_ApplicationOctetStream_ByteArray(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-applicationoctetstream-bytearray")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
