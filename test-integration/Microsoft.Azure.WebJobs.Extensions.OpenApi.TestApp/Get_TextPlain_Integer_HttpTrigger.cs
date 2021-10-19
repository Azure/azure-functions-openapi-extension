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
    public class Get_TextPlain_Integer_HttpTrigger
    {
        [FunctionName(nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_Int16))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_Int16), tags: new[] { "integer" })]
        [OpenApiParameter(name: "int16value", In = ParameterLocation.Path, Required = true, Type = typeof(short), Description = "The **int16** parameter")]
        [OpenApiParameter(name: "int16value", In = ParameterLocation.Query, Required = true, Type = typeof(short), Description = "The **int16** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(short), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_Int16(
        [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "get-textplain-int16")] HttpRequest req,
        ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_Int32))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_Int32), tags: new[] { "integer" })]
        [OpenApiParameter(name: "int32value", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The **int32** parameter")]
        [OpenApiParameter(name: "int32value", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The **int32** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(int), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_Int32(
        [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "get-textplain-int32")] HttpRequest req,
        ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_Int64))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_Int64), tags: new[] { "integer" })]
        [OpenApiParameter(name: "int64value", In = ParameterLocation.Path, Required = true, Type = typeof(long), Description = "The **int64** parameter")]
        [OpenApiParameter(name: "int64value", In = ParameterLocation.Query, Required = true, Type = typeof(long), Description = "The **int64** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(long), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_Int64(
        [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "get-textplain-int64")] HttpRequest req,
        ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_UInt16))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_UInt16), tags: new[] { "integer" })]
        [OpenApiParameter(name: "uint16value", In = ParameterLocation.Path, Required = true, Type = typeof(short), Description = "The **uint16** parameter")]
        [OpenApiParameter(name: "uint16value", In = ParameterLocation.Query, Required = true, Type = typeof(short), Description = "The **uint16** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(ushort), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_UInt16(
        [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "get-textplain-uint16")] HttpRequest req,
        ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }


        [FunctionName(nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_UInt32))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_UInt32), tags: new[] { "integer" })]
        [OpenApiParameter(name: "uint32value", In = ParameterLocation.Path, Required = true, Type = typeof(uint), Description = "The **uint32** parameter")]
        [OpenApiParameter(name: "uint32value", In = ParameterLocation.Query, Required = true, Type = typeof(uint), Description = "The **uint32** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(uint), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_UInt32(
        [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "get-textplain-uint32")] HttpRequest req,
        ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }


        [FunctionName(nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_UInt64))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Integer_HttpTrigger.Get_TextPlain_UInt64), tags: new[] { "integer" })]
        [OpenApiParameter(name: "uint64value", In = ParameterLocation.Path, Required = true, Type = typeof(ulong), Description = "The **uint64** parameter")]
        [OpenApiParameter(name: "uint64value", In = ParameterLocation.Query, Required = true, Type = typeof(ulong), Description = "The **uint64** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(ulong), Description = "The OK response")]
        public static async Task<IActionResult> Get_TextPlain_UInt64(
        [HttpTrigger(AuthorizationLevel.Anonymous, "Get", Route = "get-textplain-uint64")] HttpRequest req,
        ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
