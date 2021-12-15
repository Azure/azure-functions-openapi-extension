using Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Model;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Service.Interface
{
    public interface ICustomApiMockCreator
    {
        ApiMock SetupApi(string projectPath, string configuration, string targetFramework);
    }
}