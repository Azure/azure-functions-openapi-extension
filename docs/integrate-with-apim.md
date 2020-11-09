# Integrating Open API-enabled Azure Functions with Azure API Management #

[Azure API Management][az apim] provides HTTP API consumers with a consistent way of using API gateway features for back-end API services. You can integrate your Azure Function app with Azure API Management by [directly importing the Function app][az apim azfunc import]. But you can also make use of this Open API extension so that everything will be set up at one go.


## Prerequisites ##

To get yourself started, you need to have the followings installed on your local machine.

* [Free Microsoft Azure Account][az account free]
* [Azure CLI][az cli]


## Create Azure API Management Instance ##

There are several ways to create an Azure API Management instance. But [Azure CLI][az cli apim] will be used here. Run the following command in your shell:

```bash
az apim create \
    -g [RESOURCE_GROUP_NAME] \
    -n [API_MANAGEMENT_INSTANCE_NAME] \
    -l [LOCATION] \
    --sku-name Consumption \
    --publisher-name contoso.com \
    --publisher-email apim@contoso.com
    --no-wait
```

> Make sure that, as creating an Azure API Management instance takes about 30-40 minutes, adding the `--no-wait` parameter would be nice. Also use the SKU of `Consumption` instead of the default SKU (`Developer`) to reduce the instance generation time for this practice. The `Consumption` tier takes up to two minutes for instance creation.


## Create Azure Function App ##

Please refer to [the Azure Function app with the Open API extension enabled](docs/enable-open-api-endpoints.md). Once the Function app is deployed to Azure, make sure you can generate `swagger.json` document on-the-fly.

![swagger.json on Azure][image-11]

Copy the URL of the `swagger.json` for later use.

> The sample Function app code can be found at this [GitHub repository][gh sample v3ioc].


## Import Azure Function App into Azure API Management ##

On you Azure API Management instance, select `APIs` blade, select `+ Add API` tab and click the `OpenAPI` panel.

![APIM APIs blade][image-12]

Paste the URL copied from the previous step into the `OpenAPI specification` field and enter `contoso` into the `API URL suffix` field. All the rest are automatically filled in.

![Import swagger.json][image-13]

Azure Functions API is now successfully imported.

![swagger.json successfully imported][image-14]


## Test API Endpoint through Azure API Management ##

To test the imported API endpoint, go to the newly imported API by clicking the `APIs` blade, clicking the `Open API Sample on Azure...` tab and choosing one of API endpoints, and selecting the `Test` tab.

![Testing imported API endpoint][image-15]

Enter relevant information to the given fields anc click the `Send` button

![Enter necessary information for HTTP API endpoint testing][image-16]

Confirm the test has been successfully performed.

![Test successful][image-17]


## Clean-up Resources ##

When you continue to the another step, [Integrating Open API-enabled Azure Functions to Power Platform][docs powerplatform], you'll need to keep all your resources in place to build on what you've already done.

Otherwise, you can use the following steps to delete the function app and its related resources to avoid incurring any further costs.

1. In Visual Studio Code, press <kbd>F1</kbd> to open the command palette. In the command palette, search for and select `Azure Functions: Open in portal`.
1. Choose your function app, and press <kbd>Enter</kbd>. The function app page opens in the Azure portal.
1. In the **Overview** tab, select the named link next to **Resource group**.

    ![Select the resource group to delete from the function app page][image-10]

1. In the **Resource group** page, review the list of included resources, and verify that they are the ones you want to delete.
1. Select **Delete resource group**, and follow the instructions.

   Deletion may take a couple of minutes. When it's done, a notification appears for a few seconds. You can also select the bell icon at the top of the page to view the notification.

To learn more about Functions costs, see [Estimating Consumption plan costs][az func costs]. To learn more about API Management costs, see [Feature-based comparison of the Azure API Management tiers][az apim costs].


## Next Steps ##

You have got an Azure Functions app with Open API metadata enabled. In the next articles, you will be able to integrate this Open API-enabled Azure Functions app with either [Azure API Management][az apim], [Azure Logic Apps][az logapp] or [Power Platform][power platform].

* [Support Azure Functions v1 with Open API Extension][docs v1 suppport]
* [Integrating Open API-enabled Azure Functions to Power Platform][docs powerplatform]


[image-10]: images/image-10.png
[image-11]: images/image-11.png
[image-12]: images/image-12.png
[image-13]: images/image-13.png
[image-14]: images/image-14.png
[image-15]: images/image-15.png
[image-16]: images/image-16.png
[image-17]: images/image-17.png

[docs v1 support]: azure-functions-v1-support.md
[docs apim]: integrate-with-apim.md
[docs powerplatform]: integrate-with-powerplatform.md

[az account free]: https://azure.microsoft.com/free/?WT.mc_id=dotnet-0000-juyoo

[az func costs]: https://docs.microsoft.com/azure/azure-functions/functions-consumption-costs?WT.mc_id=dotnet-0000-juyoo

[az logapp]: https://docs.microsoft.com/azure/logic-apps/logic-apps-overview?WT.mc_id=dotnet-0000-juyoo
[power platform]: https://powerplatform.microsoft.com/?WT.mc_id=dotnet-0000-juyoo

[az apim]: https://docs.microsoft.com/azure/api-management/api-management-key-concepts?WT.mc_id=dotnet-0000-juyoo
[az apim azfunc import]: https://docs.microsoft.com/azure/azure-functions/functions-openapi-definition?WT.mc_id=dotnet-0000-juyoo
[az apim costs]: https://docs.microsoft.com/azure/api-management/api-management-features?WT.mc_id=dotnet-0000-juyoo

[az cli]: https://docs.microsoft.com/cli/azure/what-is-azure-cli?WT.mc_id=dotnet-0000-juyoo
[az cli apim]: https://docs.microsoft.com/azure/api-management/get-started-create-service-instance-cli?WT.mc_id=dotnet-0000-juyoo

[gh sample v3ioc]: https://github.com/Azure/azure-functions-openapi-extension/tree/main/samples/Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3IoC
