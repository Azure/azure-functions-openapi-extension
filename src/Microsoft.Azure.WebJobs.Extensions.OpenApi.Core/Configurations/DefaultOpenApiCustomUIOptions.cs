using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// Default implementation of <see cref="IOpenApiCustomUIOptions"/>, providing
    /// empty replacements for custom javascript and stylesheets
    /// </summary>
    public class DefaultOpenApiCustomUIOptions : IOpenApiCustomUIOptions
    {
        private readonly Assembly _assembly;

        /// <summary>
        /// Creates a new 
        /// </summary>
        /// <param name="assembly"></param>
        public DefaultOpenApiCustomUIOptions(Assembly assembly)
        {
            this._assembly = assembly.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        public virtual async Task<string> GetJavascriptAsync(string filepath = "dist.custom.js")
        {
            using (var stream = this._assembly.GetManifestResourceStream($"{this._assembly.GetName().Name}.{filepath}"))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return await reader.ReadToEndAsync();
                    }
                }
            }

            return string.Empty;
        }

        /// <inheritdoc/>
        public virtual async Task<string> GetStylesheetAsync(string filepath = "dist.custom.css")
        {
            using (var stream = this._assembly.GetManifestResourceStream($"{this._assembly.GetName().Name}.{filepath}"))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return await reader.ReadToEndAsync();
                    }
                }
            }

            return string.Empty;
        }
    }
}
