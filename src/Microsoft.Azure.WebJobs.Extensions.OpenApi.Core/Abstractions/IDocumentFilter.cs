using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// This interface allows creating custom document filters to modify the contents of the OpenApi / Swagger documentation before it is rendered.
    /// </summary>
    public interface IDocumentFilter
    {
        /// <summary>
        /// This method is invoked after the <see cref="IDocument"/> has been built and just before it is rendered and returned to the client.
        /// </summary>
        /// <param name="req"><see cref="IHttpRequestDataObject"/> instance.</param>
        /// <param name="document"><see cref="OpenApiDocument"/> instance.</param>
        void Apply(IHttpRequestDataObject req, OpenApiDocument document);
    }
}
