using System;
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
using Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.InProc.Headers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.InProc
{
    public class UserHttpTrigger
    {
        private readonly ILogger<UserHttpTrigger> _logger;
        private readonly OpenApiSettings _openapi;
        private readonly Fixture _fixture;

        public UserHttpTrigger(ILogger<UserHttpTrigger> log, OpenApiSettings openapi, Fixture fixture)
        {
            this._logger = log.ThrowIfNullOrDefault();
            this._openapi = openapi.ThrowIfNullOrDefault();
            this._fixture = fixture.ThrowIfNullOrDefault();
        }

        [FunctionName(nameof(UserHttpTrigger.CreateUser))]
        [OpenApiOperation(operationId: "createUser", tags: new[] { "user" }, Summary = "Creates user", Description = "This can only be done by the logged in user.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(User), Required = true, Description = "Created user object")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "successful operation", Description = "successful operation")]
        public async Task<IActionResult> CreateUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "user")] HttpRequest req)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            return await Task.FromResult(new OkObjectResult(this._fixture.Create<User>())).ConfigureAwait(false);
        }

        [FunctionName(nameof(UserHttpTrigger.CreateUsersWithArrayInput))]
        [OpenApiOperation(operationId: "createUsersWithArrayInput", tags: new[] { "user" }, Summary = "Creates list of users with given input array", Description = "This Creates list of users with given input array.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(List<User>), Required = true, Description = "List of user object")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<User>), Summary = "successful operation", Description = "successful operation")]
        public async Task<IActionResult> CreateUsersWithArrayInput(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "user/createWithArray")] HttpRequest req)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            return await Task.FromResult(new OkObjectResult(this._fixture.Create<List<User>>())).ConfigureAwait(false);
        }

        [FunctionName(nameof(UserHttpTrigger.CreateUsersWithListInput))]
        [OpenApiOperation(operationId: "createUsersWithListInput", tags: new[] { "user" }, Summary = "Creates list of users with given input array", Description = "This Creates list of users with given input array.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(List<User>), Required = true, Description = "List of user object")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<User>), Summary = "successful operation", Description = "successful operation")]
        public async Task<IActionResult> CreateUsersWithListInput(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "user/createWithList")] HttpRequest req)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            return await Task.FromResult(new OkObjectResult(this._fixture.Create<List<User>>())).ConfigureAwait(false);
        }

        [FunctionName(nameof(UserHttpTrigger.LoginUser))]
        [OpenApiOperation(operationId: "loginUser", tags: new[] { "user" }, Summary = "Logs user into the system", Description = "This logs user into the system.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "username", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "The user name for login", Description = "The user name for login", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "password", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "The password for login in clear text", Description = "The password for login in clear text", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), CustomHeaderType = typeof(LoginUserResponseHeader), Summary = "successful operation", Description = "successful operation")]
        public async Task<IActionResult> LoginUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "user/login")] HttpRequest req)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            req.HttpContext.Response.Headers.Add("X-Rate-Limit", this._fixture.Create<int>().ToString());
            req.HttpContext.Response.Headers.Add("X-Expires-After", this._fixture.Create<DateTimeOffset>().ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ"));

            var result = new ContentResult()
            {
                StatusCode = (int)HttpStatusCode.OK,
                ContentType = "text/plain; charset=utf-8",
                Content = this._fixture.Create<string>(),
            };

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        [FunctionName(nameof(UserHttpTrigger.LogoutUser))]
        [OpenApiOperation(operationId: "logoutUser", tags: new[] { "user" }, Summary = "Logs out current logged in user session", Description = "This logs out current logged in user session.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Summary = "successful operation", Description = "successful operation")]
        public async Task<IActionResult> LogoutUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "user/logout")] HttpRequest req)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            return await Task.FromResult(new OkResult()).ConfigureAwait(false);
        }

        [FunctionName(nameof(UserHttpTrigger.GetUserByName))]
        [OpenApiOperation(operationId: "getUserByName", tags: new[] { "user" }, Summary = "Gets user by user name", Description = "This gets user by user name.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "username", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "The user name for login", Description = "The user name for login", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid username supplied", Description = "Invalid username supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
        public async Task<IActionResult> GetUserByName(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "user/{username:regex((?!^login$)(^.+$))}")] HttpRequest req, string username)
        {
            this._logger.LogInformation($"document title: {this._openapi.DocTitle}");

            var user = this._fixture.Build<User>().With(p => p.Username, username).Create();

            return await Task.FromResult(new OkObjectResult(user)).ConfigureAwait(false);
        }
    }
}
