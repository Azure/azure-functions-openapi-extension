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
    public static class Post_ApplicationJson_NumberObject_HttpTrigger
    {
        [FunctionName(nameof(Post_ApplicationJson_SingleObject))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_NumberObject_HttpTrigger.Post_ApplicationJson_SingleObject), tags: new[] { "number" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(float), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NumberObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_SingleObject(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-single")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Post_ApplicationJson_DoubleObject))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_NumberObject_HttpTrigger.Post_ApplicationJson_DoubleObject), tags: new[] { "number" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(double), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NumberObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_DoubleObject(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-double")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Post_ApplicationJson_DecimalObject))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_NumberObject_HttpTrigger.Post_ApplicationJson_DecimalObject), tags: new[] { "number" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(decimal), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NumberObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_DecimalObject(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-decimal")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
