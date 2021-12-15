using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Model
{
    public class ApiMock
    {
        public string CompiledPath { set; get; }
        public string CompiledDllPath { set; get; }
        public HttpSettings HttpSettings { set; get; }
        public OpenApiInfo OpenApiInfo { set; get; }
    }
}