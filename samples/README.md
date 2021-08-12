# Azure Functions Sample Apps #

Here are sample applications with different scenarios.


## With Internal Projects ##

The following sample apps use projects internally, meaning no external NuGet packages are used.


### Azure Functions Runtime V2 ###

* [With IoC Container](./Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V2IoC)
* [Without IoC Container](./Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V2Static)


### Azure Functions Runtime V3 ###

* [With IoC Container](./Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3IoC)
* [Without IoC Container](./Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3Static)
* [Proxy to V1](./Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V1Proxy)


### Azure Functions on .NET 5 ###

* [.NET 5](./Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.V3Net5)


## With External Projects ##

The following sample apps use NuGet packages.


### Azure Functions Runtime V3 ###

* [With IoC Container](https://github.com/justinyoo/azfunc-openapi-dotnet/tree/main/NetCoreApp31.FunctionApp.IoC)
* [Without IoC Container](https://github.com/justinyoo/azfunc-openapi-dotnet/tree/main/NetCoreApp31.FunctionApp.Static)


### Azure Functions on .NET 5 ###

* [With IoC Container](https://github.com/justinyoo/azfunc-openapi-dotnet/tree/main/Net50.FunctionApp.IoC)
* [Without IoC Container](https://github.com/justinyoo/azfunc-openapi-dotnet/tree/main/Net50.FunctionApp.Static)
