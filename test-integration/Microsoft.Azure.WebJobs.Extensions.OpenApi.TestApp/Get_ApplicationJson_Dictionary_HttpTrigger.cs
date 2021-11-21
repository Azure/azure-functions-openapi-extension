using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp
{
    public class Get_ApplicationJson_Dictionary_HttpTrigger
    {
        [FunctionName(nameof(Get_ApplicationJson_Dictionary_HttpTrigger.Get_ApplicationJson_Dictionary))]
        [OpenApiOperation(operationId: nameof(Get_ApplicationJson_Dictionary_HttpTrigger.Get_ApplicationJson_Dictionary), tags: new[] { "dictionary" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Dictionary<string,string>), Description = "The OK response")]
        public static async Task<IActionResult> Get_ApplicationJson_Dictionary(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-applicationjson-dictionary")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_ApplicationJson_Dictionary_HttpTrigger.Get_ApplicationJson_Dictionary_I))]
        [OpenApiOperation(operationId: nameof(Get_ApplicationJson_Dictionary_HttpTrigger.Get_ApplicationJson_Dictionary_I), tags: new[] { "dictionary" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IDictionary<string,int>), Description = "The OK response")]
        public static async Task<IActionResult> Get_ApplicationJson_Dictionary_I(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-applicationjson-dictionary-i")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_ApplicationJson_Dictionary_HttpTrigger.Get_ApplicationJson_Dictionary_Ireadonly))]
        [OpenApiOperation(operationId: nameof(Get_ApplicationJson_Dictionary_HttpTrigger.Get_ApplicationJson_Dictionary_Ireadonly), tags: new[] { "dictionary" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IReadOnlyDictionary<string,double>), Description = "The OK response")]
        public static async Task<IActionResult> Get_ApplicationJson_Dictionary_Ireadonly(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-applicationjson-dictionary-ireadonly")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(Get_ApplicationJson_Dictionary_HttpTrigger.Get_ApplicationJson_Dictionary_Key))]
        [OpenApiOperation(operationId: nameof(Get_ApplicationJson_Dictionary_HttpTrigger.Get_ApplicationJson_Dictionary_Key), tags: new[] { "dictionary" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(KeyValuePair<string,bool>), Description = "The OK response")]
        public static async Task<IActionResult> Get_ApplicationJson_Dictionary_Key(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-applicationjson-dictionary-key")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }

}
