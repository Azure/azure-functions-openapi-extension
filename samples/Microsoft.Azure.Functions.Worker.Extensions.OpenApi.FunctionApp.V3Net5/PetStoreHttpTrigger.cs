using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.V3Net5.SecurityFlows;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.V3Net5
{
    public class PetStoreHttpTrigger
    {
        [Function(nameof(PetStoreHttpTrigger.AddPet))]
        [OpenApiOperation(operationId: "addPet", tags: new[] { "pet" }, Summary = "Add a new pet to the store", Description = "This add a new pet to the store.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("petstore_auth", SecuritySchemeType.OAuth2, Flows = typeof(PetStoreAuth))]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Pet), Required = true, Description = "Pet object that needs to be added to the store")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Pet), Summary = "New pet details added", Description = "New pet details added")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
        public static async Task<HttpResponseData> AddPet(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "pet")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            await response.WriteStringAsync($"Welcome to Azure Functions!").ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }

        [Function(nameof(PetStoreHttpTrigger.UpdatePet))]
        [OpenApiOperation(operationId: "updatePet", tags: new[] { "pet" }, Summary = "Update an existing pet", Description = "This updates an existing pet.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("petstore_auth", SecuritySchemeType.OAuth2, Flows = typeof(PetStoreAuth))]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Pet), Required = true, Description = "Pet object that needs to be updated to the store")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Pet), Summary = "Pet details updated", Description = "Pet details updated")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Pet not found", Description = "Pet not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public static async Task<HttpResponseData> UpdatePet(
            [HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "pet")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            await response.WriteStringAsync($"Welcome to Azure Functions!").ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }

        [Function(nameof(PetStoreHttpTrigger.FindByStatus))]
        [OpenApiOperation(operationId: "findPetsByStatus", tags: new[] { "pet" }, Summary = "Finds Pets by status", Description = "Multiple status values can be provided with comma separated strings.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("petstore_auth", SecuritySchemeType.OAuth2, Flows = typeof(PetStoreAuth))]
        [OpenApiParameter(name: "status", In = ParameterLocation.Query, Required = true, Type = typeof(List<PetStatus>), Summary = "Pet status value", Description = "Status values that need to be considered for filter", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Pet>), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid status value", Description = "Invalid status value")]
        public async Task<HttpResponseData> FindByStatus(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "pet/findByStatus")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            await response.WriteStringAsync($"Welcome to Azure Functions!").ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }

        [Function(nameof(PetStoreHttpTrigger.FindByTags))]
        [OpenApiOperation(operationId: "findPetsByTags", tags: new[] { "pet" }, Summary = "Finds Pets by tags", Description = "Muliple tags can be provided with comma separated strings.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("petstore_auth", SecuritySchemeType.OAuth2, Flows = typeof(PetStoreAuth))]
        [OpenApiParameter(name: "tags", In = ParameterLocation.Query, Required = true, Type = typeof(List<string>), Summary = "Tags to filter by", Description = "Tags to filter by", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Pet>), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid tag value", Description = "Invalid tag value")]
        public async Task<HttpResponseData> FindByTags(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "pet/findByTags")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            await response.WriteStringAsync($"Welcome to Azure Functions!").ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }

        [Function(nameof(PetStoreHttpTrigger.GetPetById))]
        [OpenApiOperation(operationId: "getPetById", tags: new[] { "pet" }, Summary = "Find pet by ID", Description = "Returns a single pet.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("api_key", SecuritySchemeType.ApiKey, Name = "api_key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiParameter(name: "petId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of pet to return", Description = "ID of pet to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Pet), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Pet not found", Description = "Pet not found")]
        public async Task<HttpResponseData> GetPetById(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "pet/{petId}")] HttpRequestData req,
            long petId,
            FunctionContext executionContext)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            await response.WriteStringAsync($"Welcome to Azure Functions!").ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }
    }
}
