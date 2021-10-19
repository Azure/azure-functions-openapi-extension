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
    public static class Get_TextPlain_Guid_HttpTrigger
    {
        [FunctionName(nameof(Get_TextPlain_Guid_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Get_TextPlain_Guid_HttpTrigger), tags: new[] { "guid" })]
        [OpenApiParameter(name: "guid_path", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "guid parameter_path")]
        [OpenApiParameter(name: "guid_query", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Description = "guid parameter_query")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(Guid), Description = "The OK response")]
        public static async Task<IActionResult> Get_ApplicationJson_Object(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-textplain-guid")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}

