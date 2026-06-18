using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Generator;

/// <summary>
/// Generates an OpenAPI document for an isolated-worker Function App offline, by reflecting over the
/// app assembly's <c>[OpenApiOperation]</c> functions. The Functions host is never booted - no app
/// settings and no network access are required. This mirrors the builder pipeline the runtime
/// OpenAPI document endpoint uses, so the output matches what the running app would serve.
/// </summary>
public static class OpenApiSpecGenerator
{
    /// <summary>
    /// Generates the OpenAPI v3 JSON document for the given Function App assembly.
    /// </summary>
    /// <param name="applicationAssembly">The built Function App assembly to reflect over.</param>
    /// <param name="routePrefix">Route prefix used for the host-agnostic relative server URL.</param>
    /// <returns>The serialised OpenAPI v3 JSON document.</returns>
    public static async Task<string> GenerateAsync(Assembly applicationAssembly, string routePrefix)
    {
        var options = ResolveConfigurationOptions(applicationAssembly);

        // The deployed host is unknown at generation time, so emit a single relative "/{prefix}"
        // server and disable host-name injection - otherwise AddServer falls back to the stub
        // request's host and leaks a bogus "localhost" server.
        options.IncludeRequestingHostName = false;
        if (options.Servers is null || options.Servers.Count == 0)
        {
            options.Servers = [new OpenApiServer { Url = $"/{routePrefix.TrimStart('/')}" }];
        }

        var context = new OpenApiHttpTriggerContext(options);

        return await context.Document
            .InitialiseDocument()
            .AddMetadata(context.OpenApiConfigurationOptions.Info)
            .AddServer(new OfflineHttpRequest(), routePrefix, context.OpenApiConfigurationOptions)
            .AddNamingStrategy(context.NamingStrategy)
            .AddVisitors(context.GetVisitorCollection())
            .Build(applicationAssembly, OpenApiVersionType.V3)
            .ApplyDocumentFilters(context.GetDocumentFilterCollection())
            .RenderAsync(OpenApiSpecVersion.OpenApi3_0, OpenApiFormat.Json);
    }

    /// <summary>
    /// Discovers a concrete <see cref="IOpenApiConfigurationOptions"/> in the app assembly (the same
    /// metadata the runtime endpoint uses), falling back to defaults when the app has none.
    /// </summary>
    public static IOpenApiConfigurationOptions ResolveConfigurationOptions(Assembly assembly)
    {
        var optionsType = SafeGetTypes(assembly).FirstOrDefault(t =>
            typeof(IOpenApiConfigurationOptions).IsAssignableFrom(t)
            && t is { IsAbstract: false, IsInterface: false }
            && t.GetConstructor(Type.EmptyTypes) is not null);

        if (optionsType is not null)
        {
            return (IOpenApiConfigurationOptions)Activator.CreateInstance(optionsType)!;
        }

        return new DefaultOpenApiConfigurationOptions();
    }

    private static IEnumerable<Type> SafeGetTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(t => t is not null)!;
        }
    }
}
