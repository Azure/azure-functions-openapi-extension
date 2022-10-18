FROM mcr.microsoft.com/dotnet/sdk:6.0-jammy

LABEL "com.github.actions.name"="OpenAPI Generator from Azure Functions"
LABEL "com.github.actions.description"="Generates OpenAPI document from Azure Functions app"
LABEL "com.github.actions.icon"="file-text"
LABEL "com.github.actions.color"="blue"

LABEL "repository"="https://github.com/Azure/azure-functions-openapi-extension"
LABEL "homepage"="http://azure.com"
LABEL "maintainer"="Azure Functions OpenAPI <azfunc-openapi@microsoft.com>"

# ⬇️⬇️⬇️ Uncomment the lines below for local testing ⬇️⬇️⬇️
# ENV GITHUB_WORKSPACE=/home/runner/work/azure-functions-openapi-extension/azure-functions-openapi-extension/samples/Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.InProc
# ENV WORKDIR=/home/runner/work/azure-functions-openapi-extension/azure-functions-openapi-extension/samples/Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.InProc
# ⬆️⬆️⬆️ Uncomment the lines above for local testing ⬆️⬆️⬆️

# Install Azure Functions Core Tools
RUN wget -q https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN apt-get update && apt-get install -y azure-functions-core-tools-4

ADD entrypoint.ps1 /entrypoint.ps1
RUN chmod +x /entrypoint.ps1

ENTRYPOINT ["pwsh", "-File", "/entrypoint.ps1"]
