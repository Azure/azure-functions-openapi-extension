using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Tests.Fakes
{
    public class FakeDocumentExtension : IDocumentExtension
    {
        public IDocument ExtendDocument(IDocument document, HttpRequest request)
        {
            document.OpenApiDocument.Paths["/MockExtension"] = new OpenApiPathItem
            {
                Description = "Mock Path",
            };

            return document;
        }
    }
}
