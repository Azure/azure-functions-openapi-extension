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
    public static class Post_ApplicationJson_NullableObject_HttpTrigger
    {
        [FunctionName(nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableBoolean))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableBoolean), tags: new[] { "nullable" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(bool?), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NullableObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_NullableBoolean(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-nullableboolean")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableUInt16))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableUInt16), tags: new[] { "nullable" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(ushort?), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NullableObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_NullableUInt16(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-nullableuint16")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableUInt32))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableUInt32), tags: new[] { "nullable" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(uint?), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NullableObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_NullableUInt32(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-nullableuint32")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableUInt64))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableUInt64), tags: new[] { "nullable" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(ulong?), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NullableObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_NullableUInt64(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-nullableuint64")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableInt16))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableInt16), tags: new[] { "nullable" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(short?), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NullableObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_NullableInt16(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-nullableint16")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableInt32))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableInt32), tags: new[] { "nullable" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(int?), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NullableObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_NullableInt32(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-nullableint32")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableInt64))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableInt64), tags: new[] { "nullable" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(long?), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NullableObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_NullableInt64(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-nullableint64")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableSingle))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableSingle), tags: new[] { "nullable" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(float?), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NullableObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_NullableSingle(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-nullablesingle")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableDouble))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableDouble), tags: new[] { "nullable" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(double?), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NullableObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_NullableDouble(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-nullabledouble")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableDecimal))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableDecimal), tags: new[] { "nullable" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(decimal?), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NullableObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_NullableDecimal(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-nullabledecimal")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableDateTime))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableDateTime), tags: new[] { "nullable" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(DateTime?), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NullableObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_NullableDateTime(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-nullabledatetime")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableDateTimeOffset))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_NullableObject_HttpTrigger.Post_ApplicationJson_NullableDateTimeOffset), tags: new[] { "nullable" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(DateTimeOffset?), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(NullableObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_NullableDateTimeOffset(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-nullabledatetimeoffset")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
