# Enable OpenAPI Endpoints on Azure Functions &ndash; In-Process Model #

[OpenAPI metadata][openapi] supports in Azure Functions is now available with this extension, [Azure Functions OpenAPI Extension (In-Process Worker)][az func openapi extension]. With this extension, you can directly let your API endpoints be discoverable.

> [!IMPORTANT]
> This extension supports only Azure Functions v2 and onwards. If you want to get your Azure Functions v1 supported, find [this preview document][az func openapi v1 preview] or [community contribution][az func openapi community]. You may also want to see [this out-of-process model][docs out-of-proc].

[OpenAPI metadata][openapi] allows wide variety of other software and applications to consume an Azure Functions app hosting HTTP APIs. The software and applications include Microsoft products and services like [Power Platform][power platform], [API Management][az apim] and third-party tools like [Postman][postman].


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
func init MyOpenApiFunctionApp --dotnet
```

Navigate to the project directory

```bash
cd MyOpenApiFunctionApp
```

Add a function to your project by using the following command, where the `--name` argument is the unique name of your function (`MyHttpTrigger`) and the `--template` argument specifies `HTTP trigger`.

```bash
func new --name MyHttpTrigger --template "HTTP trigger"
```

Your Azure Functions app structure might look like this:

![Azure Functions app structure in Visual Studio Code][image-01]

Run the Function app on your local by running the command below:

```bash
func host start
```

Open a web browser and type the following URL, and you will be able to see the Functions app is up and running, which returns the response.

```http
http://localhost:7071/api/MyHttpTrigger?name=OpenApi
```

![Azure Functions run result on a web browser][image-02]


## Enable OpenAPI Document ##

To enable OpenAPI document, you will need to install a NuGet package, [Microsoft.Azure.WebJobs.Extensions.OpenApi][az func openapi extension].

```bash
dotnet add package Microsoft.Azure.WebJobs.Extensions.OpenApi
```

With [Visual Studio Code][vs code], open your HTTP trigger, `MyHttpTrigger.cs`, to enable the OpenAPI metadata, and add attribute classes on top of the `FunctionName(...)` decorator.

```csharp
namespace MyOpenApiFunctionApp
{
    public static class MyHttpTrigger
    {
        // Add these four attribute classes below
        [OpenApiOperation(operationId: "getName", tags: new[] { "name" }, Summary = "Gets the name", Description = "This gets the name.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "The name", Description = "The name", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Summary = "The response", Description = "This returns the response")]
        // Add these four attribute classes above

        [FunctionName("MyHttpTrigger")]
        public static async Task<IActionResult> Run(
...
```

Run the Function app again on your local by running the command below:

```bash
func host start
```

Open your web browser and type the following URL, and you will be able to see the Swagger UI page that describes your HTTP API endpoint above.

```http
http://localhost:7071/api/swagger/ui
```

![Swagger UI for Azure Functions app on local machine][image-03]

Copy the link in the search bar at the top of the page and open it on another web browser window, and you will be able to see the OpenAPI 2.0 document generated on-the-fly.

![swagger.json on local machine][image-04]


## Sign-in to Azure ##

Before you can publish your app, you must sign in to Azure.

1. If you aren't already signed in, choose the Azure icon in the Activity bar, then in the **Azure: Functions** area, choose **Sign in to Azure...**. If you don't already have one, you can [Create a free Azure account][az account free]. Students can [create a free Azure account for Students][az account free students].

    ![Sign in to Azure within VS Code][image-05]

    If you're already signed in, go to the next section.

1. When prompted in the browser, choose your Azure account and sign in using your Azure account credentials.

1. After you've successfully signed in, you can close the new browser window. The subscriptions that belong to your Azure account are displayed in the Side bar.


## Deploy Function App to Azure ##

Once logged into Azure, create a function app and related resources in your Azure subscription and then deploy your code.

> [!IMPORTANT]
> Deploying to an existing function app overwrites the content of that app in Azure.

1. Choose the Azure icon in the Activity bar, then in the **Azure: Functions** area, choose the **Deploy to function app...** button.

    ![Publish your project to Azure][image-06]

1. Provide the following information at the prompts:

    * **Select folder**: Choose a folder from your workspace or browse to one that contains your function app. You won't see this if you already have a valid function app opened.
    * **Select subscription**: Choose the subscription to use. You won't see this if you only have one subscription.
    * **Select Function App in Azure**: Choose `- Create new Function App`. (Don't choose the `Advanced` option, which isn't covered in this article.)
    * **Enter a globally unique name for the function app**: Type a name that is valid in a URL path. The name you type is validated to make sure that it's unique in Azure Functions.
    * **Select a location for new resources**:  For better performance, choose a [region][az region] near you.

1. When completed, the following Azure resources are created in your subscription, using names based on your function app name:

    * A resource group, which is a logical container for related resources.
    * A standard Azure Storage account, which maintains state and other information about your projects.
    * A consumption plan, which defines the underlying host for your serverless function app. 
    * A function app, which provides the environment for executing your function code. A function app lets you group functions as a logical unit for easier management, deployment, and sharing of resources within the same hosting plan.
    * An Application Insights instance connected to the function app, which tracks usage of your serverless function.

    A notification is displayed after your function app is created and the deployment package is applied.

1. Select **View output** in this notification to view the creation and deployment results, including the Azure resources that you created. If you miss the notification, select the bell icon in the lower right corner to see it again.

    ![Create complete notification][image-07]


## Run Function App in Azure ##

1. Back in the **Azure: Functions** area in the side bar, expand the new function app under your subscription. Expand **Functions**, and you will be able to see several functions including `RenderSwaggerUI`. Right-click (Windows) or <kbd>Ctrl -</kbd> click (macOS) on `RenderSwaggerUI`, and then choose **Copy Function URL**.

    ![Copy the function URL for the new HTTP trigger][image-08]

1. Paste this URL for the HTTP request into your browser's address bar, and execute the request. The URL that calls your HTTP-triggered function should be in the following format:

    ```http
    http://<functionappname>.azurewebsites.net/api/swagger/ui
    ```

    You should be able to see the same Swagger UI page as what you saw in your local machine.

    ![Swagger UI for Azure Functions app on Azure][image-09]


## Clean-up Resources ##

When you continue to the next step, [Integrating OpenAPI-enabled Azure Functions with Azure API Management][docs apim], you'll need to keep all your resources in place to build on what you've already done.

Otherwise, you can use the following steps to delete the function app and its related resources to avoid incurring any further costs.

1. In Visual Studio Code, press <kbd>F1</kbd> to open the command palette. In the command palette, search for and select `Azure Functions: Open in portal`.
2. Choose your function app, and press <kbd>Enter</kbd>. The function app page opens in the Azure portal.
3. In the **Overview** tab, select the named link next to **Resource group**.

    ![Select the resource group to delete from the function app page][image-10]

4. In the **Resource group** page, review the list of included resources, and verify that they are the ones you want to delete.
5. Select **Delete resource group**, and follow the instructions.

   Deletion may take a couple of minutes. When it's done, a notification appears for a few seconds. You can also select the bell icon at the top of the page to view the notification.

To learn more about Functions costs, see [Estimating Consumption plan costs][az func costs].


## Next Steps ##

You have got an Azure Functions app with OpenAPI metadata enabled. In the next articles, you will be able to integrate this OpenAPI-enabled Azure Functions app with either [Azure API Management][az apim], [Azure Logic Apps][az logapp] or [Power Platform][power platform].

* [Configuring OpenAPI Document and Swagger UI Permission and Visibility][docs ui configuration]
* [Customising OpenAPI Document and Swagger UI][docs ui customisation]
* [Enable OpenAPI Endpoints on Azure Functions &ndash; Out-of-Process Model][docs out-of-proc]
* [Support Azure Functions v1 with OpenAPI Extension][docs v1 support]
* [Integrating OpenAPI-enabled Azure Functions to Azure API Management][docs apim]
<!-- * [Integrating OpenAPI-enabled Azure Functions to Power Platform][docs powerplatform] -->


[image-01]: images/image-01.png
[image-02]: images/image-02.png
[image-03]: images/image-03.png
[image-04]: images/image-04.png
[image-05]: images/image-05.png
[image-06]: images/image-06.png
[image-07]: images/image-07.png
[image-08]: images/image-08.png
[image-09]: images/image-09.png
[image-10]: images/image-10.png

[docs ui configuration]: openapi.md#Configure-Authorization-Level
[docs ui customisation]: openapi-core.md#OpenAPI-Metadata-Configuration
[docs out-of-proc]: ./openapi-out-of-proc.md
[docs v1 support]: azure-functions-v1-support.md
[docs apim]: integrate-with-apim.md
[docs powerplatform]: integrate-with-powerplatform.md

[dotnet core sdk]: https://dotnet.microsoft.com/download/dotnet-core/3.1?WT.mc_id=dotnet-0000-juyoo

[az account free]: https://azure.microsoft.com/free/?WT.mc_id=dotnet-0000-juyoo
[az account free students]: https://azure.microsoft.com/free/students/?WT.mc_id=dotnet-0000-juyoo

[az func core tools]: https://docs.microsoft.com/azure/azure-functions/functions-run-local?WT.mc_id=dotnet-0000-juyoo
[az func openapi extension]: https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.OpenApi
[az func openapi v1 preview]: https://docs.microsoft.com/azure/azure-functions/functions-api-definition?WT.mc_id=dotnet-0000-juyoo
[az func openapi community]: https://github.com/aliencube/AzureFunctions.Extensions
[az func create]: https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-first-azure-function-azure-cli?tabs=bash%2Cbrowser&pivots=programming-language-csharp&WT.mc_id=dotnet-0000-juyoo
[az func costs]: https://docs.microsoft.com/azure/azure-functions/functions-consumption-costs?WT.mc_id=dotnet-0000-juyoo

[az apim]: https://docs.microsoft.com/azure/api-management/api-management-key-concepts?WT.mc_id=dotnet-0000-juyoo
[az logapp]: https://docs.microsoft.com/azure/logic-apps/logic-apps-overview?WT.mc_id=dotnet-0000-juyoo
[az region]: https://azure.microsoft.com/regions/?WT.mc_id=dotnet-0000-juyoo
[power platform]: https://powerplatform.microsoft.com/?WT.mc_id=dotnet-0000-juyoo
[openapi]: https://www.openapis.org/
[postman]: https://www.postman.com/

[vs code]: https://code.visualstudio.com/
[vs code azure tools]: https://marketplace.visualstudio.com/items?itemName=ms-vscode.vscode-node-azure-pack
