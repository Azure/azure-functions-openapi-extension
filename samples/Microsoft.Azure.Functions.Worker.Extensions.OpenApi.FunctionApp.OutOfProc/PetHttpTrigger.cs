using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using AutoFixture;

using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Configurations;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.OutOfProc.SecurityFlows;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.OutOfProc
{
    public class PetHttpTrigger
    {
        private readonly ILogger _logger;
        private readonly OpenApiSettings _openapi;
        private readonly Fixture _fixture;

        public PetHttpTrigger(ILoggerFactory loggerFactory, OpenApiSettings openapi, Fixture fixture)
        {
            this._logger = loggerFactory.ThrowIfNullOrDefault().CreateLogger<PetHttpTrigger>();
            this._openapi = openapi.ThrowIfNullOrDefault();
            this._fixture = fixture.ThrowIfNullOrDefault();
        }

        [Function(nameof(PetHttpTrigger.UpdatePet))]
        [OpenApiOperation(operationId: "updatePet", tags: new[] { "pet" }, Summary = "Update an existing pet", Description = "This updates an existing pet.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("petstore_auth", SecuritySchemeType.OAuth2, Flows = typeof(PetStoreAuth))]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Pet), Required = true, Description = "Pet object that needs to be updated to the store")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Pet), Summary = "Pet details updated", Description = "Pet details updated")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Pet not found", Description = "Pet not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<HttpResponseData> UpdatePet(
            [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "pet")] HttpRequestData req)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(this._fixture.Create<Pet>()).ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }

        [Function(nameof(PetHttpTrigger.AddPet))]
        [OpenApiOperation(operationId: "addPet", tags: new[] { "pet" }, Summary = "Add a new pet to the store", Description = "This add a new pet to the store.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("petstore_auth", SecuritySchemeType.OAuth2, Flows = typeof(PetStoreAuth))]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Pet), Required = true, Description = "Pet object that needs to be added to the store")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Pet), Summary = "New pet details added", Description = "New pet details added")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
        public async Task<HttpResponseData> AddPet(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "pet")] HttpRequestData req)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(this._fixture.Create<Pet>()).ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }

        [Function(nameof(PetHttpTrigger.FindByStatus))]
        [OpenApiOperation(operationId: "findPetsByStatus", tags: new[] { "pet" }, Summary = "Finds Pets by status", Description = "Multiple status values can be provided with comma separated strings.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("petstore_auth", SecuritySchemeType.OAuth2, Flows = typeof(PetStoreAuth))]
        [OpenApiParameter(name: "status", In = ParameterLocation.Query, Required = true, Type = typeof(List<PetStatus>), Explode = true, Summary = "Pet status value", Description = "Status values that need to be considered for filter", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Pet>), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid status value", Description = "Invalid status value")]
        public async Task<HttpResponseData> FindByStatus(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "pet/findByStatus")] HttpRequestData req)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var response = req.CreateResponse(HttpStatusCode.OK);

            var status = req.Query("status")
                            .Select(p =>
                            {
                                var converted = Enum.TryParse<PetStatus>(p, ignoreCase: true, out var result) ? result : PetStatus.Available;
                                return converted;
                            })
                            .ToList();
            var pets = this._fixture.Create<List<Pet>>().Where(p => status.Contains(p.Status));

            await response.WriteAsJsonAsync(pets).ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }

        [Function(nameof(PetHttpTrigger.FindByTags))]
        [OpenApiOperation(operationId: "findPetsByTags", tags: new[] { "pet" }, Summary = "Finds Pets by tags", Description = "Muliple tags can be provided with comma separated strings.", Deprecated = true, Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("petstore_auth", SecuritySchemeType.OAuth2, Flows = typeof(PetStoreAuth))]
        [OpenApiParameter(name: "tags", In = ParameterLocation.Query, Required = true, Type = typeof(List<string>), Explode = true, Summary = "Tags to filter by", Description = "Tags to filter by", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Pet>), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid tag value", Description = "Invalid tag value")]
        public async Task<HttpResponseData> FindByTags(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "pet/findByTags")] HttpRequestData req)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var response = req.CreateResponse(HttpStatusCode.OK);

            var tags = req.Query("tags")
                          .Select(p =>
                          {
                              var tag = this._fixture.Build<Tag>().With(q => q.Name, p).Create();
                              return tag;
                          })
                          .ToList();
            var pets = this._fixture.Create<List<Pet>>()
                           .Select(p =>
                           {
                               p.Tags = tags;
                               return p;
                           });

            await response.WriteAsJsonAsync(pets).ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }

        [Function(nameof(PetHttpTrigger.GetPetById))]
        [OpenApiOperation(operationId: "getPetById", tags: new[] { "pet" }, Summary = "Find pet by ID", Description = "Returns a single pet.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("api_key", SecuritySchemeType.ApiKey, Name = "api_key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiParameter(name: "petId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of pet to return", Description = "ID of pet to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Pet), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Pet not found", Description = "Pet not found")]
        public async Task<HttpResponseData> GetPetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "pet/{petId}")] HttpRequestData req, long petId)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var response = req.CreateResponse(HttpStatusCode.OK);

            var pet = this._fixture.Build<Pet>().With(p => p.Id, petId).Create();

            await response.WriteAsJsonAsync(pet).ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }

        [Function(nameof(PetHttpTrigger.UpdatePetWithForm))]
        [OpenApiOperation(operationId: "updatePetWithForm", tags: new[] { "pet" }, Summary = "Updates a pet in the store with form data", Description = "This updates a pet in the store with form data.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("petstore_auth", SecuritySchemeType.OAuth2, Flows = typeof(PetStoreAuth))]
        [OpenApiParameter(name: "petId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of pet that needs to be updated", Description = "ID of pet that needs to be updated", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/x-www-form-urlencoded", bodyType: typeof(PetUrlForm), Required = true, Description = "Pet object that needs to be added to the store")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Pet), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
        public async Task<HttpResponseData> UpdatePetWithForm(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "pet/{petId}")] HttpRequestData req, long petId)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var response = req.CreateResponse(HttpStatusCode.OK);

            var pet = this._fixture.Build<Pet>().With(p => p.Id, petId).Create();

            await response.WriteAsJsonAsync(this._fixture.Create<Pet>()).ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }

        [Function(nameof(PetHttpTrigger.DeletePet))]
        [OpenApiOperation(operationId: "deletePet", tags: new[] { "pet" }, Summary = "Deletes a pet", Description = "This deletes a pet.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("petstore_auth", SecuritySchemeType.OAuth2, Flows = typeof(PetStoreAuth))]
        [OpenApiParameter(name: "api_key", In = ParameterLocation.Header, Type = typeof(string), Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "petId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "Pet id to delete", Description = "Pet id to delete", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Pet not found", Description = "Pet not found")]
        public async Task<HttpResponseData> DeletePet(
            [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "pet/{petId}")] HttpRequestData req, long petId)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var response = req.CreateResponse(HttpStatusCode.OK);

            return await Task.FromResult(response).ConfigureAwait(false);
        }

        [Function(nameof(PetHttpTrigger.UploadFile))]
        [OpenApiOperation(operationId: "uploadFile", tags: new[] { "pet" }, Summary = "Uploads an image", Description = "This uploads an image.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("petstore_auth", SecuritySchemeType.OAuth2, Flows = typeof(PetStoreAuth))]
        [OpenApiParameter(name: "petId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of pet to update", Description = "ID of pet to update", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(PetFormData))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ApiResponse), Summary = "successful operation", Description = "successful operation")]
        public async Task<HttpResponseData> UploadFile(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "pet/{petId}/uploadImage")] HttpRequestData req, long petId)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(this._fixture.Create<ApiResponse>()).ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }
    }
}
