using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3Static.ResponseHeaders;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3Static
{
    public static class DummyHttpTrigger
    {
        [FunctionName(nameof(DummyHttpTrigger.GetDummies))]
        [OpenApiOperation(operationId: "getDummies", tags: new[] { "dummy" }, Summary = "Gets the list of dummies", Description = "This gets the list of dummies.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("basic_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "Dummy name", Description = "Dummy name", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "switch", In = ParameterLocation.Query, Required = true, Type = typeof(bool), Summary = "Dummy switch", Description = "Dummy switch", Visibility = OpenApiVisibilityType.Important, Deprecated = true)]
        [OpenApiParameter(name: "onoff", In = ParameterLocation.Path, Required = true, Type = typeof(StringEnum), Summary = "Dummy switch", Description = "Dummy switch", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<DummyResponseModel>), Summary = "List of the dummy responses", Description = "This returns the list of dummy responses", CustomHeaderType = typeof(DummyResponseHeader))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Name not found", Description = "Name parameter is not found", CustomHeaderType = typeof(DummyResponseHeader))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid switch", Description = "Switch parameter is not valid", CustomHeaderType = typeof(DummyResponseHeader))]
        public static async Task<IActionResult> GetDummies(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "dummies/{onoff}")] HttpRequest req,
            string onoff,
            ILogger log)
        {
            var content = new List<DummyResponseModel>()
            {
                new DummyResponseModel(),
            };
            var result = new OkObjectResult(content);

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(DummyHttpTrigger.AddDummy))]
        [OpenApiOperation(operationId: "addDummy", tags: new[] { "dummy" }, Summary = "Adds a dummy", Description = "This adds a dummy.", Visibility = OpenApiVisibilityType.Advanced, Deprecated = true)]
        [OpenApiSecurity("openid_auth", SecuritySchemeType.OpenIdConnect, OpenIdConnectUrl = "https://example.com/.well-known/openid-configuration", OpenIdConnectScopes = "pets_read, pets_write, admin")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(DummyRequestModel), Required = true, Description = "Dummy request model")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DummyResponseModel), Summary = "Dummy response", Description = "This returns the dummy response", CustomHeaderType = typeof(DummyResponseHeader))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid request payload", Description = "Request payload is not valid", CustomHeaderType = typeof(DummyResponseHeader))]
        public static async Task<IActionResult> AddDummy(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "dummies")] HttpRequest req,
            ILogger log)
        {
            var content = new DummyResponseModel();
            var result = new OkObjectResult(content);

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(DummyHttpTrigger.UpdateDummies))]
        [OpenApiOperation(operationId: "updateDummies", tags: new[] { "dummy" }, Summary = "Updates a list of dummies", Description = "This updates a list of dummies.", Visibility = OpenApiVisibilityType.Advanced)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(DummyListModel), Required = true, Description = "Dummy list model")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<DummyStringModel>), Summary = "Dummy response", Description = "This returns the dummy response", CustomHeaderType = typeof(DummyResponseHeader))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DummyStringModel), Summary = "Dummy response", Description = "This returns the dummy response", Deprecated = true, CustomHeaderType = typeof(DummyResponseHeader))]
        public static async Task<IActionResult> UpdateDummies(
            [HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "dummies")] HttpRequest req,
            ILogger log)
        {
            var content = new List<DummyStringModel>();
            var result = new OkObjectResult(content);

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
