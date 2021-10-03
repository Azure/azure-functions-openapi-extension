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
    public static class Post_ApplicationJson_UInt64_HttpTrigger
    {
        [FunctionName(nameof(Post_ApplicationJson_UInt64_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_UInt64_HttpTrigger.Post_ApplicationJson_UInt64), tags: new[] { "UInt64" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(UInt64), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UInt64ObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_UInt64(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-uint64")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
