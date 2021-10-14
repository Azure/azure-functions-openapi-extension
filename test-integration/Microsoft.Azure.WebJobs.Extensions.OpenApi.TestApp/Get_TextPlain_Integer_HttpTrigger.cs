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
    public class Get_TextPlain_Integer_HttpTrigger
    {
        [FunctionName(nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_Int16))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_Int16), tags: new[] { "int16value" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(int), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_Int16(
        [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "get-textplain-int16")] HttpRequest req,
        ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_Int32))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_Int32), tags: new[] { "int32value" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(int), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_Int32(
        [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "get-textplain-int32")] HttpRequest req,
        ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_Int64))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_Int64), tags: new[] { "int64value" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(int), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_Int64(
        [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "get-textplain-int64")] HttpRequest req,
        ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_UInt16))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_UInt16), tags: new[] { "uint16value" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(int), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_UInt16(
        [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "get-textplain-uint16")] HttpRequest req,
        ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }


        [FunctionName(nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_UInt32))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_UInt32), tags: new[] { "uint32value" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(int), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_UInt32(
        [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "get-textplain-uint32")] HttpRequest req,
        ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }


        [FunctionName(nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_UInt64))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_UInt64), tags: new[] { "uint64value" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(int), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_UInt64(
        [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "get-textplain-uint64")] HttpRequest req,
        ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
