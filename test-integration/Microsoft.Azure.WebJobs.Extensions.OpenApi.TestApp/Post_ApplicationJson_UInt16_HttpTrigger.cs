using System;
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
    public static class Post_ApplicationJson_UInt16_HttpTrigger
    {
        [FunctionName(nameof(Post_ApplicationJson_UInt16_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_UInt16_HttpTrigger.Post_ApplicationJson_UInt16), tags: new[] { "UInt16" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(UInt16), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UInt16ObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_UInt16(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-uint16")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
