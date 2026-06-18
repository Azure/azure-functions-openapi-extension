using System.Reflection;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Generator;

/// <summary>
/// Command-line entry point for the build-time OpenAPI generator. Kept as a testable static so the
/// argument parsing and run flow can be exercised without spawning a process.
/// </summary>
public static class GeneratorCli
{
    /// <summary>
    /// Runs the generator.
    /// Usage: <c>--assembly &lt;path-to-function-app.dll&gt; --output &lt;swagger.json&gt; [--route-prefix api]</c>.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    /// <returns>Process exit code (0 on success, 1 on usage/IO error).</returns>
    public static async Task<int> RunAsync(string[] args)
    {
        var parsed = ParseArgs(args);

        if (!parsed.TryGetValue("assembly", out var assemblyPath) || string.IsNullOrWhiteSpace(assemblyPath))
        {
            await Console.Error.WriteLineAsync("Missing required --assembly <path>.");
            return 1;
        }

        var outputPath = parsed.TryGetValue("output", out var o) && !string.IsNullOrWhiteSpace(o)
            ? o
            : "swagger.json";
        var routePrefix = parsed.TryGetValue("route-prefix", out var rp) && !string.IsNullOrWhiteSpace(rp)
            ? rp
            : "api";

        if (!File.Exists(assemblyPath))
        {
            await Console.Error.WriteLineAsync($"Assembly not found: {assemblyPath}");
            return 1;
        }

        var applicationAssembly = Assembly.LoadFrom(Path.GetFullPath(assemblyPath));
        var swagger = await OpenApiSpecGenerator.GenerateAsync(applicationAssembly, routePrefix);

        var fullOutputPath = Path.GetFullPath(outputPath);
        var directory = Path.GetDirectoryName(fullOutputPath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.WriteAllTextAsync(fullOutputPath, swagger);
        Console.WriteLine($"Wrote OpenAPI document to {fullOutputPath}");
        return 0;
    }

    /// <summary>
    /// Parses <c>--key value</c> / <c>--flag</c> argument pairs into a case-insensitive map.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    /// <returns>Parsed key/value pairs.</returns>
    public static Dictionary<string, string> ParseArgs(string[] args)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var i = 0; i < args.Length; i++)
        {
            if (!args[i].StartsWith("--", StringComparison.Ordinal))
            {
                continue;
            }

            var key = args[i][2..];
            if (i + 1 < args.Length && !args[i + 1].StartsWith("--", StringComparison.Ordinal))
            {
                result[key] = args[++i];
            }
            else
            {
                result[key] = "true";
            }
        }

        return result;
    }
}
