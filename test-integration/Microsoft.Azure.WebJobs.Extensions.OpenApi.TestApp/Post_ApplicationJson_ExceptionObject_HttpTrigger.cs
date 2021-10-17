using System;
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
    public class Post_ApplicationJson_ExceptionObject_HttpTrigger
    {
        [FunctionName(nameof(Post_ApplicationJson_ExceptionObject_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_ExceptionObject_HttpTrigger.Post_ApplicationJson_ExceptionObject), tags: new[] { "exception" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(InvalidOperationException), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ExceptionObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Post_ApplicationJson_ExceptionObject(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-exception")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
