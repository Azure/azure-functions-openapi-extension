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
    public static class Get_Query_TextPlain_DataType_HttpTrigger
    {
        [FunctionName(nameof(Get_Query_TextPlain_DataType_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Get_Query_TextPlain_DataType_HttpTrigger.Get_Query_TextPlain_DataType), tags: new[] { "greeting" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(DataTypeClass), Description = "The OK response")]
        public static async Task<IActionResult> Get_Query_TextPlain_DataType(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-query-textplain-datatype")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }

}
