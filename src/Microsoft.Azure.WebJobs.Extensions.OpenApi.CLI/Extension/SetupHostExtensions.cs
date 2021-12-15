using System;
using System.Linq;
using System.Reflection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Extension
{
    public static class SetupHostExtensions
    {
        public static HttpSettings SetHostSettings(this string hostJsonPath)
        {
            var host = new ConfigurationBuilder()
                .SetBasePath(hostJsonPath.Replace("host.json", ""))
                .AddJsonFile("host.json")
                .AddEnvironmentVariables()
                .Build();

            var version = host.GetSection("version").Value;

            HttpSettings hostJsonSetting;
            if (string.IsNullOrWhiteSpace(version))
                hostJsonSetting = host.Get<HttpSettings>("http");
            else if (version.Equals("2.0", StringComparison.CurrentCultureIgnoreCase))
                hostJsonSetting = host.Get<HttpSettings>("extensions:http");
            else
                hostJsonSetting = host.Get<HttpSettings>("http");

            return hostJsonSetting;
        }

        public static OpenApiInfo SetOpenApiInfo(this string compiledDllPath)
        {
            var assembly = Assembly.LoadFrom(compiledDllPath);
            var type = assembly.GetLoadableTypes().SingleOrDefault(p => p.GetInterface(nameof(IOpenApiConfigurationOptions), true).IsNullOrDefault() == false);
            return !type.IsNullOrDefault() ? (Activator.CreateInstance(type) as IOpenApiConfigurationOptions)?.Info : new DefaultOpenApiConfigurationOptions().Info;
        }
    }
}