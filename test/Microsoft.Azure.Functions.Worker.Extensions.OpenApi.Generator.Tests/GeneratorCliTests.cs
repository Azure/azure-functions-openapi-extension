using System.Reflection;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Generator.Tests;

[TestClass]
public class GeneratorCliTests
{
    private static string SampleAssemblyPath => Path.Combine(
        AppContext.BaseDirectory,
        "Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.OutOfProc.dll");

    [TestMethod]
    public void Given_KeyValue_Args_When_ParseArgs_Then_Maps_Pairs()
    {
        var parsed = GeneratorCli.ParseArgs(["--assembly", "app.dll", "--route-prefix", "v1"]);

        parsed["assembly"].Should().Be("app.dll");
        parsed["route-prefix"].Should().Be("v1");
    }

    [TestMethod]
    public void Given_Flag_Without_Value_When_ParseArgs_Then_Sets_True()
    {
        var parsed = GeneratorCli.ParseArgs(["--verbose", "--output", "out.json"]);

        parsed["verbose"].Should().Be("true");
        parsed["output"].Should().Be("out.json");
    }

    [TestMethod]
    public void Given_Mixed_Case_Keys_When_ParseArgs_Then_Lookup_Is_Case_Insensitive()
    {
        var parsed = GeneratorCli.ParseArgs(["--Assembly", "app.dll"]);

        parsed["assembly"].Should().Be("app.dll");
    }

    [TestMethod]
    public async Task Given_No_Assembly_Arg_When_RunAsync_Then_Returns_Error_Code()
    {
        var exitCode = await GeneratorCli.RunAsync(["--output", "out.json"]);

        exitCode.Should().Be(1);
    }

    [TestMethod]
    public async Task Given_Missing_Assembly_File_When_RunAsync_Then_Returns_Error_Code()
    {
        var exitCode = await GeneratorCli.RunAsync(["--assembly", "does-not-exist.dll"]);

        exitCode.Should().Be(1);
    }

    [TestMethod]
    public async Task Given_Valid_Assembly_When_RunAsync_Then_Writes_Document_And_Returns_Zero()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"swagger-{Guid.NewGuid():N}.json");

        try
        {
            var exitCode = await GeneratorCli.RunAsync(
                ["--assembly", SampleAssemblyPath, "--output", outputPath, "--route-prefix", "api"]);

            exitCode.Should().Be(0);
            File.Exists(outputPath).Should().BeTrue();
            (await File.ReadAllTextAsync(outputPath)).Should().Contain("\"openapi\"");
        }
        finally
        {
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
        }
    }
}
