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
    public static class Post_ApplicationJson_ByteArrayObject_HttpTrigger
    {
        [FunctionName(nameof(Post_ApplicationJson_ByteArrayObject_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_ByteArrayObject_HttpTrigger.Post_ApplicationJson_ByteArrayObject), tags: new[] { "bytearray" })]
        [OpenApiRequestBody(contentType: "application/octet-stream", bodyType: typeof(byte[]), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ByteArrayObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_ByteArrayObject(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-bytearray")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
