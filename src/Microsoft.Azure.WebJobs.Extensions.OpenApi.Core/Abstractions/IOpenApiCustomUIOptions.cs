using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// This provides interfaces classes implementing for the custom UI.
    /// </summary>
    public interface IOpenApiCustomUIOptions
    {
        /// <summary>
        /// Gets the filepath for stylesheet for custom UI.
        /// </summary>
        string CustomStylesheetPath { get; }

        /// <summary>
        /// Gets filepath for JavaScript for custom UI.
        /// </summary>
        string CustomJavaScriptPath { get; }

        /// <summary>
        /// Gets metaTags for Favicon for custom UI.
        /// </summary>
        IEnumerable<string> CustomFaviconMetaTags { get; }

        /// <summary>
        /// Gets the stylesheet to be rendered on the page.
        /// </summary>
        /// <returns>The stylesheet string for custom UI.</returns>
        Task<string> GetStylesheetAsync();

        /// <summary>
        /// Gets the javascript to be rendered on the page.
        /// </summary>
        /// <returns>The JavaScript string for custom UI.</returns>
        Task<string> GetJavaScriptAsync();

        /// <summary>
        /// Gets the favicon meta tags to be rendered on the page.
        /// </summary>
        /// <returns>The Favicon meta tags for custom UI.</returns>
        Task<IEnumerable<string>> GetFaviconMetaTagsAsync();
    }
}
