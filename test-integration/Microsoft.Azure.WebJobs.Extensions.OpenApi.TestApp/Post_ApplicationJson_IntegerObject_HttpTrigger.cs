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
    public class Post_ApplicationJson_IntegerObject_HttpTrigger
    {
        [FunctionName(nameof(Post_ApplicationJson_Int16Object))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_IntegerObject_HttpTrigger.Post_ApplicationJson_Int16Object), tags: new[] { "integer" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(short), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IntegerObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_Int16Object(
           [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-int16")] HttpRequest req,
           ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Post_ApplicationJson_Int32Object))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_IntegerObject_HttpTrigger.Post_ApplicationJson_Int32Object), tags: new[] { "integer" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(int), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IntegerObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_Int32Object(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-int32")] HttpRequest req,
        ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Post_ApplicationJson_Int64Object))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_IntegerObject_HttpTrigger.Post_ApplicationJson_Int64Object), tags: new[] { "integer" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(long), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IntegerObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_Int64Object(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-int64")] HttpRequest req,
        ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Post_ApplicationJson_UInt16Object))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_IntegerObject_HttpTrigger.Post_ApplicationJson_UInt16Object), tags: new[] { "uint16value" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(ushort), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IntegerObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_UInt16Object(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-uint16")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Post_ApplicationJson_UInt32Object))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_IntegerObject_HttpTrigger.Post_ApplicationJson_UInt32Object), tags: new[] { "integer" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(uint), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IntegerObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_UInt32Object(
          [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-uint32")] HttpRequest req,
          ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Post_ApplicationJson_UInt64Object))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_IntegerObject_HttpTrigger.Post_ApplicationJson_UInt64Object), tags: new[] { "integer" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(ulong), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IntegerObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_UInt64Object(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-uint64")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
