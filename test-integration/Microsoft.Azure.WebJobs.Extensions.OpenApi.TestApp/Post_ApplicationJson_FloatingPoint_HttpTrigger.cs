using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models;
using Microsoft.Extensions.Logging;


namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp
{
    public class Post_ApplicationJson_FloatingPoint_HttpTrigger
    {
        [FunctionName(nameof(Post_ApplicationJson_Single))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_FloatingPoint_HttpTrigger.Post_ApplicationJson_Single), tags: new[] { "single" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(float), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(FloatingPointObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_Single(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-single")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Post_ApplicationJson_Double))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_FloatingPoint_HttpTrigger.Post_ApplicationJson_Double), tags: new[] { "double" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(double), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(FloatingPointObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_Double(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-double")] HttpRequest req,
        ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Post_ApplicationJson_Decimal))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_FloatingPoint_HttpTrigger.Post_ApplicationJson_Decimal), tags: new[] { "demical" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(double), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(FloatingPointObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_Decimal(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-decimal")] HttpRequest req,
        ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
