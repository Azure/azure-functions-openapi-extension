# Build-time OpenAPI Document Generation #

For **isolated-worker** Azure Function Apps you can emit the OpenAPI (`swagger.json`) document at
**build time**, without running the Functions host. The
`Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Generator` package reflects over your built app
assembly using the **same builder pipeline** as the runtime OpenAPI document endpoint, so the output
matches what the running app would serve &mdash; but **no host, app settings or network** are
required. This makes it well-suited to generating API specs deterministically in CI/CD.

> This complements [Generic CI/CD Pipeline Support](./generic-cicd-pipeline-support.md). Use that
> approach when you need to run the host (for example to honour runtime-resolved settings); use this
> one for fast, host-less generation as part of `dotnet build`.


## Usage ##

Add the package reference. It is a **build-only** dependency &mdash; no runtime dependency is added to
your app:

```xml
<PackageReference Include="Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Generator" Version="1.0.*" PrivateAssets="all" />
```

On `dotnet build`, `swagger.json` is written next to the built assembly (`$(TargetDir)swagger.json`).

To apply it to **every** Function App in a solution at once, add that line to a shared
`Directory.Build.props`. Non-Function-App projects (libraries, test projects) are detected and
skipped, so a blanket reference is safe.


## Metadata ##

The generator auto-discovers any class implementing `IOpenApiConfigurationOptions` in your app (the
same class the runtime endpoint uses) for the title, description and version. If your app registers
its options *inline in dependency injection* instead of as a class, extract them into a discoverable
class to get rich metadata &mdash; otherwise a valid document with default metadata is produced.


## Requirements ##

* The app must be an isolated-worker Function App (`AzureFunctionsVersion` is set) referencing
  `Microsoft.Azure.Functions.Worker.Extensions.OpenApi`.
* A `host.json` must be present in the build output (standard for every Function App).


## MSBuild properties ##

| Property | Default | Purpose |
| --- | --- | --- |
| `GenerateOpenApiOnBuild` | `true` | Set `false` to disable generation for a project. |
| `OpenApiOutputPath` | `$(TargetDir)swagger.json` | Where the document is written. |
| `OpenApiRoutePrefix` | `api` | Route prefix used for the host-agnostic relative `server` URL. |

For example, to write the document to a fixed location:

```xml
<PropertyGroup>
  <OpenApiOutputPath>$(MSBuildProjectDirectory)/openapi/swagger.json</OpenApiOutputPath>
</PropertyGroup>
```
