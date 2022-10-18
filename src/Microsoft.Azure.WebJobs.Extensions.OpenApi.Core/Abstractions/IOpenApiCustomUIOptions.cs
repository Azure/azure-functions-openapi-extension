using System.Threading.Tasks;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// This provides interfaces classes implementing for the custom UI.
    /// </summary>
    public interface IOpenApiCustomUIOptions
    {
        /// <summary>
        /// Gets or sets the filepath or URL for stylesheet for custom UI.
        /// </summary>
        string CustomStylesheetPath { get; set; }

        /// <summary>
        /// Gets or sets filepath or URL for JavaScript for custom UI.
        /// </summary>
        string CustomJavaScriptPath { get; set; }

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
