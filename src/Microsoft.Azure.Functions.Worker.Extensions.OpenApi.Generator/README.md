# Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Generator

Build-time OpenAPI (`swagger.json`) generation for isolated-worker Azure Function Apps.

It reflects over the built app assembly using the same pipeline as the runtime OpenAPI document
endpoint &mdash; **no Functions host, app settings or network** are needed. The document is emitted
into the build output on every build, which makes it ideal for generating API specs in CI.

## Usage

Add the package reference (build-only &mdash; no runtime dependency is added to your app):

```xml
<PackageReference Include="Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Generator" Version="1.0.*" PrivateAssets="all" />
```

To apply it to **every** Function App in a solution at once, add that line to a shared
`Directory.Build.props`. Non-Function-App projects (libraries, tests) are detected and skipped, so a
blanket reference is safe.

On `dotnet build`, `swagger.json` is written next to the built assembly (`$(TargetDir)swagger.json`).

### Metadata

The generator auto-discovers any class implementing `IOpenApiConfigurationOptions` in your app (the
same class the runtime endpoint uses) for the title/description/version. If your app registers its
options *inline in DI* instead of as a class, extract them into a discoverable class to get rich
metadata &mdash; otherwise a valid document with default metadata is produced.

### Requirements

- The app must be an isolated-worker Function App (`AzureFunctionsVersion` set) referencing
  `Microsoft.Azure.Functions.Worker.Extensions.OpenApi`.
- A `host.json` must be present in the build output (standard for every Function App).

## MSBuild knobs

| Property | Default | Purpose |
| --- | --- | --- |
| `GenerateOpenApiOnBuild` | `true` | Set `false` to disable generation for a project. |
| `OpenApiOutputPath` | `$(TargetDir)swagger.json` | Where the document is written. |
| `OpenApiRoutePrefix` | `api` | Route prefix used for the host-agnostic relative `server` URL. |
