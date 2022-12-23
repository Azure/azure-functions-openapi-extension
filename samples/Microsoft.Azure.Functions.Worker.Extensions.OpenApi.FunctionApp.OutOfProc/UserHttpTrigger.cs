using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using AutoFixture;

using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Configurations;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.OutOfProc.Headers;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.OutOfProc
{
    public class UserHttpTrigger
    {
        private readonly ILogger _logger;
        private readonly OpenApiSettings _openapi;
        private readonly Fixture _fixture;

        public UserHttpTrigger(ILoggerFactory loggerFactory, OpenApiSettings openapi, Fixture fixture)
        {
            this._logger = loggerFactory.ThrowIfNullOrDefault().CreateLogger<PetHttpTrigger>();
            this._openapi = openapi.ThrowIfNullOrDefault();
            this._fixture = fixture.ThrowIfNullOrDefault();
        }

        [Function(nameof(UserHttpTrigger.CreateUser))]
        [OpenApiOperation(operationId: "createUser", tags: new[] { "user" }, Summary = "Creates user", Description = "This can only be done by the logged in user.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(User), Required = true, Description = "Created user object")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "successful operation", Description = "successful operation")]
        public async Task<HttpResponseData> CreateUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "user")] HttpRequestData req)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(this._fixture.Create<User>()).ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }

        [Function(nameof(UserHttpTrigger.CreateUsersWithArrayInput))]
        [OpenApiOperation(operationId: "createUsersWithArrayInput", tags: new[] { "user" }, Summary = "Creates list of users with given input array", Description = "This Creates list of users with given input array.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(List<User>), Required = true, Description = "List of user object")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<User>), Summary = "successful operation", Description = "successful operation")]
        public async Task<HttpResponseData> CreateUsersWithArrayInput(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "user/createWithArray")] HttpRequestData req)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(this._fixture.Create<List<User>>()).ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }

        [Function(nameof(UserHttpTrigger.CreateUsersWithListInput))]
        [OpenApiOperation(operationId: "createUsersWithListInput", tags: new[] { "user" }, Summary = "Creates list of users with given input array", Description = "This Creates list of users with given input array.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(List<User>), Required = true, Description = "List of user object")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<User>), Summary = "successful operation", Description = "successful operation")]
        public async Task<HttpResponseData> CreateUsersWithListInput(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "user/createWithList")] HttpRequestData req)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(this._fixture.Create<List<User>>()).ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }

        [Function(nameof(UserHttpTrigger.LoginUser))]
        [OpenApiOperation(operationId: "loginUser", tags: new[] { "user" }, Summary = "Logs user into the system", Description = "This logs user into the system.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "username", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "The user name for login", Description = "The user name for login", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "password", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "The password for login in clear text", Description = "The password for login in clear text", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), CustomHeaderType = typeof(LoginUserResponseHeader), Summary = "successful operation", Description = "successful operation")]
        public async Task<HttpResponseData> LoginUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "user/login")] HttpRequestData req)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.Headers.Add("X-Rate-Limit", this._fixture.Create<int>().ToString());
            response.Headers.Add("X-Expires-After", this._fixture.Create<DateTimeOffset>().ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ"));

            await response.WriteStringAsync(this._fixture.Create<string>()).ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }

        [Function(nameof(UserHttpTrigger.LogoutUser))]
        [OpenApiOperation(operationId: "logoutUser", tags: new[] { "user" }, Summary = "Logs out current logged in user session", Description = "This logs out current logged in user session.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Summary = "successful operation", Description = "successful operation")]
        public async Task<HttpResponseData> LogoutUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "user/logout")] HttpRequestData req)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var response = req.CreateResponse(HttpStatusCode.OK);

            return await Task.FromResult(response).ConfigureAwait(false);
        }

        [Function(nameof(UserHttpTrigger.GetUserByName))]
        [OpenApiOperation(operationId: "getUserByName", tags: new[] { "user" }, Summary = "Gets user by user name", Description = "This gets user by user name.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "username", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "The user name for login", Description = "The user name for login", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid username supplied", Description = "Invalid username supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
        public async Task<HttpResponseData> GetUserByName(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "user/{username:regex((?!^login$)(^.+$))}")] HttpRequestData req, string username)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var response = req.CreateResponse(HttpStatusCode.OK);

            var user = this._fixture.Build<User>().With(p => p.Username, username).Create();

            await response.WriteAsJsonAsync(user).ConfigureAwait(false);

            return await Task.FromResult(response).ConfigureAwait(false);
        }
    }
}
