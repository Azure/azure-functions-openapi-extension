# Securing Azure Functions Endpoints through OpenAPI Auth #

[Azure security baseline for Azure Functions][az funcapp security baseline] well describes the security consideration in general for your Azure Functions application development. In addition to that, Azure Functions offers a built-in authentication method through the functions key. If you use this OpenAPI extension for Azure Functions, you can define the endpoint authentication and authorisation for each API endpoint in various ways. You can even try them through the Swagger UI page.

> **[NOTE]**
> 
> If you want to see a working example, please visit [this repository](https://github.com/devkimchi/azure-functions-oauth-authentications-via-swagger-ui).

## OpenAPI Spec for Authentication ##

It could be a good idea to take a look at the [authentication spec defined in OpenAPI][openapi spec security] before going further.

* `type`: defines what type of authentication method will be used. Currently, it accepts `API Key`, `HTTP`, `OAuth2`, and `OpenID Connect`. But, the OpenAPI v2 spec doesn't support the OpenID Connect.
* `name`: declares the auth key name. It's required for API Key.
* `in`: defines the location of the auth key. It's required for API Key and accepts `query`, `header`, or `cookie`.
* `scheme`: declares the auth scheme. It's required for HTTP auth and accepts either `Basic` or `Bearer`.
* `bearerFormat`: uses `JWT` in most cases when using the `Bearer` token through the HTTP auth.
* `flows`: is required for the `OAuth2` auth. Its value can be `implicit`, `password`, `clientCredentials`, or `authorizationCode`.
* `openIdConnectUrl`: is necessary for the `OpenID Connect` auth. However, it is advised to use either `OAuth2` or `Bearer` auth for the OpenAPI v2 spec.

Based on the understandings above, let's apply the different auth approach to Azure Function endpoints through the OpenAPI extension.

> **[NOTE]**
> 
> The sample code snippet described in this document uses the in-proc worker. However, the out-of-proc worker uses the same approach.


## Known Limitations ##

There are a couple of limitations around using the OAuth2 authN flows on Swagger UI.

* OAuth2 authorisation code flow: It needs the PKCE certificate that supports from the OpenAPI spec v3.1.0
* OAuth2 client credentials flow: It needs auth server/daemon.


## APK Key in Querystring ##

This is the built-in feature of Azure Functions. Let's take a look at the code below. If you installed the OpenAPI extension, you could add the decorators. Spot on the `OpenApiSecurityAttribute(...)` decorator, which sets the value.

* `Type`: `SecuritySchemeType.ApiKey`
* `In`: `OpenApiSecurityLocationType.Query`
* `Name`: `code`

```csharp
public static class ApiKeyInQueryAuthFlowHttpTrigger
{
    [FunctionName(nameof(ApiKeyInQueryAuthFlowHttpTrigger))]
    [OpenApiOperation(operationId: "apikey.query", tags: new[] { "apikey" }, Summary = "API Key authentication code flow via querystring", Description = "This shows the API Key authentication code flow via querystring", Visibility = OpenApiVisibilityType.Important)]

    // ⬇️⬇️⬇️ API key in querystring ⬇️⬇️⬇️
    [OpenApiSecurity("apikeyquery_auth",
                     SecuritySchemeType.ApiKey,
                     In = OpenApiSecurityLocationType.Query,
                     Name = "code")]
    // ⬆️⬆️⬆️ API key in querystring ⬆️⬆️⬆️

    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Dictionary<string, string>), Summary = "successful operation", Description = "successful operation")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "GET", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        // ⬇️⬇️⬇️ Check your auth key from querystring ⬇️⬇️⬇️
        var queries = req.Query.ToDictionary(q => q.Key, q => (string) q.Value);
        // ⬆️⬆️⬆️ Check your auth key from querystring ⬆️⬆️⬆️

        var result = new OkObjectResult(queries);

        return await Task.FromResult(result).ConfigureAwait(false);
    }
}
```

Run the function app, and you will see the Swagger UI page.

![Swagger UI - Query][image-01]

Click the lock button on the right-hand side to enter the API key value. This value will be appended to the querystring parameter.

![Swagger UI - Query - API Key][image-02]

The result screen shows the API key passed through the querystring parameter, `code`.

![Swagger UI - Query - Result][image-03]


## API Key in Request Header ##

It's also the Azure Function's built-in feature. This time, set the value of the `OpenApiSecurityAttribute(...)` decorator like below (line #6-9).

* `Type`: `SecuritySchemeType.ApiKey`
* `In`: `OpenApiSecurityLocationType.Header`
* `Name`: `x-functions-key`

```csharp
public static class ApiKeyInHeaderAuthFlowHttpTrigger
{
    [FunctionName(nameof(ApiKeyInHeaderAuthFlowHttpTrigger))]
    [OpenApiOperation(operationId: "apikey.header", tags: new[] { "apikey" }, Summary = "API Key authentication code flow via header", Description = "This shows the API Key authentication code flow via header", Visibility = OpenApiVisibilityType.Important)]

    // ⬇️⬇️⬇️ API key in request header ⬇️⬇️⬇️
    [OpenApiSecurity("apikeyheader_auth",
                     SecuritySchemeType.ApiKey,
                     In = OpenApiSecurityLocationType.Header,
                     Name = "x-functions-key")]
    // ⬆️⬆️⬆️ API key in request header ⬆️⬆️⬆️

    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Dictionary<string, string>), Summary = "successful operation", Description = "successful operation")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "GET", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        // ⬇️⬇️⬇️ Check your auth key from request header ⬇️⬇️⬇️
        var headers = req.Headers.ToDictionary(q => q.Key, q => (string) q.Value);
        // ⬆️⬆️⬆️ Check your auth key from request header ⬆️⬆️⬆️

        var result = new OkObjectResult(headers);

        return await Task.FromResult(result).ConfigureAwait(false);
    }
}
```

Run the function app and see the Swagger UI page.

![Swagger UI - Header][image-04]

If you want to authenticate the endpoint, enter the API key value to the field, labelled as `x-functions-key`.

![Swagger UI - Header - API Key][image-05]

As a result, the API key was sent through the request header, `x-functions-key`.

![Swagger UI - Header - Result][image-06]


## Basic Auth Token ##

Let's use the Basic auth token this time. Set the property values of `OpenApiSecurityAttribute(...)` (line #6-8).

* `Type`: `SecuritySchemeType.Http`
* `Scheme`: `OpenApiSecuritySchemeType.Basic`

As this is not the built-in feature, you can use this approach for additional auth methods or replace the built-in feature. If you don't want to use the built-in API key, you should set the auth level value of the `HttpTrigger` binding to `AuthorizationLevel.Anonymous` (line #12).

```csharp
public static class HttpBasicAuthFlowHttpTrigger
{
    [FunctionName(nameof(HttpBasicAuthFlowHttpTrigger))]
    [OpenApiOperation(operationId: "http.basic", tags: new[] { "http" }, Summary = "Basic authentication token flow via header", Description = "This shows the basic authentication token flow via header", Visibility = OpenApiVisibilityType.Important)]

    // ⬇️⬇️⬇️ Basic auth token ⬇️⬇️⬇️
    [OpenApiSecurity("basic_auth",
                     SecuritySchemeType.Http,
                     Scheme = OpenApiSecuritySchemeType.Basic)]
    // ⬆️⬆️⬆️ Basic auth token ⬆️⬆️⬆️

    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Dictionary<string, string>), Summary = "successful operation", Description = "successful operation")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        // ⬇️⬇️⬇️ Check your auth header ⬇️⬇️⬇️
        var headers = req.Headers.ToDictionary(q => q.Key, q => (string) q.Value);
        // ⬆️⬆️⬆️ Check your auth header ⬆️⬆️⬆️

        var result = new OkObjectResult(headers);

        return await Task.FromResult(result).ConfigureAwait(false);
    }
}
```

Run the app to see the Swagger UI like below.

![Swagger UI - Basic Auth][image-07]

To authenticate your endpoint, you should enter the Username and Password, added to the `Authorization` header.

![Swagger UI - Basic Auth - Details][image-08]

The result screen shows the request header of `Authorization` with the base64 encoded value.

![Swagger UI - Basis Auth - Result][image-09]

Then, you should validate the auth details with your custom logic.


## Bearer Auth Token ##

Similarly, this time, let's use the Bearer auth token. Set the property values of `OpenApiSecurityAttribute(...)` (line #5).

* `Type`: `SecuritySchemeType.Http`
* `Scheme`: `OpenApiSecuritySchemeType.Bearer`
* `BearerFormat`: `JWT`

You now know how to set the auth level of the `HttpTrigger` binding to `AuthorizationLevel.Anonymous` (line #13).

```csharp
public static class HttpBearerAuthFlowHttpTrigger
{
    [FunctionName(nameof(HttpBearerAuthFlowHttpTrigger))]
    [OpenApiOperation(operationId: "http.bearer", tags: new[] { "http" }, Summary = "Bearer authentication token flow via header", Description = "This shows the bearer authentication token flow via header", Visibility = OpenApiVisibilityType.Important)]

    // ⬇️⬇️⬇️ Bearer auth token ⬇️⬇️⬇️
    [OpenApiSecurity("bearer_auth",
                     SecuritySchemeType.Http,
                     Scheme = OpenApiSecuritySchemeType.Bearer,
                     BearerFormat = "JWT")]
    // ⬆️⬆️⬆️ Bearer auth token ⬆️⬆️⬆️

    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Dictionary<string, string>), Summary = "successful operation", Description = "successful operation")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        // ⬇️⬇️⬇️ Check your claims ⬇️⬇️⬇️
        var headers = req.Headers.ToDictionary(q => q.Key, q => (string) q.Value);
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(headers["Authorization"].Split(' ').Last());
        var claims = token.Claims.Select(p => p.ToString());
        var content = new { headers = headers, claims = claims };
        // ⬆️⬆️⬆️ Check your claims ⬆️⬆️⬆️

        var result = new OkObjectResult(content);

        return await Task.FromResult(result).ConfigureAwait(false);
    }
}
```

Run the function app and see the Swagger UI page.

![Swagger UI - Bearer Auth][image-10]

During the authentication, you are asked to enter the Bearer token value. The `Authorization` header will add the value.

![Swagger UI - Bearer Auth - Details][image-11]

The result screen shows the JWT value in the `Authorization` header.

![Swagger UI - Bearer Auth - Result][image-12]

You should decode the JWT and find the appropriate claims and validate them for further processing.


## OAuth2 Implicit Auth Flow ##

Although there are many ways in the OAuth2 authentication flow, I'm going to use the [Implicit flow][az ad oauth2 implicit flow] for this time. Set the properties of `OpenApiSecurityAttribute(...)` (line #6-8).

* `Type`: `SecuritySchemeType.OAuth2`
* `Flows`: `ImplicitAuthFlow`

Auth level is also set to `Anonymous` (line #12).

```csharp
public static class OAuthImplicitAuthFlowHttpTrigger
{
    [FunctionName(nameof(OAuthImplicitAuthFlowHttpTrigger))]
    [OpenApiOperation(operationId: "oauth.flows.implicit", tags: new[] { "oauth" }, Summary = "OAuth implicit flows", Description = "This shows the OAuth implicit flows", Visibility = OpenApiVisibilityType.Important)]

    // ⬇️⬇️⬇️ OAuth2 implicit auth flow ⬇️⬇️⬇️
    [OpenApiSecurity("implicit_auth",
                     SecuritySchemeType.OAuth2,
                     Flows = typeof(ImplicitAuthFlow))]
    // ⬆️⬆️⬆️ OAuth2 implicit auth flow ⬆️⬆️⬆️

    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<string>), Summary = "successful operation", Description = "successful operation")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        // ⬇️⬇️⬇️ Check your claims ⬇️⬇️⬇️
        var headers = req.Headers.ToDictionary(p => p.Key, p => (string) p.Value);
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(headers["Authorization"].Split(' ').Last());
        var claims = token.Claims.Select(p => p.ToString());
        // ⬆️⬆️⬆️ Check your claims ⬆️⬆️⬆️

        var result = new OkObjectResult(claims);

        return await Task.FromResult(result).ConfigureAwait(false);
    }
}
```

You can see `ImplicitAuthFlow` as the flow type. Since it uses [Azure Active Directory][az ad], it sets `AuthorizationUrl`, `RefreshUrl`, and `Scopes` values. It also takes the [single tenant type][az ad tenancy], which requires the tenant ID (line #3-6, 10, 14-15). `Scopes` has the default value (line #17).

```csharp
public class ImplicitAuthFlow : OpenApiOAuthSecurityFlows
{
    private const string AuthorisationUrl =
        "https://login.microsoftonline.com/{0}/oauth2/v2.0/authorize";
    private const string RefreshUrl =
        "https://login.microsoftonline.com/{0}/oauth2/v2.0/token";

    public ImplicitAuthFlow()
    {
        var tenantId = Environment.GetEnvironmentVariable("OpenApi__Auth__TenantId");

        this.Implicit = new OpenApiOAuthFlow()
        {
            AuthorizationUrl = new Uri(string.Format(AuthorisationUrl, tenantId)),
            RefreshUrl = new Uri(string.Format(RefreshUrl, tenantId)),

            Scopes = { { "https://graph.microsoft.com/.default", "Default scope defined in the app" } }
        };
    }
}
```

Run the function app and check the Swagger UI page.

![Swagger UI - OAuth2 Implicit Auth][image-13]

When you click the lock button, it asks you to enter the client ID value, redirecting you to sign in to Azure Active Directory. Then, you will get the access token.

![Swagger UI - OAuth2 Implicit Auth - Details][image-14]

The result shows the `Authorization` header with the access token in the JWT format.

![Swagger UI - OAuth2 Implicit Auth - Result][image-15]

That JWT is now decoded and verified for further processing.


## OpenID Connect Auth Flow ##

Finally, let's use the [OpenID Connect auth flow][az ad oidc flow]. `OpenApiSecurityAttribute(...)` contains the following definitions (line #6-9).

* `Type`: `SecuritySchemeType.OpenIdConnect`
* `OpenIdConnectUrl`: `https://login.microsoftonline.com/{tenant_id}/v2.0/.well-known/openid-configuration`
* `OpenIdConnectScopes`: `openid,profile`

The `{tenant_id}` value, of course, should be replaced with the real tenant ID. With this OpenID Connect URL, it automatically discovers the OAuth2 auth flows. Then, set the auth level to `Anonymous` (line #12).

```csharp
public static class OpenIDConnectAuthFlowHttpTrigger
{
    [FunctionName(nameof(OpenIDConnectAuthFlowHttpTrigger))]
    [OpenApiOperation(operationId: "openidconnect", tags: new[] { "oidc" }, Summary = "OpenID Connect auth flows", Description = "This shows the OpenID Connect auth flows", Visibility = OpenApiVisibilityType.Important)]

    // ⬇️⬇️⬇️ OpenIDConnect auth flow ⬇️⬇️⬇️
    [OpenApiSecurity("oidc_auth",
                     SecuritySchemeType.OpenIdConnect,
                     OpenIdConnectUrl = "https://login.microsoftonline.com/{tenant_id}/v2.0/.well-known/openid-configuration",
                     OpenIdConnectScopes = "openid,profile")]
    // ⬆️⬆️⬆️ OpenIDConnect auth flow ⬆️⬆️⬆️

    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<string>), Summary = "successful operation", Description = "successful operation")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        // ⬇️⬇️⬇️ Check your claims ⬇️⬇️⬇️
        var headers = req.Headers.ToDictionary(p => p.Key, p => (string) p.Value);
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(headers["Authorization"].Split(' ').Last());
        var claims = token.Claims.Select(p => p.ToString());
        var content = new { headers = headers, claims = claims };
        // ⬆️⬆️⬆️ Check your claims ⬆️⬆️⬆️

        var result = new OkObjectResult(content);

        return await Task.FromResult(result).ConfigureAwait(false);
    }
}
```

Run the function app and find the Swagger UI page.

![Swagger UI - OpenID Connect Auth][image-16]

Unlike other auth flows, this OpenID Connect auth flow shows two methods. The first one is the authentication code flow, and the other one is the implicit flow. Let's use the second one and enter the client ID value. It will redirect you to Azure Active Directory to sign in and give you the access token.

![Swagger UI - OpenID Connect Auth - Details][image-17]

Once execute the endpoint, the access token is passed through the `Authorization` header in the JWT format.

![Swagger UI - OpenID Connect Auth - Result][image-18]

Decode and validate the token for further processing.


## OAuth2 Easy Auth Flow ##

This approach is only applicable when your Azure Functions app has already been deployed to Azure. You don't need to do anything in the code because Azure Functions app instance does everything for you. All you need to do in your code is to check the claims. For more details about Easy Auth, visit this page, [Authentication and authorization in Azure App Service and Azure Functions][az funcapp easy auth].

```csharp
public static class OAuthEasyAuthFlowHttpTrigger
{
    [FunctionName(nameof(OAuthEasyAuthFlowHttpTrigger))]
    [OpenApiOperation(operationId: "oauth.flows.easyauth", tags: new[] { "oauth" }, Summary = "OAuth easy auth flows - MUST be deployed to Azure", Description = "This shows the OAuth easy auth flows. To use this feature, this function app MUST be deployed to Azure.", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<string>), Summary = "successful operation", Description = "successful operation")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        // ⬇️⬇️⬇️ Check your claims ⬇️⬇️⬇️
        var claims = req.HttpContext.User.Claims.Select(p => p.ToString());
        // ⬆️⬆️⬆️ Check your claims ⬆️⬆️⬆️

        var result = new OkObjectResult(claims);

        return await Task.FromResult(result).ConfigureAwait(false);
    }
}
```


[image-01]: images/securing-azure-function-endpoints-via-openapi-auth-01.png
[image-02]: images/securing-azure-function-endpoints-via-openapi-auth-02.png
[image-03]: images/securing-azure-function-endpoints-via-openapi-auth-03.png
[image-04]: images/securing-azure-function-endpoints-via-openapi-auth-04.png
[image-05]: images/securing-azure-function-endpoints-via-openapi-auth-05.png
[image-06]: images/securing-azure-function-endpoints-via-openapi-auth-06.png
[image-07]: images/securing-azure-function-endpoints-via-openapi-auth-07.png
[image-08]: images/securing-azure-function-endpoints-via-openapi-auth-08.png
[image-09]: images/securing-azure-function-endpoints-via-openapi-auth-09.png
[image-10]: images/securing-azure-function-endpoints-via-openapi-auth-10.png
[image-11]: images/securing-azure-function-endpoints-via-openapi-auth-11.png
[image-12]: images/securing-azure-function-endpoints-via-openapi-auth-12.png
[image-13]: images/securing-azure-function-endpoints-via-openapi-auth-13.png
[image-14]: images/securing-azure-function-endpoints-via-openapi-auth-14.png
[image-15]: images/securing-azure-function-endpoints-via-openapi-auth-15.png
[image-16]: images/securing-azure-function-endpoints-via-openapi-auth-16.png
[image-17]: images/securing-azure-function-endpoints-via-openapi-auth-17.png
[image-18]: images/securing-azure-function-endpoints-via-openapi-auth-18.png


[openapi spec security]: https://github.com/OAI/OpenAPI-Specification/blob/main/versions/3.0.1.md#securitySchemeObject

[az funcapp security baseline]: https://learn.microsoft.com/security/benchmark/azure/baselines/functions-security-baseline
[az funcapp easy auth]: https://learn.microsoft.com/azure/app-service/overview-authentication-authorization

[az ad]: https://learn.microsoft.com/azure/active-directory/fundamentals/active-directory-whatis
[az ad tenancy]: https://learn.microsoft.com/azure/active-directory/develop/single-and-multi-tenant-apps
[az ad oauth2 implicit flow]: https://learn.microsoft.com/azure/active-directory/develop/v2-oauth2-implicit-grant-flow
[az ad oidc flow]: https://learn.microsoft.com/azure/active-directory/develop/v2-protocols-oidc
