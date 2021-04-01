using System.Threading.Tasks;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// Interface for a custom UI provider that can provide custom javascript
    /// and CSS to be populated on a page
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
        /// Gets the stylesheet to be rendered on the page.
        /// </summary>
        /// <returns>The stylesheet string for custom UI.</returns>
        Task<string> GetStylesheetAsync();

        /// <summary>
        /// Gets the javascript to be rendered on the page.
        /// </summary>
        /// <returns>The JavaScript string for custom UI.</returns>
        Task<string> GetJavaScriptAsync();
    }
}
