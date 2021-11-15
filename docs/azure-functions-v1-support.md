# Support Azure Functions v1 with OpenAPI Extension #

Due to the backward compatibility issue, this OpenAPI extension has dropped supporting Azure Functions v1 runtime. However, there are many enterprise scenarios that still need Azure Functions v1 runtime. Although there is no direct support from this OpenAPI extension, you can still take the benefits of using this extension, using the [Azure Functions Proxy][az func proxy] feature.


## Prerequisites ##

To get yourself started, you need to have the followings installed on your local machine.

> [!IMPORTANT]
> This extension is currently available in .NET Core runtime.

* [.NET Core SDK 3.1 LTS][dotnet core sdk]
* [Azure Functions Core Tools][az func core tools]
* [Visual Studio Code][vs code]
* [Visual Studio Extensions for Azure Tools][vs code azure tools]
* [Free Microsoft Azure Account][az account free]


## Create Function App ##

Firs of all, [create a function app on your local machine][az func create].

```bash
func init MyV1FunctionAppProxy --dotnet
```

Navigate to the project directory

```bash
cd MyV1FunctionAppProxy
```

Add a function to your project by using the following command, where the `--name` argument is the same name of your v1 function (`MyHttpTrigger`) and the `--template` argument specifies `HTTP trigger`.

```bash
func new --name MyHttpTrigger --template "HTTP trigger"
```

Inside the function method, replace all code with the following one line of code. It's assumed that the v1 Function has the endpoint of `/api/lorem/ipsum`. The proxy function MUST have the same endpoint.

```csharp
namespace MyV1FunctionAppProxy
{
    public static class MyHttpTrigger
    {
        [FunctionName("MyHttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "lorem/ipsum")] HttpRequest req,
            ILogger log)
        {
            return await Task.FromResult(new OkResult()).ConfigureAwait(false);
        }
    }
}
```


## Add OpenAPI Extension ##

To enable OpenAPI metadata, you will need to install a NuGet package, [Microsoft.Azure.WebJobs.Extensions.OpenApi][az func openapi extension].

```bash
dotnet add package Microsoft.Azure.WebJobs.Extensions.OpenApi
```

Add attribute classes on top of the `FunctionName(...)` decorator.

```csharp
namespace MyV1FunctionAppProxy
{
    public static class MyHttpTrigger
    {
        // Add these three attribute classes below
        [OpenApiOperation(operationId: "lorem", tags: new[] { "name" }, Summary = "Gets lorem ipsum", Description = "This gets lorem ipsum.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "The name", Description = "The name", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Summary = "The response", Description = "This returns the response")]
        // Add these three attribute classes above

        [FunctionName("MyHttpTrigger")]
        public static async Task<IActionResult> Run(
...
```

By doing so, this Function app is now able to generate the OpenAPI definition document on-the-fly. However, it won't work as expected because there is no business logic implemented.


## Add Functions Proxy ##

As this is the proxy app, you need to implement the `proxies.json` file at the root directory. `<my-v1-function-app>` is your Azure Functions v1 app.

```json
{
  "$schema": "http://json.schemastore.org/proxies",
  "proxies": {
    "ProxyLoremIpsum": {
      "matchCondition": {
        "route": "/api/lorem/ipsum",
        "methods": [
          "GET"
        ]
      },
      "backendUri": "https://<my-v1-function-app>.azurewebsites.net/api/lorem/ipsum",
      "requestOverrides": {
        "backend.request.headers": "{request.headers}",
        "backend.request.querystring": "{request.querystring}"
      }
    }
  }
}
```

You also need to update your `MyV1FunctionAppProxy.csproj` file to include this `proxies.json` file during the build time.

```xml
<Project Sdk="Microsoft.NET.Sdk">
  ...
  <ItemGroup>
    ...
    <None Update="proxies.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>
  ...
</Project>
```

Then all your API requests to the proxy function are forwarded to the v1 Function app and processed there.


## Clean-up Resources ##

When you continue to the another step, [Integrating OpenAPI-enabled Azure Functions with Azure API Management][docs apim], you'll need to keep all your resources in place to build on what you've already done.

Otherwise, you can use the following steps to delete the function app and its related resources to avoid incurring any further costs.

1. In Visual Studio Code, press <kbd>F1</kbd> to open the command palette. In the command palette, search for and select `Azure Functions: Open in portal`.
1. Choose your function app, and press <kbd>Enter</kbd>. The function app page opens in the Azure portal.
1. In the **Overview** tab, select the named link next to **Resource group**.

    ![Select the resource group to delete from the function app page][image-10]

1. In the **Resource group** page, review the list of included resources, and verify that they are the ones you want to delete.
1. Select **Delete resource group**, and follow the instructions.

   Deletion may take a couple of minutes. When it's done, a notification appears for a few seconds. You can also select the bell icon at the top of the page to view the notification.

To learn more about Functions costs, see [Estimating Consumption plan costs][az func costs].


## Next Steps ##

You have got an Azure Functions app with OpenAPI metadata enabled. In the next articles, you will be able to integrate this OpenAPI-enabled Azure Functions app with either [Azure API Management][az apim], [Azure Logic Apps][az logapp] or [Power Platform][power platform].

* [Integrating OpenAPI-enabled Azure Functions to Azure API Management][docs apim]
<!-- * [Integrating OpenAPI-enabled Azure Functions to Power Platform][docs powerplatform] -->


[image-10]: images/image-10.png

[docs apim]: integrate-with-apim.md
[docs powerplatform]: integrate-with-powerplatform.md

[dotnet core sdk]: https://dotnet.microsoft.com/download/dotnet-core/3.1?WT.mc_id=dotnet-0000-juyoo

[az account free]: https://azure.microsoft.com/free/?WT.mc_id=dotnet-0000-juyoo

[az func core tools]: https://docs.microsoft.com/azure/azure-functions/functions-run-local?WT.mc_id=dotnet-0000-juyoo
[az func openapi extension]: https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.OpenApi
[az func proxy]: https://docs.microsoft.com/azure/azure-functions/functions-proxies?WT.mc_id=dotnet-0000-juyoo
[az func create]: https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-first-azure-function-azure-cli?tabs=bash%2Cbrowser&pivots=programming-language-csharp&WT.mc_id=dotnet-0000-juyoo
[az func costs]: https://docs.microsoft.com/azure/azure-functions/functions-consumption-costs?WT.mc_id=dotnet-0000-juyoo

[vs code]: https://code.visualstudio.com/
[vs code azure tools]: https://marketplace.visualstudio.com/items?itemName=ms-vscode.vscode-node-azure-pack

[az apim]: https://docs.microsoft.com/azure/api-management/api-management-key-concepts?WT.mc_id=dotnet-0000-juyoo
[az logapp]: https://docs.microsoft.com/azure/logic-apps/logic-apps-overview?WT.mc_id=dotnet-0000-juyoo
[power platform]: https://powerplatform.microsoft.com/?WT.mc_id=dotnet-0000-juyoo
