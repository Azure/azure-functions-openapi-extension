using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp
{
    public class Get_Deprecated_HttpTrigger
    {
        [FunctionName(nameof(Get_Deprecated_HttpTrigger.Get_TextPlain_Deprecated_True))]
        [OpenApiOperation(operationId: nameof(Get_Deprecated_HttpTrigger.Get_TextPlain_Deprecated_True), tags: new[] { "deprecated" }, Deprecated = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_Deprecated_True(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-textplain-deprecated-true")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_Deprecated_HttpTrigger.Get_TextPlain_Deprecated_False))]
        [OpenApiOperation(operationId: nameof(Get_Deprecated_HttpTrigger.Get_TextPlain_Deprecated_False), tags: new[] { "deprecated" }, Deprecated = false)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_Deprecated_False(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-textplain-deprecated-false")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_Deprecated_HttpTrigger.Get_TextPlain_Deprecated_Null))]
        [OpenApiOperation(operationId: nameof(Get_Deprecated_HttpTrigger.Get_TextPlain_Deprecated_Null), tags: new[] { "deprecated" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_Deprecated_Null(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-textplain-deprecated-null")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
