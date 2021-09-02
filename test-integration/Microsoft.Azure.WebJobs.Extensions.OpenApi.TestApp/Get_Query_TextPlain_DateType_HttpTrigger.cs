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
    public static class Get_Query_TextPlain_DateType_HttpTrigger
    {
        [FunctionName(nameof(Get_Query_TextPlain_DateType_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Get_Query_TextPlain_DateType_HttpTrigger.Get_Query_TextPlain_DateType), tags: new[] { "greeting" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(DateTypeObject), Description = "The OK response")]
        public static async Task<IActionResult> Get_Query_TextPlain_DateType(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-query-textplain-datetype")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }

}
