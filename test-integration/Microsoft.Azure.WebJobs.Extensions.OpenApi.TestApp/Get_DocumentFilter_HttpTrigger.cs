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
    public static class Get_DocumentFilter_HttpTrigger
    {
        [FunctionName(nameof(Get_DocumentFilter_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Get_DocumentFilter_HttpTrigger.Get_DocumentFilter), tags: new[] { "documentFilter" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Get_DocumentFilter(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-documentfilter")] HttpRequest req,
            ILogger log)
        {
            var result  = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
