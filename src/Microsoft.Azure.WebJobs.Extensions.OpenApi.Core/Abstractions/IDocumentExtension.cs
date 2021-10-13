using Microsoft.AspNetCore.Http;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// This provides interfaces to the classes that extend the OpenApi document.
    /// </summary>
    public interface IDocumentExtension
    {
        IDocument ExtendDocument(IDocument document, HttpRequest request);
    }
}
