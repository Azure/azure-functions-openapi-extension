using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp
{
    public class Post_ApplicationJson_DecimalType_HttpTrigger
    {
        [FunctionName(nameof(Post_ApplicationJson_DecimalType_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Post_ApplicationJson_DecimalType_HttpTrigger.Post_ApplicationJson_DecimalType), tags: new[] { "demical" })]
        [OpenApiRequestBody(contentType: "text/plain", bodyType: typeof(decimal), Required = true, Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(DecimalTypeObjectModel), Description = "The OK response")]

        public static async Task<IActionResult> Post_ApplicationJson_DecimalType(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "post-applicationjson-decimal")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }   
}
