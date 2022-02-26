using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.DocumentFilters
{
    internal class RewriteDescriptionDocumentFilter : IDocumentFilter
    {
        public void Apply(IHttpRequestDataObject request, OpenApiDocument document)
        {
            document
                .Paths["/get-documentfilter"]
                .Operations[OperationType.Get]
                .Responses["200"]
                .Description += " rewritten";
        }
    }
}
