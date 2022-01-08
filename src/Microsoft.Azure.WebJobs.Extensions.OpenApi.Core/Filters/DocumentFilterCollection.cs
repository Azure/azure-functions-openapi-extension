using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Filters
{
    /// <summary>
    /// This represents the collection entity for <see cref="IDocumentFilter"/> instances.
    /// </summary>
    public class DocumentFilterCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentFilterCollection"/> class.
        /// </summary>
        /// <param name="documentFilters">List of <see cref="IDocumentFilter"/> instances.</param>
        public DocumentFilterCollection(List<IDocumentFilter> documentFilters)
        {
            this.DocumentFilters = documentFilters;
        }

        /// <summary>
        /// Gets the list of <see cref="IDocumentFilter"/> instances.
        /// </summary>
        public List<IDocumentFilter> DocumentFilters { get; set; }
    }
}
