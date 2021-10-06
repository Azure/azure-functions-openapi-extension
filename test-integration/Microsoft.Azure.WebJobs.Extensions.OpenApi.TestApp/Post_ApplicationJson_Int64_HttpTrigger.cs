using System;
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
    public class Post_ApplicationJson_Int64_HttpTrigger
    {
        [FunctionName(nameof(Post_ApplicationJson_Int64_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_Int64_HttpTrigger.Post_ApplicationJson_Int64Object), tags: new[] { "int64" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(Int64), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Int64ObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_Int64Object(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-int64")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
