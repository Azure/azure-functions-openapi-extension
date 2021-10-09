using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.V3Net5
{
    public class DefaultHttpTrigger
    {
        [Function("Exception")]
        [OpenApiOperation(operationId: "exception", Description = "Verify the health of this API")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ExceptionModel), Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.ServiceUnavailable, contentType: "application/json", bodyType: typeof(ExceptionModel), Description = "Service unavailable response")]
        public IActionResult Exception(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequest req)
        {
            return new OkObjectResult("");
        }
    }

    public class ExceptionModel
    {
        public Int16 Int16 { get; set; }
        public UInt16 UInt16 { get; set; }
        public Int32 Int32 { get; set; }
        public UInt32 UInt32 { get; set; }
        public Int64 Int64 { get; set; }
        public UInt64 UInt64 { get; set; }


        public long Long { get; set; }
        public ulong ULong { get; set; }
    }
}
