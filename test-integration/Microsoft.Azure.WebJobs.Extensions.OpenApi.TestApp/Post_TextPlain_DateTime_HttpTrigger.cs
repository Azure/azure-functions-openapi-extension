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
    public static class Post_TextPlain_DateTime_HttpTrigger
    {
        [FunctionName(nameof(Post_TextPlain_DateTime_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Post_TextPlain_DateTime_HttpTrigger.Post_TextPlain_DateTime), tags: new[] { "datetime" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(DateTime), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(DateTime), Description = "The OK response")]
        public static async Task<IActionResult> Post_TextPlain_DateTime(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-textplain-datetime")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }

    public static class Post_TextPlain_DateTimeOffset_HttpTrigger
    {
        [FunctionName(nameof(Post_TextPlain_DateTimeOffset_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Post_TextPlain_DateTimeOffset_HttpTrigger.Post_TextPlain_DateTimeOffset), tags: new[] { "datetime" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(DateTime), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(DateTimeOffset), Description = "The OK response")]
        public static async Task<IActionResult> Post_TextPlain_DateTimeOffset(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-textplain-datetimeoffset")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}