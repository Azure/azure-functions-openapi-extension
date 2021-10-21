# Advanced Configurations for OpenAPI Extension #

This page describes the advanced configurations for OpenAPI and Swagger UI in general. If you look for process-specific configurations, please find the following pages:

* [Microsoft.Azure.WebJobs.Extensions.OpenApi &ndash; In-Process Model](./openapi-in-proc.md)
* [Microsoft.Azure.Functions.Worker.Extensions.OpenApi &ndash; Out-of-Process Model](./openapi-out-of-proc.md)


## Configuration ##

For the extension's advanced configuration, it expects the following config keys.


### Configure Authorization Level ###

> **NOTE**: Currently, the out-of-process worker extension only supports the `AuthorizationLevel.Anonymous` access. The following configurations are only applicable to the in-process worker extension.

As a default, all endpoints to render Swagger UI and OpenAPI documents have the authorisation level of `AuthorizationLevel.Anonymous`. However, if you want to secure those endpoints, change their authorisation level to `AuthorizationLevel.Function` and pass the API Key through either request header or querystring parameter. This can be done through the environment variables. Here's the sample `local.settings.json` file. The other values are omitted for brevity.

```json
{
  "Values": {
    "OpenApi__ApiKey": "",
    "OpenApi__AuthLevel__Document": "Anonymous",
    "OpenApi__AuthLevel__UI": "Anonymous"
  }
}
```

You can have granular controls to both Swagger UI and OpenAPI documents by setting the authorisation level to `Anonymous`, `User`, `Function`, `System` or `Admin`. Make sure that you MUST provide the `OpenApi__AuthKey` value, if you choose the `OpenApi__AuthLevel__Document` value other than `Anonymous`. Otherwise, it will throw an error.

> **NOTE**: Both Swagger UI and OpenAPI document pages are allowed `Anonymous` access by default.


### Configure Swagger UI Visibility ###

> **NOTE**: Currently, the out-of-process worker model doesn't support hiding Swagger UI. The following configurations are only applicable to the in-process worker extension.

You may want to only enable the Swagger UI page during the development time, and disable the page when publishing it to Azure. You can configure an environment variable to enable/disable the Swagger UI page. Here's the sample `local.settings.json` file. The other values are omitted for brevity.

```json
{
  "Values": {
    "OpenApi__HideSwaggerUI": "false"
  }
}
```

If you set the `OpenApi__HideSwaggerUI` value to `true`, the Swagger UI page won't be showing up, and you will see the 404 error.

> **NOTE**: The default value for `OpenApi__HideSwaggerUI` is `false`.


### Configure OpenAPI Information ###

As a default, the OpenAPI document automatically generated provides a minimum set of information like:

* OpenAPI Version: `2.0`
* OpenAPI Document Title: `OpenAPI Document on Azure Functions`
* OpenAPI Document Version: `1.0.0`

You may want to provide consumers with more details by implementing the `IOpenApiConfigurationOptions` interface or inheriting the `DefaultOpenApiConfigurationOptions` class.  If you inherit the `DefaultOpenApiConfigurationOptions` class, you can use the following environment variables to avoid the app from being recompiled and redeployed. Here's the sample `local.settings.json` file. The other values are omitted for brevity.

```json
{
  "Values": {
    "OpenApi__Version": "v2",
    "OpenApi__DocTitle": "Azure Functions OpenAPI Extension",
    "OpenApi__DocVersion": "1.0.0"
  }
}
```


### Configure Custom Base URLs ###

There's a chance that you want to expose the UI and OpenAPI document through [Azure API Management](https://docs.microsoft.com/azure/api-management/api-management-key-concepts?WT.mc_id=github-0000-juyoo) or load balancing services like [Azure Front Door](https://docs.microsoft.com/azure/frontdoor/front-door-overview?WT.mc_id=github-0000-juyoo). If you inherit the `DefaultOpenApiConfigurationOptions` class, you can configure an environment variable to add them. Here's the sample `local.settings.json` file. The other values are omitted for brevity.

```json
{
  "Values": {
    "OpenApi__HostNames": "https://contoso.com/api/,https://fabrikam.com/api/"
  }
}
```

> **NOTE**: This multiple hostnames support feature only works with OpenAPI 3.x, not OpenAPI 2.x.


### Force HTTP or HTTPS for Swagger UI ###

There's a chance if you want to force the Swagger UI to render either HTTP or HTTPS. If you inherit the `DefaultOpenApiConfigurationOptions` class, you can configure environment variables to add them. Here's the sample `local.settings.json` file. The other values are omitted for brevity.

> **NOTE**: Both values are set to `false` by default.

```json
{
  "Values": {
    "OpenApi__ForceHttps": "true",
    "OpenApi__ForceHttp": "false",
  }
}
```

> **NOTE**: If your Azure Functions app is running on the [Linux Dedicated Plan](https://docs.microsoft.com/azure/azure-functions/dedicated-plan?WT.mc_id=github-0000-juyoo), consider this configuration.


## Swaggrer UI Endpoints Filtering ##

You may want to selectively display endpoints on the Swagger UI and OpenAPI documents. In this case, you can use the tags to filter which endpoints you want to show. For example, you've got endpoints like:

```csharp
[FunctionName("MyAdminFunction1")]
[OpenApiOperation(operationId: ..., tags: new[] { "admin" })]
...
public static async Task<IActionResult> MyAdminFunction1(...)
{
    ...
}

[FunctionName("MyAdminFunction2")]
[OpenApiOperation(operationId: ..., tags: new[] { "admin" })]
...
public static async Task<IActionResult> MyAdminFunction2(...)
{
    ...
}

[FunctionName("MyFunction1")]
[OpenApiOperation(operationId: ..., tags: new[] { "product" })]
...
public static async Task<IActionResult> MyFunction1(...)
{
    ...
}

[FunctionName("MyFunction2")]
[OpenApiOperation(operationId: ..., tags: new[] { "product" })]
...
public static async Task<IActionResult> MyFunction2(...)
{
    ...
}

[FunctionName("MyOptionFunction1")]
[OpenApiOperation(operationId: ..., tags: new[] { "option" })]
...
public static async Task<IActionResult> MyOptionFunction1(...)
{
    ...
}

```

If you only want to only show the admin API endpoints, add `tag=admin` to the querystring:

```
http://localhost:7071/api/swagger/ui?tag=admin
http://localhost:7071/api/swagger.json?tag=admin
```

The `tag` parameter accepts a commma separated list of tags. Any function with any tag passed on the `tag` parameter will be selected.
If you only want to show the API endpoints related to `product` or `option` tag, add `tag=product,option` to the querystring:

```
http://localhost:7071/api/swagger/ui?tag=product,option
http://localhost:7071/api/swagger.json?tag=product,option
```


## Further Authentication and Authorisation ##

This extension relies on the built-in authentication method, either `code=` in the querystring or `x-functions-key` in the request header. However, if you want additional authentication and authorisation for both Swagger UI and OpenAPI documents, you can implement the `IOpenApiHttpTriggerAuthorization` interface or inherit the `DefaultOpenApiHttpTriggerAuthorization` class.

Here's the `DefaultOpenApiHttpTriggerAuthorization` class implementing the `IOpenApiHttpTriggerAuthorization` interface.

```csharp
public class DefaultOpenApiHttpTriggerAuthorization : IOpenApiHttpTriggerAuthorization
{
    public virtual async Task<OpenApiAuthorizationResult> AuthorizeAsync(IHttpRequestDataObject req)
    {
        var result = default(OpenApiAuthorizationResult);

        return await Task.FromResult(result).ConfigureAwait(false);
    }
}
```

If you want your own implementation with OAuth2, you may like to do like:

```csharp
public class OpenApiHttpTriggerAuthorization : DefaultOpenApiHttpTriggerAuthorization
{
    public override async Task<OpenApiAuthorizationResult> AuthorizeAsync(IHttpRequestDataObject req)
    {
        var result = default(OpenApiAuthorizationResult);
        var authtoken = (string) req.Headers["Authorization"];

        // Redirect to the auth page, if no auth header is found.
        if (authtoken.IsNullOrWhiteSpace())
        {
            result = new OpenApiAuthorizationResult()
            {
                StatusCode = HttpStatusCode.Redirect,
                ContentType = "text/html",
                Payload = "<html><head><meta http-equiv=\"refresh\" content=\"0;url=https://login.microsoftonline.com/common/oauth2/v2.0/authorize\"/></head><body></body></html>",
            };

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        // Shows invalid auth format, if the auth header is not the bearer token.
        if (authtoken.StartsWith("Bearer", ignoreCase: true, CultureInfo.InvariantCulture) == false)
        {
            result = new OpenApiAuthorizationResult()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                ContentType = "text/plain",
                Payload = "Invalid auth format",
            };

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        // Shows invalid auth token, if token value is invalid.
        var token = authtoken.Split(' ').Last();
        if (token != "secret")
        {
            result = new OpenApiAuthorizationResult()
            {
                StatusCode = HttpStatusCode.Forbidden,
                ContentType = "text/plain",
                Payload = "Invalid auth token",
            };

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        // DO SOMETHING IF AUTH TOKEN IS VALID.
        ...

        // Return null if auth is valid.
        return await Task.FromResult(result).ConfigureAwait(false);
    }
}
```

Then, `OpenApiHttpTriggerContext` automatically picks it up and invokes its `AuthorizeAsync(...)` method. The following code shows how your auth results are handled within each Swagger UI and OpenAPI document endpoints.

```csharp
// In-Process Worker
var auth = await context.SetApplicationAssemblyAsync(ctx.FunctionAppDirectory)
                        .AuthorizeAsync(request)
                        .ConfigureAwait(false);
if (!auth.IsNullOrDefault())
{
    content = new ContentResult()
    {
        Content = auth.Payload,
        ContentType = auth.ContentType,
        StatusCode = (int)auth.StatusCode,
    };

    return content;
}

// Out-Of-Process Worker
var auth = await this._context
                        .SetApplicationAssemblyAsync(fi.Directory.FullName, appendBin: false)
                        .AuthorizeAsync(request)
                        .ConfigureAwait(false);
if (!auth.IsNullOrDefault())
{
    response = req.CreateResponse(auth.StatusCode);
    response.Headers.Add("Content-Type", auth.ContentType);
    await response.WriteStringAsync(auth.Payload).ConfigureAwait(false);

    return response;
}
```
