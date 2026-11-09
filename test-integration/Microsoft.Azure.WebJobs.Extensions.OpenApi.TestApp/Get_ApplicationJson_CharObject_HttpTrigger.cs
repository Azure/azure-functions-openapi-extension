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
    public class Get_ApplicationJson_CharObject_HttpTrigger
    {
        [FunctionName(nameof(Get_ApplicationJson_CharObject_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Get_ApplicationJson_CharObject_HttpTrigger.Get_ApplicationJson_CharObject), tags: new[] { "char" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(CharObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Get_ApplicationJson_CharObject(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-applicationjson-char")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}