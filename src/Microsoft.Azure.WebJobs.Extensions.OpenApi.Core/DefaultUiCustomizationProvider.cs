using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core
{
    /// <summary>
    /// Default implementation of <see cref="IUiCustomizationProvider"/>, providing
    /// empty replacements for custom javascript and stylesheets
    /// </summary>
    public class DefaultUiCustomizationProvider : IUiCustomizationProvider
    {
        /// <inheritdoc/>
        public virtual Task<string> GetJavascriptAsync()
        {
            return Task.FromResult<string>(null);
        }

        /// <inheritdoc/>
        public virtual Task<string> GetStylesheetAsync()
        {
            return Task.FromResult<string>(null);
        }
    }
}
