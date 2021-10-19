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
    public static class Get_TextPlain_Nullable_HttpTrigger
    {
        [FunctionName(nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableBoolean))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableBoolean), tags: new[] { "nullable" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(bool?), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_NullableBoolean(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-textplain-nullableboolean")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableUInt16))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableUInt16), tags: new[] { "nullable" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(ushort?), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_NullableUInt16(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-textplain-nullableuint16")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableUInt32))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableUInt32), tags: new[] { "nullable" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(uint?), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_NullableUInt32(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-textplain-nullableuint32")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableUInt64))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableUInt64), tags: new[] { "nullable" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(ulong?), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_NullableUInt64(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-textplain-nullableuint64")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableInt16))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableInt16), tags: new[] { "nullable" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(short?), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_NullableInt16(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-textplain-nullableint16")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableInt32))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableInt32), tags: new[] { "nullable" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(int?), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_NullableInt32(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-textplain-nullableint32")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableInt64))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableInt64), tags: new[] { "nullable" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(long?), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_NullableInt64(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-textplain-nullableint64")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableSingle))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableSingle), tags: new[] { "nullable" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(float?), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_NullableSingle(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-textplain-nullablesingle")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableDouble))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableDouble), tags: new[] { "nullable" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(double?), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_NullableDouble(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-textplain-nullabledouble")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableDecimal))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableDecimal), tags: new[] { "nullable" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(decimal?), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_NullableDecimal(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-textplain-nullabledecimal")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableDateTime))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableDateTime), tags: new[] { "nullable" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(DateTime?), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_NullableDateTime(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-textplain-nullabledatetime")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableDateTimeOffset))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Nullable_HttpTrigger.Get_TextPlain_NullableDateTimeOffset), tags: new[] { "nullable" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(DateTimeOffset?), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_NullableDateTimeOffset(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-textplain-nullabledatetimeoffset")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}

