using System.Collections.Generic;
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
    public static class Get_ApplicationJson_Array_HttpTrigger
    {
        
        [FunctionName(nameof(Get_ApplicationJson_Array_HttpTrigger.Get_ApplicationJson_StringArray))]
        [OpenApiOperation(operationId: nameof(Get_ApplicationJson_Array_HttpTrigger.Get_ApplicationJson_StringArray), tags: new[] { "array" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string[]), Description = "The OK response")]
        public static async Task<IActionResult> Get_ApplicationJson_StringArray(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-applicationjson-string-array")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_ApplicationJson_Array_HttpTrigger.Get_ApplicationJson_IntArray))]
        [OpenApiOperation(operationId: nameof(Get_ApplicationJson_Array_HttpTrigger.Get_ApplicationJson_IntArray), tags: new[] { "array" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(int[]), Description = "The OK response")]
        public static async Task<IActionResult> Get_ApplicationJson_IntArray(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-applicationjson-int-array")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_ApplicationJson_Array_HttpTrigger.Get_ApplicationJson_BoolArray))]
        [OpenApiOperation(operationId: nameof(Get_ApplicationJson_Array_HttpTrigger.Get_ApplicationJson_BoolArray), tags: new[] { "array" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(bool[]), Description = "The OK response")]
        public static async Task<IActionResult> Get_ApplicationJson_BoolArray(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-applicationjson-bool-array")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_ApplicationJson_Array_HttpTrigger.Get_ApplicationJson_IntList))]
        [OpenApiOperation(operationId: nameof(Get_ApplicationJson_Array_HttpTrigger.Get_ApplicationJson_IntList), tags: new[] { "array" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<int>), Description = "The OK response")]
        public static async Task<IActionResult> Get_ApplicationJson_IntList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-applicationjson-int-list")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}