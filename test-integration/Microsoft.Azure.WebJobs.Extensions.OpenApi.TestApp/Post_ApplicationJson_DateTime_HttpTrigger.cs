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
    public static class Post_ApplicationJson_DateTimeObject_HttpTrigger
    {
        [FunctionName(nameof(Post_ApplicationJson_DateTimeObject_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_DateTimeObject_HttpTrigger.Post_ApplicationJson_DateTimeObject), tags: new[] { "datetime" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(DateTime), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DateTimeObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_DateTimeObject(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-datetimeobject")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }

    public static class Post_ApplicationJson_DateTimeOffset_HttpTrigger
    {
        [FunctionName(nameof(Post_ApplicationJson_DateTimeOffset_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_DateTimeOffset_HttpTrigger.Post_ApplicationJson_DateTimeOffset), tags: new[] { "datetimeoffset" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(DateTimeOffset), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DateTimeOffsetObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_DateTimeOffset(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-datetimeoffset")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}  