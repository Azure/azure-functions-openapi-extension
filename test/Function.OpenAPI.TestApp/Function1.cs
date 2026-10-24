using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Function.OpenAPI.TestApp;

public class Function1(ILogger<Function1> logger)
{
    [Function("Function1")]
    [OpenApiOperation(operationId: nameof(Function1), tags: new[] { "string" })]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "get-textplain-string")] HttpRequest req)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}
