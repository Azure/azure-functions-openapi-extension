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
        /// Gets the stylesheet to be rendered on the page
        /// </summary>
        /// <returns></returns>
        Task<string> GetStylesheetAsync(string filepath = "dist.custom.css");

        /// <summary>
        /// Gets the javascript to be rendered on the page
        /// </summary>
        /// <returns></returns>
        Task<string> GetJavascriptAsync(string filepath = "dist.custom.js");
    }
}
