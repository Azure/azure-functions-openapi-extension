using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using AutoFixture;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.InProc
{
    public class StoreHttpTrigger
    {
        private readonly ILogger<StoreHttpTrigger> _logger;
        private readonly OpenApiSettings _openapi;
        private readonly Fixture _fixture;

        public StoreHttpTrigger(ILogger<StoreHttpTrigger> log, OpenApiSettings openapi, Fixture fixture)
        {
            this._logger = log.ThrowIfNullOrDefault();
            this._openapi = openapi.ThrowIfNullOrDefault();
            this._fixture = fixture.ThrowIfNullOrDefault();
        }

        [FunctionName(nameof(StoreHttpTrigger.GetInventory))]
        [OpenApiOperation(operationId: "getInventory", tags: new[] { "store" }, Summary = "Returns pet inventories by status", Description = "This returns a map of status codes to quantities.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("api_key", SecuritySchemeType.ApiKey, Name = "api_key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Dictionary<string, int>), Description = "Successful operation")]
        public async Task<IActionResult> GetInventory(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "store/inventory")] HttpRequest req)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var result = this._fixture.Create<Dictionary<string, int>>();

            return await Task.FromResult(new OkObjectResult(result)).ConfigureAwait(false);
        }

        [FunctionName(nameof(StoreHttpTrigger.PlaceOrder))]
        [OpenApiOperation(operationId: "placeOrder", tags: new[] { "store" }, Summary = "Places an order for a pet", Description = "This places an order for a pet.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Order), Required = true, Description = "Order placed for purchasing the pet")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Order), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
        public async Task<IActionResult> PlaceOrder(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "store/order")] HttpRequest req)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            return await Task.FromResult(new OkObjectResult(this._fixture.Create<Order>())).ConfigureAwait(false);
        }

        [FunctionName(nameof(StoreHttpTrigger.GetOrderById))]
        [OpenApiOperation(operationId: "getOrderById", tags: new[] { "store" }, Summary = "Finds purchase order by ID", Description = "This finds purchase order by ID.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "orderId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of order that needs to be fetched", Description = "ID of order that needs to be fetched", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Order), Description = "Successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Order not found", Description = "Order not found")]
        public async Task<IActionResult> GetOrderById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "store/order/{orderId}")] HttpRequest req, long orderId)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var order = this._fixture.Build<Order>().With(p => p.Id, orderId).Create();

            return await Task.FromResult(new OkObjectResult(order)).ConfigureAwait(false);
        }

        [FunctionName(nameof(StoreHttpTrigger.DeleteOrder))]
        [OpenApiOperation(operationId: "deleteOrder", tags: new[] { "store" }, Summary = "Deletes purchase order by ID", Description = "For valid response try integer IDs with positive integer value. Negative or non - integer values will generate API errors.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "orderId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of order that needs to be deleted", Description = "ID of order that needs to be deleted", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Order not found", Description = "Order not found")]
        public async Task<IActionResult> DeleteOrder(
            [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "store/order/{orderId}")] HttpRequest req, long orderId)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            return await Task.FromResult(new OkResult()).ConfigureAwait(false);
        }
    }
}
