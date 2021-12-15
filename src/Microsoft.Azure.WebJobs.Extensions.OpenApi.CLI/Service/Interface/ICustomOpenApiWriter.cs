using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Service.Interface
{
    public interface ICustomOpenApiWriter
    {
        Task WriteOpenApiToFile(string openApiDocument, string outputPath, OpenApiFormatType format);
    }
}