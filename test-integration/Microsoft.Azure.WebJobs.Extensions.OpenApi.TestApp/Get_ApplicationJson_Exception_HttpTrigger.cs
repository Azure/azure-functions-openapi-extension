using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp
{
    public class Get_ApplicationJson_Exception_HttpTrigger
    {
        [FunctionName(nameof(Get_ApplicationJson_Exception_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Get_ApplicationJson_Exception_HttpTrigger.Get_ApplicationJson_Exception), tags: new[] { "exception" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(StackOverflowException), Description = "The OK response")]
        public static async Task<IActionResult> Get_ApplicationJson_Exception(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-applicationjson-exception")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
