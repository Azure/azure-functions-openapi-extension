using System;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Model;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Services
{
    public class CustomApiMockCreator : ICustomApiMockCreator
    {
        public ApiMock SetupApi(string projectPath, string configuration, string targetFramework)
        {
            try
            {
                var trimProjectPath = projectPath.TrimProjectPath();
                var csProjFileName = trimProjectPath.GetCsProjFileName();
                var dllFileName = trimProjectPath.GetProjectDllFileName(csProjFileName);
                var compiledPath = trimProjectPath.GetProjectCompiledPath(configuration, targetFramework);
                var compiledDllPath = compiledPath.GetProjectCompiledDllPath(dllFileName);
                var hostJsonPath = compiledPath.GetProjectHostJsonPath();
                var httpSettings = hostJsonPath.SetHostSettings();
                var openApiInfo = compiledDllPath.SetOpenApiInfo();

                var apiMock = new ApiMock
                {
                    CompiledPath = compiledPath,
                    CompiledDllPath = compiledDllPath,
                    HttpSettings = httpSettings,
                    OpenApiInfo = openApiInfo
                };

                return apiMock;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}