using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models;
using Microsoft.Extensions.Logging;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp
{
    public class Get_ApplicationJson_DictionaryObject_HttpTrigger
    {
        [FunctionName(nameof(Get_ApplicationJson_DictionaryObject_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Get_ApplicationJson_DictionaryObject_HttpTrigger.Get_ApplicationJson_DictionaryObject), tags: new[] { "dictionary" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DictionaryObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Get_ApplicationJson_DictionaryObject(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-applicationjson-dictionaryobject")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
