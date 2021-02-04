# Microsoft.Azure.WebJobs.Extensions.OpenApi.Core #

![Build and Test](https://github.com/Azure/azure-functions-openapi-extension/workflows/Build%20and%20Test/badge.svg) [![](https://img.shields.io/nuget/dt/Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.svg)](https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.OpenApi.Core/) [![](https://img.shields.io/nuget/v/Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.svg)](https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.OpenApi.Core/)

This enables Azure Functions to render Open API document and Swagger UI. The more details around the Swagger UI on Azure Functions can be found on this [blog post](https://devkimchi.com/2019/02/02/introducing-swagger-ui-on-azure-functions/).

> **NOTE**: This extension supports both [Open API 2.0 (aka Swagger)](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/2.0.md) and [Open API 3.0.1](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md) spec.


## Acknowledgement ##

* [Swagger UI](https://github.com/swagger-api/swagger-ui) version used for this library is [3.20.5](https://github.com/swagger-api/swagger-ui/releases/tag/v3.20.5) under the [Apache 2.0 license](https://opensource.org/licenses/Apache-2.0).


## Issues? ##

While using this library, if you find any issue, please raise a ticket on the [Issue](https://github.com/Azure/azure-functions-openapi-extension/issues) page.


## Getting Started ##

### Install NuGet Package ###

In order for your Azure Functions app to enable Open API capability, download the following NuGet package into your Azure Functions project.

```bash
dotnet add <PROJECT> package Microsoft.Azure.WebJobs.Extensions.OpenApi.OpenApi.Core
```


### Expose Endpoints to Open API Document ###

In order to include HTTP endpoints into the Open API document, use attribute classes (decorators) like:

```csharp
[FunctionName(nameof(AddDummy))]
[OpenApiOperation("addDummy", "dummy")]
[OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
[OpenApiRequestBody("application/json", typeof(DummyRequestModel))]
[OpenApiResponseBody(HttpStatusCode.OK, "application/json", typeof(DummyResponseModel))]
public static async Task<IActionResult> AddDummy(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "dummies")] HttpRequest req,
    ILogger log)
{
    ...
}
```


### Configure App Settings Key ###

This key is only required if:

* The Function app is deployed to Azure, and
* The Open API related endpoints has the `AuthorizationLevel` value other than `Anonymous`.

If the above conditions are met, add the following key to your `local.settings.json` or App Settings blade on Azure.

* `OpenApi__ApiKey`: either the host key value or the master key value.

> **NOTE**: It is NOT required if your Open API related endpoints are set to the authorisation level of `Anonymous`.


## Open API Metadata Configuration ##

To generate an Open API document, [OpenApiInfo object](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md#infoObject) needs to be defined. This information can be declared in **ONE OF THREE** places &ndash; `host.json`, `openapisettings.json` or `local.settings.json`.

This library looks for the information in the following order:

1. `host.json`
2. `openapisettings.json`
3. `local.settings.json` or App Settings blade on Azure


### `host.json` ###

Although it has not been officially accepted to be a part of `host.json`, the Open API metadata still can be stored in it like:

```json
{
  ...
  "openApi": {
    "info": {
      "version": "1.0.0",
      "title": "Open API Sample on Azure Functions",
      "description": "A sample API that runs on Azure Functions 3.x using Open API specification - from **host. json**.",
      "termsOfService": "https://github.com/Azure/azure-functions-openapi-extension",
      "contact": {
        "name": "Azure Functions Open API",
        "email": "azfunc-openapi@microsoft.com",
        "url": "https://github.com/Azure/azure-functions-openapi-extension/issues"
      },
      "license": {
        "name": "MIT",
        "url": "http://opensource.org/licenses/MIT"
      }
    }
  }
  ...
}
```


### `openapisettings.json` ###

The Open API metadata can be defined in a separate file, `openapisettings.json` like:

```json
{
  "info": {
    "version": "1.0.0",
    "title": "Open API Sample on Azure Functions",
    "description": "A sample API that runs on Azure Functions 3.x using Open API specification - from  **openapisettings.json**.",
    "termsOfService": "https://github.com/Azure/azure-functions-openapi-extension",
    "contact": {
      "name": "Azure Functions Open API",
      "email": "azfunc-openapi@microsoft.com",
      "url": "https://github.com/Azure/azure-functions-openapi-extension/issues"
    },
    "license": {
      "name": "MIT",
      "url": "http://opensource.org/licenses/MIT"
    }
  }
}
```


### `local.settings.json` or App Settings Blade ###

On either your `local.settings.json` or App Settings on Azure Functions instance, those details can be set up like:

* `OpenApi__Info__Version`: **REQUIRED** Version of Open API document. This is not the version of Open API spec. eg. 1.0.0
* `OpenApi__Info__Title`: **REQUIRED** Title of Open API document. eg. Open API Sample on Azure Functions
* `OpenApi__Info__Description`: Description of Open API document. eg. A sample API that runs on Azure Functions either 1.x or 2.x using Open API specification.
* `OpenApi__Info__TermsOfService`: Terms of service URL. eg. https://github.com/aliencube/AzureFunctions.Extensions
* `OpenApi__Info__Contact__Name`: Name of contact. eg. Aliencube Community
* `OpenApi__Info__Contact__Email`: Email address for the contact. eg. no-reply@aliencube.org
* `OpenApi__Info__Contact__Url`: Contact URL. eg. https://github.com/aliencube/AzureFunctions.Extensions/issues
* `OpenApi__Info__License__Name`: **REQUIRED** License name. eg. MIT
* `OpenApi__Info__License__Url`: License URL. eg. http://opensource.org/licenses/MIT


## Decorators ##

In order to render Open API document, this uses attribute classes (decorators).

> **NOTE**: Not all Open API specs have been implemented.


### `OpenApiIgnoreAttribute` ###

If there is any HTTP trigger that you want to exclude from the Open API document, use this decorator. Typically this is used for the endpoints that render Open API document and Swagger UI.

```csharp
[FunctionName(nameof(RenderSwaggerDocument))]
[OpenApiIgnore] // This HTTP endpoint is excluded from the Open API document.
public static async Task<IActionResult> RenderSwaggerDocument(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "swagger.{extension}")] HttpRequest req,
    string extension,
    ILogger log)
{
    ...
}
```


### `OpenApiOperationAttribute` ###

This decorator implements a part of [Operation object](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md#operationObject) spec.

```csharp
[FunctionName(nameof(GetSample))]
[OpenApiOperation(operationId: "list", tags: new[] { "sample" })]
...
public static async Task<IActionResult> GetSample(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "samples")] HttpRequest req,
    ILogger log)
{
    ...
}
```

* `OperationId`: is the ID of the operation. If this is omitted, a combination of function name and verb is considered as the operation ID. eg) `Get_GetSample`
* `Tags`: are the list of tags of operation.
* `Summary`: is the summary of the operation.
* `Description`: is the description of the operation.
* `Visibility`: indicates how the operation is visible in Azure Logic Apps &ndash; `important`, `advanced` or `internal`. Default value is `undefined`.


### `OpenApiParameterAttribute` ###

This decorator implements the [Parameter object](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md#parameterObject) spec.

```csharp
[FunctionName(nameof(GetSample))]
[OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string))]
...
public static async Task<IActionResult> GetSample(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "samples")] HttpRequest req,
    ILogger log)
{
    ...
}
```

* `Name`: is the name of the parameter.
* `Summary`: is the summary of the parameter.
* `Description`: is the description of the parameter.
* `Type`: defines the parameter type. Default value is `typeof(string)`.
* `In`: identifies where the parameter is located &ndash; `header`, `path`, `query` or `cookie`. Default value is `path`.
* `CollectionDelimiter`: identifies the delimiter when a query parameter accepts multiple values &ndash; `comma`, `space` or `pipe`. Default value is `comma`.
* `Explode`: indicates whether a query parameter is used multiple times (eg. `foo=bar1&foo=bar2&foo=bar3`) or not (eg. `foo=bar1,bar2,bar3`). Default value is `false`.
* `Required`: indicates whether the parameter is required or not. Default value is `false`.
* `Visibility`: indicates how the parameter is visible in Azure Logic Apps &ndash; `important`, `advanced` or `internal`. Default value is `undefined`.


### `OpenApiSecurityAttribute` ###

This decorator implements both [Security Requirement Object](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md#securityRequirementObject) and [Security Scheme Object](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md#securitySchemeObject) spec.

```csharp
// API Key Auth
public static class DummyHttpTrigger
{
    [FunctionName(nameof(DummyHttpTrigger.Update))]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
    public static async Task<IActionResult> Update(
        [HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "dummies")] HttpRequest req,
        ILogger log)
    {
        ...
    }
}

// Basic HTTP Auth
public static class DummyHttpTrigger
{
    [FunctionName(nameof(DummyHttpTrigger.Update))]
    [OpenApiSecurity("basic_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic)]
    public static async Task<IActionResult> Update(
        [HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "dummies")] HttpRequest req,
        ILogger log)
    {
        ...
    }
}

// OAuth Auth
public class PetStoreAuth : OpenApiOAuthSecurityFlows
{
    public PetStoreAuth()
    {
        this.Implicit = new OpenApiOAuthFlow()
        {
            AuthorizationUrl = new Uri("http://petstore.swagger.io/oauth/dialog"),
            Scopes = { { "write:pets", "modify pets in your account" }, { "read:pets", "read your pets" } }
        };
    }
}

public static class DummyHttpTrigger
{
    [FunctionName(nameof(DummyHttpTrigger.Update))]
    [OpenApiSecurity("petstore_auth", SecuritySchemeType.OAuth2, Flows = typeof(PetStoreAuth))]
    public static async Task<IActionResult> Update(
        [HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "dummies")] HttpRequest req,
        ILogger log)
    {
        ...
    }
}
```

* `SchemeName`: defines the name of the security scheme.
* `SchemeType`: is the type of the security scheme. Valid values are `SecuritySchemeType.ApiKey`, `SecuritySchemeType.Http`, `SecuritySchemeType.OAuth2`, and `SecuritySchemeType.OpenIdConnect`.
* `Description`: is a short description for security scheme.
* `Name`: is the name of the header, query or cookie parameter to be used. This **MUST** be provided when the `SchemeType` is set to `SecuritySchemeType.ApiKey`.
* `In`: is the location of the API key. This **MUST** be provided when the `SchemeType` is set to `SecuritySchemeType.ApiKey`. Valid values are `OpenApiSecurityLocationType.Query`, `OpenApiSecurityLocationType.Header`, and `OpenApiSecurityLocationType.Cookie`.
* `Scheme`: is the name of the authorisation scheme. This **MUST** be provided when the `SchemeType` is set to `SecuritySchemeType.Http`. Valid values are `OpenApiSecuritySchemeType.Basic` and `OpenApiSecuritySchemeType.Bearer`.
* `BearerFormat`: is the hint to the client to identify how the bearer token is formatted. This **MUST** be provided when the `SchemeType` is set to `SecuritySchemeType.Http`.
* `Flows`: defines the configuration information for the flow types supported. This **MUST** be the type inheriting `OpenApiOAuthSecurityFlows`. This **MUST** be provided when the `SchemeType` is set to `SecuritySchemeType.OAuth2`.
* `OpenIdConnectUrl`: is the OpenId Connect URL to discover OAuth2 configuration values. This **MUST** be provided when the `SchemeType` is set to `SecuritySchemeType.OpenIdConnect`.
* `OpenIdConnectScopes`: is the comma delimited list of scopes of OpenId Connect. This **MUST** be provided when the `SchemeType` is set to `SecuritySchemeType.OpenIdConnect`.


### `OpenApiRequestBodyAttribute` ###

This decorator implements the [Request Body object](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md#requestBodyObject) spec.

```csharp
[FunctionName(nameof(PostSample))]
[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(SampleRequestModel))]
...
public static async Task<IActionResult> PostSample(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "samples")] HttpRequest req,
    ILogger log)
{
    ...
}
```

* `ContentType`: defines the content type of the request body payload. eg) `application/json` or `text/xml`
* `BodyType`: defines the type of the request payload.
* `Description`: is the description of the request payload.
* `Required`: indicates whether the request payload is mandatory or not.


### `OpenApiResponseWithBodyAttribute` ###

This decorator implements the [Response object](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md#responseObject) spec.

```csharp
[FunctionName(nameof(PostSample))]
[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SampleResponseModel))]
...
public static async Task<IActionResult> PostSample(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "samples")] HttpRequest req,
    ILogger log)
{
    ...
}
```

* `StatusCode`: defines the HTTP status code. eg) `HttpStatusCode.OK`
* `ContentType`: defines the content type of the response payload. eg) `application/json` or `text/xml`
* `BodyType`: defines the type of the response payload.
* `Summary`: is the summary of the response.
* `Description`: is the description of the response.


### `OpenApiResponseWithoutBodyAttribute` ###

This decorator implements the [Response object](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md#responseObject) spec.

```csharp
[FunctionName(nameof(PostSample))]
[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK)]
...
public static async Task<IActionResult> PostSample(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "samples")] HttpRequest req,
    ILogger log)
{
    ...
}
```

* `StatusCode`: defines the HTTP status code. eg) `HttpStatusCode.OK`
* `Summary`: is the summary of the response.
* `Description`: is the description of the response.


### `OpenApiSchemaVisibilityAttribute` ###

This decorator is a part of extended property for custom connectors of Azure Logic Apps and Power Platform.

```csharp
public class MyResponse
{
    [OpenApiSchemaVisibility(OpenApiVisibilityType.Advanced)]
    public string MyProperty { get; set; }
}

// This will result in:
// {
//   "myResponse": {
//     "myProperty": {
//       "type": "string",
//       "x-ms-visibility": "advanced"
//     }
//   }
// }
```


### `OpenApiPropertyDescriptionAttribute` ###

This decorator provides model properties with description.

```csharp
public class MyModel
{
    [OpenApiPropertyDescription("The number value")]
    public int Number { get; set; }

    [OpenApiPropertyDescription("The text value")]
    public string Text { get; set; }
}

// This will result in:
// {
//   "myModel": {
//     "number": {
//       "type": "integer",
//       "format": "int32",
//       "description": "The number value"
//     },
//     "text": {
//       "type": "string",
//       "description": "The text value"
//     }
//   }
// }
```


### `DisplayAttribute` ###

Use this decorator, if you want to display string values on your enum decorated with `Newtonsoft.Json.Converters.StringEnumConverter`.

> You can also use the `System.Runtime.Serialization.EnumMemberAttribute` decorator together. Make sure that this decorator takes precedence to the `DisplayAttribute` decorator.

```csharp
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Contoso
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StringEnum
    {
        Zero = 0,

        [Display("hana")]
        One = 1,

        [EnumMember(Value = "dul")]
        Two = 2,

        [Display("set")]
        [EnumMember(Value = "sam")]
        Three = 3,
    }
}

// This will result in
// "stringEnum": {
//   "type": "string",
//   "enum": [
//     "zero",
//     "hana",
//     "dul",
//     "sam"
//   ],
//   "default": "zero"
// }
```


## Supported Json.NET Decorators ##

Those attribute classes from [Json.NET](https://www.newtonsoft.com/json) are supported for payload definitions.


### `JsonIgnore` ###

Properties decorated with the `JsonIgnore` attribute class will not be included in the response.


### `JsonProperty` ###

Properties decorated with `JsonProperty` attribute class will use `JsonProperty.Name` value instead of their property names. In addition to this, if `JsonProperty.Required` property has `Required.Always` or `Required.DisallowNull`, the property will be recognised as the `required` field.


### `JsonRequired` ###

Properties decorated with `JsonRequired` attribute class will be recognised as the `required` field.


### `JsonConverter` ###

Enums types decorated with `[JsonConverter(typeof(StringEnumConverter))]` will appear in the document with their string names (names mangled based on default property naming standard).
