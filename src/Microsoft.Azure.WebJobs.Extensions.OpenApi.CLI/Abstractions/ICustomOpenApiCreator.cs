using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Abstractions
{
    public interface ICustomOpenApiCreator
    {
        Task<string> CreateOpenApiDocument(
            string apiBaseUrl,
            string compiledDllPath,
            string routePrefix,
            OpenApiInfo openApiInfo,
            OpenApiVersionType version,
            OpenApiFormatType format);
    }
}