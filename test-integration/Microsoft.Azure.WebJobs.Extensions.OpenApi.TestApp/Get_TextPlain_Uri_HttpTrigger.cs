using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp
{
    public static class Get_TextPlain_Uri_HttpTrigger
    {
        [FunctionName(nameof(Get_TextPlain_Uri_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Uri_HttpTrigger.Get_TextPlain_Uri), tags: new[] { "uri" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(Uri), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(Uri), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_Uri(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-applicationjson-uri")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
