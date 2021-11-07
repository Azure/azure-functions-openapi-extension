# Microsoft.Azure.WebJobs.Extensions.OpenApi.Core #

![Build and Test](https://github.com/Azure/azure-functions-openapi-extension/workflows/Build%20and%20Test/badge.svg) [![](https://img.shields.io/nuget/dt/Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.svg)](https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.OpenApi.Core/) [![](https://img.shields.io/nuget/v/Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.svg)](https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.OpenApi.Core/)

This enables Azure Functions to render OpenAPI document and Swagger UI. The more details around the Swagger UI on Azure Functions can be found on this [blog post](https://devkimchi.com/2019/02/02/introducing-swagger-ui-on-azure-functions/).

> **NOTE**: This extension supports both [OpenAPI 2.0 (aka Swagger)](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/2.0.md) and [OpenAPI 3.0.1](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md) spec.


## Acknowledgement ##

* [Swagger UI](https://github.com/swagger-api/swagger-ui) version used for this library is [v3.44.0](https://github.com/swagger-api/swagger-ui/releases/tag/v3.44.0) under the [Apache 2.0 license](https://opensource.org/licenses/Apache-2.0).


## Issues? ##

While using this library, if you find any issue, please raise a ticket on the [Issue](https://github.com/Azure/azure-functions-openapi-extension/issues) page.


## Getting Started ##

### Install NuGet Package ###

In order for your Azure Functions app to enable OpenAPI capability, download the following NuGet package into your Azure Functions project.

```bash
dotnet add <PROJECT> package Microsoft.Azure.WebJobs.Extensions.OpenApi.OpenApi.Core
```


### Expose Endpoints to OpenAPI Document ###

In order to include HTTP endpoints into the OpenAPI document, use attribute classes (decorators) like:

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
* The OpenAPI related endpoints has the `AuthorizationLevel` value other than `Anonymous`.

If the above conditions are met, add the following key to your `local.settings.json` or App Settings blade on Azure.

* `OpenApi__ApiKey`: either the host key value or the master key value.

> **NOTE**: It is NOT required if your OpenAPI related endpoints are set to the authorisation level of `Anonymous`.


## OpenAPI Metadata Configuration ##

To generate an OpenAPI document, [OpenApiInfo object](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md#infoObject) needs to be defined. ***It's totally optional***, but if you want, you can implement the `IOpenApiConfigurationOptions` interface within your Azure Functions project to provide OpenAPI metadata like below:

```csharp
public class OpenApiConfigurationOptions : IOpenApiConfigurationOptions
{
    public OpenApiInfo Info { get; set; } = new OpenApiInfo()
    {
        Version = "1.0.0",
        Title = "OpenAPI Document on Azure Functions",
        Description = "HTTP APIs that run on Azure Functions using OpenAPI specification.",
        TermsOfService = new Uri("https://github.com/Azure/azure-functions-openapi-extension"),
        Contact = new OpenApiContact()
        {
            Name = "Contoso",
            Email = "azfunc-openapi@contoso.com",
            Url = new Uri("https://github.com/Azure/azure-functions-openapi-extension/issues"),
        },
        License = new OpenApiLicense()
        {
            Name = "MIT",
            Url = new Uri("http://opensource.org/licenses/MIT"),
        }
    };

    public List<OpenApiServer> Servers { get; set; } = new List<OpenApiServer>();

    public OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V2;
}
```


### Overriding Base URLs ###

It's often required for the API app to have more than one base URL, with different hostname. To have [additional server URL information](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md#serverObject), declare the `OpenApi__HostNames` value with comma delimited base URLs. Then, it will automatically sets up your base URLs.

> Find the [Configuration](openapi.md#configuration) section for the full list of the app settings keys.

Alternatively, add `OpenApiServer` details to the `Servers` property like:

```csharp
public class OpenApiConfigurationOptions : IOpenApiConfigurationOptions
{
    ...

    public List<OpenApiServer> Servers { get; set; } = new List<OpenApiServer>()
    {
        new OpenApiServer() { Url = "https://contoso.com/api/" },
        new OpenApiServer() { Url = "https://fabrikam.com/api/" },
    };

    ...
}
```

> **NOTE**:
> 
> * If no base URL is declared, the Azure Functions app's URL will be added as a default.
> * The OpenAPI v2 (Swagger) document only shows the the first server name on both UI and document, while the OpenAPI v3 document shows the first server name on the UI and all server names on the document.


### Overriding OpenAPI Version ###

The default version of OpenAPI document rendered is V2 (AKA Swagger). However, you can override the default rendering behaviour by implementing the `OpenApiVersion` property.

```csharp
public class OpenApiConfigurationOptions : IOpenApiConfigurationOptions
{
    ...

    public OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V3;

    ...
}
```


### Force HTTP or HTTPS for Swagger UI ###

> **NOTE**: If you use the [Linux Dedicated Plan](https://docs.microsoft.com/azure/azure-functions/dedicated-plan?WT.mc_id=github-0000-juyoo), you can consider this HTTP/HTTPS enforcement settings.

If you want to force either HTTP or HTTPS, configure the following properties on the `IOpenApiConfigurationOptions` interface.

```csharp
public class OpenApiConfigurationOptions : IOpenApiConfigurationOptions
{
    ...

    public bool ForceHttps { get; set; } = true;
    public bool ForceHttp { get; set; } = true;

    ...
}
```

You can set either property to `true`, and based on the combination of both, your Swagger UI renders contents through either HTTP or HTTPS. However, if you set both properties to `true`, HTTPS takes precedence.


### Inheriting `DefaultOpenApiConfigurationOptions` ###

Instead of implementing `IOpenApiConfigurationOptions`, you can inherit `DefaultOpenApiConfigurationOptions`. As `Info`, `Servers` and `OpenApiVersion` properties have the modifier of `virtual`, you can freely override them or leave them as default.

```csharp
public class MyOpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
{
    public override OpenApiInfo Info { get; set; } = new OpenApiInfo()
    {
        Version = "1.0.0",
        Title = "OpenAPI Document on Azure Functions",
        Description = "HTTP APIs that run on Azure Functions using OpenAPI specification.",
        TermsOfService = new Uri("https://github.com/Azure/azure-functions-openapi-extension"),
        Contact = new OpenApiContact()
        {
            Name = "Contoso",
            Email = "azfunc-openapi@contoso.com",
            Url = new Uri("https://github.com/Azure/azure-functions-openapi-extension/issues"),
        },
        License = new OpenApiLicense()
        {
            Name = "MIT",
            Url = new Uri("http://opensource.org/licenses/MIT"),
        }
    };

    public override List<OpenApiServer> Servers { get; set; } = new List<OpenApiServer>()
    {
        new OpenApiServer() { Url = "https://contoso.com/api/" },
        new OpenApiServer() { Url = "https://fabrikam.com/api/" },
    };

    public override OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V3;

    // Consider Linux Dedicated Plan only.
    public override bool ForceHttps { get; set; } = true;
}
```

## Swagger UI Customisation ##

Suppose you want to customise the look and feels of the Swagger UI page. In this case, there are two options you can choose.


### Use CSS and JavaScript Files Embedded ###

1. Add `custom.css` and `custom.js` under the `dist` directory of your Functions app project. Then, update your `.csproj` file to embed both files like:

    ```xml
    <ItemGroup>
      <EmbeddedResource Include="dist\custom.css" />
      <EmbeddedResource Include="dist\custom.js" />
    </ItemGroup>
    ```

1. You can inherit `DefaultOpenApiCustomUIOptions` to put additional control from your end such as changing the custom CSS and JavaScript file names or change behaviours of handing CSS and JavaScript.

    ```csharp
    public class OpenApiCustomUIOptions : DefaultOpenApiCustomUIOptions
    {
        public OpenApiCustomUIOptions(Assembly assembly)
            : base(assembly)
        {
        }

        // Declare if you want to change the custom CSS file name.
        public override string CustomStylesheetPath { get; } = "dist.my-custom.css";

        // Declare if you want to change the custom JavaScript file name.
        public override string CustomJavaScriptPath { get; } = "dist.my-custom.js";

        // Declare if you want to change the behaviours of handling the custom CSS file.
        public override async Task<string> GetStylesheetAsync()
        {
            // DO SOMETHING BEFORE CALLING THE BASE METHOD

            base.GetStylesheetAsync();

            // DO SOMETHING AFTER CALLING THE BASE METHOD
        }

        // Declare if you want to change the behaviours of handling the custom JavaScript file.
        public override async Task<string> GetJavaScriptAsync()
        {
            // DO SOMETHING BEFORE CALLING THE BASE METHOD

            base.GetJavaScriptAsync();

            // DO SOMETHING AFTER CALLING THE BASE METHOD
        }
    }
    ```


### Use CSS and JavaScript Files from CDN ###

Alternatively, you can use both CSS and JavaScript files from CDN, which is from the Internet.

```csharp
public class OpenApiCustomUIOptions : DefaultOpenApiCustomUIOptions
{
    public OpenApiCustomUIOptions(Assembly assembly)
        : base(assembly)
    {
    }
    // Declare if you want to change the custom CSS file name.
    public override string CustomStylesheetPath { get; }
        = "https://raw.githubusercontent.com/Azure/azure-functions-openapi-extension/main/samples/Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3Static/dist/my-custom.css";
    // Declare if you want to change the custom JavaScript file name.
    public override string CustomJavaScriptPath { get; }
        = "https://raw.githubusercontent.com/Azure/azure-functions-openapi-extension/main/samples/Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3Static/dist/my-custom.js";
    // Declare if you want to change the behaviours of handling the custom CSS file.
    public override async Task<string> GetStylesheetAsync()
    {
        // DO SOMETHING BEFORE CALLING THE BASE METHOD
        base.GetStylesheetAsync();
        // DO SOMETHING AFTER CALLING THE BASE METHOD
    }
    // Declare if you want to change the behaviours of handling the custom JavaScript file.
    public override async Task<string> GetJavaScriptAsync()
    {
        // DO SOMETHING BEFORE CALLING THE BASE METHOD
        base.GetJavaScriptAsync();
        // DO SOMETHING AFTER CALLING THE BASE METHOD
    }
}
```

Either way, your customised CSS and JavaScript will be applied to the Swagger UI page.


## OpenAPI Response Header Customisation ##

Often, custom response headers need to be added. You can use `IOpenApiCustomResponseHeader` to add the custom response headers.

```csharp
// Custom response header type
public class CustomResponseHeader : IOpenApiCustomResponseHeader
{
    public Dictionary<string, OpenApiHeader> Headers { get; set; } =
        new Dictionary<string, OpenApiHeader>()
        {
            {
                "x-custom-header",
                new OpenApiHeader()
                {
                    Description = "Custom response header",
                    Schema = new OpenApiSchema() { Type = "string" }
                }
            }
        };
}

public static class CustomResponseHeaderHttpTrigger
{
    [FunctionName(nameof(CustomResponseHeaderHttpTrigger))]
    ...
    [OpenApiResponseWithBody(... HeaderType = typeof(CustomResponseHeader))]
    [OpenApiResponseWithoutBody(... HeaderType = typeof(CustomResponseHeader))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "GET", Route = null)] HttpRequest req,
        ILogger log)
    {
        ...
    }
}
```


## Decorators ##

In order to render OpenAPI document, this uses attribute classes (decorators).

> **NOTE**: Not all OpenAPI specs have been implemented.


### `OpenApiIgnoreAttribute` ###

If there is any HTTP trigger that you want to exclude from the OpenAPI document, use this decorator. Typically this is used for the endpoints that render OpenAPI document and Swagger UI.

```csharp
[FunctionName(nameof(RenderSwaggerDocument))]
[OpenApiIgnore] // This HTTP endpoint is excluded from the OpenAPI document.
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
* `Deprecated`: indicates whether the operation is deprecated or not. Default is `false`.


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
* `Deprecated`: indicates whether the parameter is deprecated or not. Default is `false`. If this is set to `true`, this parameter won't be showing up the UI and OpenAPI document.


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
// Example
public class SampleRequestModelExample : OpenApiExample<SampleRequestModel>
{
    public override IOpenApiExample<SampleRequestModel> Build(NamingStrategy namingStrategy = null)
    {
        this.Examples.Add(
            OpenApiExampleResolver.Resolve(
                "sample1",
                new SampleRequestModel() { Title = "Hello World", Value = 1234 },
                namingStrategy
            ));

        return this;
    }
}

[FunctionName(nameof(PostSample))]
[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(SampleRequestModel), Example = typeof(SampleRequestModelExample))]
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
* `Deprecated`: indicates whether the request body is deprecated or not. Default is `false`. If this is set to `true`, this request body won't be showing up the UI and OpenAPI document.
* `Example`: defines the type of the request payload example. It SHOULD inherit `OpenApiExample<T>` or implement `IOpenApiExample<T>`.


### `OpenApiResponseWithBodyAttribute` ###

This decorator implements the [Response object](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md#responseObject) spec.

```csharp
// Example
public class SampleResponseModelExample : OpenApiExample<SampleResponseModel>
{
    public override IOpenApiExample<SampleResponseModel> Build(NamingStrategy namingStrategy = null)
    {
        this.Examples.Add(
            OpenApiExampleResolver.Resolve(
                "sample1",
                new SampleResponseModel() { Title = "Hello World", Value = 1234 },
                namingStrategy
            ));

        return this;
    }
}

// Response header type
public class SampleResponseHeaderType : IOpenApiResponseHeaderType
{
    public Dictionary<string, OpenApiHeader> Headers { get; set; } = new Dictionary<string, OpenApiHeader>()
    {
        { "x-sample-header", new OpenApiHeader() { Description = "Sample header", Schema = new OpenApiSchema() { Type = "string" } } }
    };
}

public static class DummyHttpTrigger
{
    [FunctionName(nameof(PostSample))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SampleResponseModel), Example = typeof(SampleResponseModelExample), HeaderType = typeof(SampleResponseHeaderType))]
    ...
    public static async Task<IActionResult> PostSample(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "samples")] HttpRequest req,
        ILogger log)
    {
        ...
    }
}
```

* `StatusCode`: defines the HTTP status code. eg) `HttpStatusCode.OK`
* `HeaderType`: defines the collection of custom response headers. eg) `x-custom-header`
* `ContentType`: defines the content type of the response payload. eg) `application/json` or `text/xml`
* `BodyType`: defines the type of the response payload.
* `Summary`: is the summary of the response.
* `Description`: is the description of the response.
* `Deprecated`: indicates whether the response body is deprecated or not. Default is `false`. If this is set to `true`, this response body won't be showing up the UI and OpenAPI document.
* `Example`: defines the type of the response payload example. It SHOULD inherit `OpenApiExample<T>` or implement `IOpenApiExample<T>`.


### `OpenApiResponseWithoutBodyAttribute` ###

This decorator implements the [Response object](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md#responseObject) spec.

```csharp
// Response header type
public class SampleResponseHeaderType : IOpenApiResponseHeaderType
{
    public Dictionary<string, OpenApiHeader> Headers { get; set; } = new Dictionary<string, OpenApiHeader>()
    {
        { "x-sample-header", new OpenApiHeader() { Description = "Sample header", Schema = new OpenApiSchema() { Type = "string" } } }
    };
}

public static class DummyHttpTrigger
{
    [FunctionName(nameof(PostSample))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, HeaderType = typeof(SampleResponseHeaderType))]
    ...
    public static async Task<IActionResult> PostSample(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "samples")] HttpRequest req,
        ILogger log)
    {
        ...
    }
}
```

* `StatusCode`: defines the HTTP status code. eg) `HttpStatusCode.OK`
* `HeaderType`: defines the collection of custom response headers. eg) `x-custom-header`
* `Summary`: is the summary of the response.
* `Description`: is the description of the response.


### `OpenApiExampleAttribute` ###

This decorator implements the example attribute defined in the [`Schema object`](https://github.com/OAI/OpenAPI-Specification/blob/main/versions/3.0.1.md#schemaObject) section.

```csharp
// Example
public class CatExample : OpenApiExample<Cat>
{
    public override IOpenApiExample<Cat> Build(NamingStrategy namingStrategy = null)
    {
        this.Examples.Add(OpenApiExampleResolver.Resolve("nabi", new Cat() { Id = 123, Name = "Nabi" }, namingStrategy));

        return this;
    }
}

// Model
[OpenApiExample(typeof(CatExample))]
public class Cat
{
    public int Id { get; set; }

    public string Name { get; set; }
}

// This will result in:
// {
//   "components": {
//     "schemas": {
//       "cat": {
//         "type": "object",
//         "properties": {
//           "id": {
//             "type": "integer"
//             "format": "int32"
//           },
//           "name": {
//             "type": "string"
//           }
//         },
//         "example": "{\"id\":123,\"name\":\"Nabi\"}"
//       }
//     }
//   }
// }
```


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


### `OpenApiPropertyAttribute` ###

This decorator provides model properties with description.

```csharp
public class MyModel
{
    [OpenApiProperty(Nullable = true, Default = 10, Description = "The number value")]
    public int Number { get; set; }

    [OpenApiProperty(Nullable = true, Default = "Hello World", Description = "The text value")]
    public string Text { get; set; }
}

// This will result in:
// {
//   "myModel": {
//     "number": {
//       "type": "integer",
//       "format": "int32",
//       "nullable": true,
//       "description": "The number value",
//       "default": 10
//     },
//     "text": {
//       "type": "string",
//       "nullable": true,
//       "description": "The text value",
//       "default": "Hello World"
//     }
//   }
// }
```

> [!IMPORTANT]
> If you use C# 8.0 or later for your Azure Functions app development, you MUST use this decorator to describe the `string` data type to be nullable or not.

* `Nullable`: defines a value indicating whether the property is nullable or not. This value takes precedence regardless the property itself is nullable value type or not.
* `Default`: defines the default value of the property.
* `Description`: defines the description of the property.


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


## Supported System.ComponentModel.DataAnnotations Decorators ##

Some of [ValidationAttribute](https://docs.microsoft.com/dotnet/api/system.componentmodel.dataannotations.validationattribute) classes from [System.ComponentModel.DataAnnotations](https://www.nuget.org/packages/System.ComponentModel.Annotations/4.4.0) are supported for payload definition.


### `DataTypeAttribute` ###

Properties decorated with the `DataTypeAttribute` class impacts on the `OpenApiSchema.Format` value.

* `DataType.DateTime`: `date-time`
* `DataType.Date`: `date`
* `DataType.Time`: `time`
* `DataType.Duration`: `duration`
* `DataType.PhoneNumber`: `tel`
* `DataType.Currency`: `currency`
* `DataType.Text`: `string`
* `DataType.Html`: `html`
* `DataType.MultilineText`: `multiline`
* `DataType.EmailAddress`: `email`
* `DataType.Password`: `password`
* `DataType.Url`: `uri`
* `DataType.ImageUrl`: `uri`
* `DataType.CreditCard`: `credit-card`
* `DataType.PostalCode`: `postal-code`


### `RegularExpressionAttribute` ###

Properties decorated with the `RegularExpressionAttribute` class impacts on the `OpenApiSchema.Pattern` value.


### `StringLengthAttribute` ###

Properties decorated with the `StringLengthAttribute` class impacts on both `OpenApiSchema.MinLength`and `OpenApiSchema.MaxLength` values.


### `RangeAttribute` ###

Properties decorated with the `RangeAttribute` class impacts on both `OpenApiSchema.Minimum` and `OpenApiSchema.Maximum` value.


### `MinLengthAttribute` ###

Properties decorated with the `MinLengthAttribute` class impacts on either `OpenApiSchema.MinItems` or `OpenApiSchema.MinLength` value.

* If `OpenApiSchema.Type` is `array`: `OpenApiSchema.MinItems`
* If `OpenApiSchema.Type` is NOT `array`: `OpenApiSchema.MinLength`


### `MaxLengthAttribute` ###

Properties decorated with the `MaxLengthAttribute` class impacts on either `OpenApiSchema.MaxItems` or `OpenApiSchema.MaxLength` value.

* If `OpenApiSchema.Type` is `array`: `OpenApiSchema.MaxItems`
* If `OpenApiSchema.Type` is NOT `array`: `OpenApiSchema.MaxLength`


## Supported System.Runtime.Serialization Decorators ##

Some attribute classes from [System.Runtime.Serialization](https://docs.microsoft.com/dotnet/api/system.runtime.serialization) are supported for payload definition.


### `DataMemberAttribute` ###

Properties decorated with `DataMemberAttribute` attribute class will use `DataMember.Name` value instead of their property names.


## Supported Json.NET Decorators ##

Those attribute classes from [Json.NET](https://www.newtonsoft.com/json) are supported for payload definitions.


### `JsonIgnoreAttribute` ###

Properties decorated with the `JsonIgnoreAttribute` attribute class will not be included in the response.


### `JsonPropertyAttribute` ###

Properties decorated with `JsonPropertyAttribute` attribute class will use `JsonProperty.Name` value instead of their property names. In addition to this, if `JsonProperty.Required` property has `Required.Always` or `Required.DisallowNull`, the property will be recognised as the `required` field.


### `JsonRequiredAttribute` ###

Properties decorated with `JsonRequiredAttribute` attribute class will be recognised as the `required` field.


### `JsonConverterAttribute` ###

Enums types decorated with `[JsonConverter(typeof(StringEnumConverter))]` will appear in the document with their string names (names mangled based on default property naming standard).
