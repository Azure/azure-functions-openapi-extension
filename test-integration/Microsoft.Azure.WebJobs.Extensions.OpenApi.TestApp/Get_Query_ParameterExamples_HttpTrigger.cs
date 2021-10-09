using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Examples;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp
{
    public static class Get_Query_ParameterExamples_HttpTrigger
    {
        [FunctionName(nameof(Get_Query_ParameterExamples_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Get_Query_ParameterExamples_HttpTrigger.Get_Query_ParameterExamples), tags: new[] { "parameterExamples" })]
        [OpenApiParameter(name: "stringParameter", In = ParameterLocation.Query, Required = true, Example = typeof(StringParameterExample), Type = typeof(string), Description = "The **string** parameter")]
        [OpenApiParameter(name: "int16Parameter", In = ParameterLocation.Query, Required = true, Example = typeof(Int16ParameterExample), Type = typeof(short), Description = "The **int16** parameter")]
        [OpenApiParameter(name: "int32Parameter", In = ParameterLocation.Query, Required = true, Example = typeof(Int32ParameterExample), Type = typeof(int), Description = "The **int32** parameter")]
        [OpenApiParameter(name: "int64Parameter", In = ParameterLocation.Query, Required = true, Example = typeof(Int64ParameterExample), Type = typeof(long), Description = "The **int64** parameter")]
        [OpenApiParameter(name: "uint16Parameter", In = ParameterLocation.Query, Required = true, Example = typeof(Uint16ParameterExample), Type = typeof(ushort), Description = "The **uint16** parameter")]
        [OpenApiParameter(name: "uint32Parameter", In = ParameterLocation.Query, Required = true, Example = typeof(Uint32ParameterExample), Type = typeof(uint), Description = "The **uint32** parameter")]
        [OpenApiParameter(name: "uint64Parameter", In = ParameterLocation.Query, Required = true, Example = typeof(Uint64ParameterExample), Type = typeof(ulong), Description = "The **uint64** parameter")]
        [OpenApiParameter(name: "singleParameter", In = ParameterLocation.Query, Required = true, Example = typeof(SingleParameterExample), Type = typeof(float), Description = "The **single** parameter")]
        [OpenApiParameter(name: "doubleParameter", In = ParameterLocation.Query, Required = true, Example = typeof(DoubleParameterExample), Type = typeof(double), Description = "The **double** parameter")]
        [OpenApiParameter(name: "dateTimeParameter", In = ParameterLocation.Query, Required = true, Example = typeof(DateTimeParameterExample), Type = typeof(DateTime), Description = "The **dateTime** parameter")]
        [OpenApiParameter(name: "dateTimeOffsetParameter", In = ParameterLocation.Query, Required = true, Example = typeof(DateTimeOffsetParameterExample), Type = typeof(DateTimeOffset), Description = "The **dateTimeOffset** parameter")]
        [OpenApiParameter(name: "booleanParameter", In = ParameterLocation.Query, Required = true, Example = typeof(BooleanParameterExample), Type = typeof(bool), Description = "The **boolean** parameter")]
        [OpenApiParameter(name: "guidParameter", In = ParameterLocation.Query, Required = true, Example = typeof(GuidParameterExample), Type = typeof(Guid), Description = "The **guid** parameter")]
        [OpenApiParameter(name: "byteArrayParameter", In = ParameterLocation.Query, Required = true, Example = typeof(ByteArrayParameterExample), Type = typeof(byte[]), Description = "The **byteArray** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Get_Query_ParameterExamples(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-query-parameter-examples")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
