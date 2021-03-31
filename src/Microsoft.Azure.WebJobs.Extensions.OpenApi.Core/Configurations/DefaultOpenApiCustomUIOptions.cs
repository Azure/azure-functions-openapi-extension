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
        /// Initializes a new instance of the <see cref="DefaultOpenApiCustomUIOptions"/> class.
        /// </summary>
        /// <param name="assembly"><see cref="Assembly"/> instance.</param>
        public DefaultOpenApiCustomUIOptions(Assembly assembly)
        {
            this._assembly = assembly.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        public virtual string CustomStylesheetPath { get; set; } = "dist.custom.css";

        /// <inheritdoc/>
        public virtual string CustomJavaScriptPath { get; set; } = "dist.custom.js";

        /// <inheritdoc/>
        public virtual async Task<string> GetStylesheetAsync(string filepath = "dist.custom.css")
        {
            if (filepath.IsNullOrWhiteSpace())
            {
                filepath = this.CustomStylesheetPath;
            }

            using (var stream = this._assembly.GetManifestResourceStream($"{this._assembly.GetName().Name}.{filepath}"))
            {
                if (stream.IsNullOrDefault())
                {
                    return string.Empty;
                }

                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        /// <inheritdoc/>
        public virtual async Task<string> GetJavaScriptAsync(string filepath = "dist.custom.js")
        {
            if (filepath.IsNullOrWhiteSpace())
            {
                filepath = this.CustomJavaScriptPath;
            }

            using (var stream = this._assembly.GetManifestResourceStream($"{this._assembly.GetName().Name}.{filepath}"))
            {
                if (stream.IsNullOrDefault())
                {
                    return string.Empty;
                }

                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
    }
}
