using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Services
{
    public class CustomOpenApiWriter : ICustomOpenApiWriter
    {
        public async Task WriteOpenApiToFile(string openApiDocument, string outputPath, OpenApiFormatType format)
        {
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            await File.WriteAllTextAsync($"{outputPath}{ProjectPathExtensions.DirectorySeparator}swagger.{format.ToDisplayName()}", openApiDocument, Encoding.UTF8);
        }
    }
}