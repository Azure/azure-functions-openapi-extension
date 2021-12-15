using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Extension
{
    public static class ProjectPathExtensions
    {
        public static readonly char DirectorySeparator = Path.DirectorySeparatorChar;

        public static string TrimProjectPath(this string path)
        {
            var filePath = !Path.IsPathFullyQualified(path)
                ? $"{Environment.CurrentDirectory.TrimEnd(DirectorySeparator)}{DirectorySeparator}{path}"
                : path;

            return new DirectoryInfo(filePath).FullName.TrimEnd(DirectorySeparator);
        }

        public static string GetCsProjFileName(this string path)
        {
            var filePath = !Path.IsPathFullyQualified(path)
                ? $"{Environment.CurrentDirectory.TrimEnd(DirectorySeparator)}{DirectorySeparator}{path}"
                : path;

            var segments = new DirectoryInfo(filePath).FullName.Split(new[]
            {
                DirectorySeparator
            }, StringSplitOptions.RemoveEmptyEntries);

            return $"{segments.Last()}.csproj";
        }

        public static string GetProjectDllFileName(this string projectPath, string csprojFileName)
        {
            var doc = new XmlDocument();

            doc.Load($"{projectPath}{DirectorySeparator}{csprojFileName}");

            var elements = doc.GetElementsByTagName(nameof(AssemblyName));

            var dllName = elements?.Cast<XmlNode>()?.FirstOrDefault()?.InnerText;

            return string.IsNullOrWhiteSpace(dllName)
                ? csprojFileName.Replace(".csproj", ".dll")
                : $"{dllName}.dll";
        }

        public static string GetProjectCompiledPath(this string path, string configuration, string targetFramework)
        {
            return $"{path.TrimEnd(DirectorySeparator)}{DirectorySeparator}bin{DirectorySeparator}{configuration}{DirectorySeparator}{targetFramework}";
        }

        public static string GetProjectCompiledDllPath(this string compiledPath, string dllFileName)
        {
            return $"{compiledPath}{DirectorySeparator}{dllFileName}";
        }

        public static string GetProjectHostJsonPath(this string compiledPath)
        {
            return $"{compiledPath}{DirectorySeparator}host.json";
        }

        public static string GetOutputPath(this string output, string compiledPath)
        {
            return !Path.IsPathFullyQualified(output)
                ? $"{compiledPath}{DirectorySeparator}{output}"
                : output;
        }
    }
}