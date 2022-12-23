using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using AutoFixture;

using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Configurations;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.OutOfProc
{
    public class StoreHttpTrigger
    {
        private readonly ILogger _logger;
        private readonly OpenApiSettings _openapi;
        private readonly Fixture _fixture;

        public StoreHttpTrigger(ILoggerFactory loggerFactory, OpenApiSettings openapi, Fixture fixture)
        {
            this._logger = loggerFactory.ThrowIfNullOrDefault().CreateLogger<PetHttpTrigger>();
            this._openapi = openapi.ThrowIfNullOrDefault();
            this._fixture = fixture.ThrowIfNullOrDefault();
        }

        [Function(nameof(StoreHttpTrigger.GetInventory))]
        [OpenApiOperation(operationId: "getInventory", tags: new[] { "store" }, Summary = "Returns pet inventories by status", Description = "This returns a map of status codes to quantities.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("api_key", SecuritySchemeType.ApiKey, Name = "api_key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Dictionary<string, int>), Description = "Successful operation")]
        public async Task<HttpResponseData> GetInventory(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "store/inventory")] HttpRequestData req)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var response = req.CreateResponse(HttpStatusCode.OK);

            var result = this._fixture.Create<Dictionary<string, int>>();

            await response.WriteAsJsonAsync(result).ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }

        [Function(nameof(StoreHttpTrigger.PlaceOrder))]
        [OpenApiOperation(operationId: "placeOrder", tags: new[] { "store" }, Summary = "Places an order for a pet", Description = "This places an order for a pet.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Order), Required = true, Description = "Order placed for purchasing the pet")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Order), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
        public async Task<HttpResponseData> PlaceOrder(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "store/order")] HttpRequestData req)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(this._fixture.Create<Order>()).ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }

        [Function(nameof(StoreHttpTrigger.GetOrderById))]
        [OpenApiOperation(operationId: "getOrderById", tags: new[] { "store" }, Summary = "Finds purchase order by ID", Description = "This finds purchase order by ID.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "orderId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of order that needs to be fetched", Description = "ID of order that needs to be fetched", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Order), Description = "Successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Order not found", Description = "Order not found")]
        public async Task<HttpResponseData> GetOrderById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "store/order/{orderId}")] HttpRequestData req, long orderId)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var response = req.CreateResponse(HttpStatusCode.OK);

            var order = this._fixture.Build<Order>().With(p => p.Id, orderId).Create();

            await response.WriteAsJsonAsync(order).ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }

        [Function(nameof(StoreHttpTrigger.DeleteOrder))]
        [OpenApiOperation(operationId: "deleteOrder", tags: new[] { "store" }, Summary = "Deletes purchase order by ID", Description = "For valid response try integer IDs with positive integer value. Negative or non - integer values will generate API errors.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "orderId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of order that needs to be deleted", Description = "ID of order that needs to be deleted", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Order not found", Description = "Order not found")]
        public async Task<HttpResponseData> DeleteOrder(
            [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "store/order/{orderId}")] HttpRequestData req, long orderId)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var response = req.CreateResponse(HttpStatusCode.OK);

            return await Task.FromResult(response).ConfigureAwait(false);
        }
    }
}
