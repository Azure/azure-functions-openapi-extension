# Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI #

![Build and Test](https://github.com/Azure/azure-functions-openapi-extension/workflows/Build%20and%20Test/badge.svg) [![](https://img.shields.io/static/v1?label=tag&message=cli-*&color=brightgreen)](https://github.com/Azure/azure-functions-openapi-extension/releases) [![](https://img.shields.io/static/v1?label=tag&message=cli-*&color=brightgreen)](https://github.com/Azure/azure-functions-openapi-extension/releases)

This generates OpenAPI document through command-line without having to run the Azure Functions instance. The more details around this CLI can be found on this [blog post](https://devkimchi.com/2020/07/08/generating-open-api-doc-for-azure-functions-in-command-line/).

> **NOTE**: This CLI supports both [OpenAPI 2.0 (aka Swagger)](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/2.0.md) and [OpenAPI 3.0.1](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md) spec.


## Issues? ##

While using this CLI, if you find any issue, please raise a ticket on the [Issue](https://github.com/Azure/azure-functions-openapi-extension/issues) page.


## Getting Started ##

### Download CLI ###

The CLI is available for download at [GitHub](https://github.com/Azure/azure-functions-openapi-extension/releases). It's always tagged with `cli-<version>`. Download the latest version of CLI.

* Linux: `azfuncopenapi-v<version>-netcoreapp3.1-linux-x64.zip`
* MacOS: `azfuncopenapi-v<version>-netcoreapp3.1-osx-x64.zip`
* Windows: `azfuncopenapi-v<version>-netcoreapp3.1-win-x64.zip`


### Generating OpenAPI Document ###

Once you have an Azure Functions instance with [Azure Functions OpenAPI extension](openapi.md) enabled, then you are ready to run this CLI.

For Windows:

```powershell
# PowerShell Console
azfuncopenapi `
    --project <PROJECT_PATH> `
    --configuration Debug `
    --target netcoreapp2.1 `
    --version v2 `
    --format json `
    --output output `
    --console false
```

For Linux/MacOS

```bash
# Bash
./azfuncopenapi \
    --project <PROJECT_PATH> \
    --configuration Debug \
    --target netcoreapp2.1 \
    --version v2 \
    --format json \
    --output output \
    --console false
```

Here are options:

* `-p|--project`: Project path. It can be a fully qualified project path including `.csproj` or project directory. Default is the current directory.
* `-c|--configuration`: Configuration value. It can be either `Debug`, `Release` or something else. Default is `Debug`.
* `-t|--target`: Target framework. It should be `netcoreapp2.x` for Azure Functions v2, and `netcoreapp3.x` for Azure Functions v3. Default is `netcoreapp2.1`.
* `-v|--version`: OpenAPI spec version. It should be either `v2` or `v3`. Default is `v2`.
* `-f|--format`: OpenAPI document format. It should be either `json` or `yaml`. Default is `json`.
* `-o|--output`: Output directory for the generated OpenAPI document. It can be a fully qualified directory path or relative path from `<PROJECT_ROOT>/bin/<CONFIGURATION>/<TARGET_FRAMEWORK>`. Default is `output`.
* `--console`: Value indicating whether to display the generated document to console or not. Default is `false`.


## Roadmap ##

* Distribution through a npm package.
* Project boilerplate generation, if an OpenAPI doc is provided.

