using System.Reflection;
using System.Text.Json;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Generator.Tests;

[TestClass]
public class OpenApiSpecGeneratorTests
{
    // The OutOfProc sample app is built and copied next to the test assembly via ProjectReference.
    private const string SampleAssemblyName =
        "Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.OutOfProc.dll";

    private static string SampleAssemblyPath =>
        Path.Combine(AppContext.BaseDirectory, SampleAssemblyName);

    private static Assembly LoadSampleAssembly() =>
        Assembly.LoadFrom(SampleAssemblyPath);

    [TestMethod]
    public async Task Given_SampleApp_When_GenerateAsync_Then_Returns_Valid_OpenApi3_Json()
    {
        var json = await OpenApiSpecGenerator.GenerateAsync(LoadSampleAssembly(), "api");

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        root.GetProperty("openapi").GetString().Should().StartWith("3.0");
        root.GetProperty("info").GetProperty("title").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [TestMethod]
    public async Task Given_SampleApp_When_GenerateAsync_Then_Document_Has_Operations()
    {
        var json = await OpenApiSpecGenerator.GenerateAsync(LoadSampleAssembly(), "api");

        using var doc = JsonDocument.Parse(json);
        var paths = doc.RootElement.GetProperty("paths");

        paths.EnumerateObject().Should().NotBeEmpty("the sample app declares [OpenApiOperation] functions");
    }

    [TestMethod]
    public async Task Given_RoutePrefix_When_GenerateAsync_Then_Emits_Relative_Server_Without_Host()
    {
        var json = await OpenApiSpecGenerator.GenerateAsync(LoadSampleAssembly(), "api");

        using var doc = JsonDocument.Parse(json);
        var servers = doc.RootElement.GetProperty("servers").EnumerateArray()
            .Select(s => s.GetProperty("url").GetString())
            .ToList();

        servers.Should().ContainSingle().Which.Should().Be("/api");
        servers.Should().NotContain(url => url!.Contains("localhost"));
    }

    [TestMethod]
    public void Given_Assembly_Without_Options_When_ResolveConfigurationOptions_Then_Falls_Back_To_Default()
    {
        // System.Private.CoreLib contains no IOpenApiConfigurationOptions implementation.
        var options = OpenApiSpecGenerator.ResolveConfigurationOptions(typeof(string).Assembly);

        options.Should().BeOfType<DefaultOpenApiConfigurationOptions>();
    }
}
