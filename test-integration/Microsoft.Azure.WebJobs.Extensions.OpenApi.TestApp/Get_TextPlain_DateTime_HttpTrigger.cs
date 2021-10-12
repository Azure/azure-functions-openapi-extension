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
    public static class Get_TextPlain_DateTime_HttpTrigger
    {
        [FunctionName(nameof(Get_TextPlain_DateTime_HttpTrigger.Get_TextPlain_DateTime))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_DateTime_HttpTrigger.Get_TextPlain_DateTime), tags: new[] { "datetime" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(DateTime), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_DateTime(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "get-textplain-datetime")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_TextPlain_DateTime_HttpTrigger.Get_TextPlain_DateTimeOffset))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_DateTime_HttpTrigger.Get_TextPlain_DateTimeOffset), tags: new[] { "datetime" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(DateTimeOffset), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_DateTimeOffset(
            [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "get-textplain-datetimeoffset")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}