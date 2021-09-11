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
    public class Get_ApplicationJson_DecimalType_HttpTrigger
    {
        [FunctionName(nameof(Get_ApplicationJson_DecimalType_HttpTrigger))]
        [OpenApiOperation(operationId: nameof(Get_ApplicationJson_DecimalType_HttpTrigger.Get_ApplicationJson_DecimalType), tags: new[] { "dataType" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DecimalTypeObjectModel), Description = "The OK response")]
        public static async Task<IActionResult> Get_ApplicationJson_DecimalType(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "get-applicationjson-datatype")] HttpRequest req,
            ILogger log)
        {
            var result = new OkResult();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
