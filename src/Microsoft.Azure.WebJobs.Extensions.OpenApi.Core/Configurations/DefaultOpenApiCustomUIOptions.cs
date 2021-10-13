using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// This represents the options entity for custom UI configuration.
    /// </summary>
    public class DefaultOpenApiCustomUIOptions : IOpenApiCustomUIOptions
    {
        private static readonly HttpClient http = new HttpClient();

        private readonly Assembly _assembly;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultOpenApiCustomUIOptions"/> class.
        /// </summary>
        /// <param name="assembly"><see cref="Assembly"/> instance.</param>
        public DefaultOpenApiCustomUIOptions(Assembly assembly)
        {
            this._assembly = assembly.ThrowIfNullOrDefault();
        }

        private readonly string faviconPath_32 = "dist.favicon-32x32.png";
        private readonly string faviconPath_16 = "dist.favicon-16x16.png";

        /// <inheritdoc/>
        public virtual string CustomStylesheetPath { get; } = "dist.custom.css";

        /// <inheritdoc/>
        public virtual string CustomJavaScriptPath { get; } = "dist.custom.js";

        /// <inheritdoc/>
        public virtual IEnumerable<string> CustomFaviconMetaTags { get; } = new List<string>()
        {
            "<link rel=\"icon\" type=\"image/png\" href=\"dist.favicon-32x32.png\" sizes=\"32x32\" />",
            "<link rel=\"icon\" type=\"image/png\" href=\"dist.favicon-16x16.png\" sizes=\"16x16\" />"
        };

        /// <inheritdoc/>
        public virtual IEnumerable<string> GetFaviconMetaTags()
        {
            var metaTags = new List<string>();
            foreach(var faviconMetaTag in this.CustomFaviconMetaTags)
            {
                var metaTag = faviconMetaTag;
                if (faviconMetaTag.Contains(this.faviconPath_32))
                {
                    metaTag = faviconMetaTag.Replace(this.faviconPath_32, this.ReadFromFileStream(this.faviconPath_32));
                }
                else if (faviconMetaTag.Contains(this.faviconPath_16))
                {
                    metaTag = faviconMetaTag.Replace(this.faviconPath_16, this.ReadFromFileStream(this.faviconPath_16));
                }
                metaTags.Add(metaTag);
            }
            return metaTags;
        }

        /// <inheritdoc/>
        public virtual async Task<string> GetStylesheetAsync()
        {
            if (Uri.TryCreate(this.CustomStylesheetPath, UriKind.Absolute, out var stylesheetUri))
            {
                return await this.ReadFromUri(stylesheetUri).ConfigureAwait(false);
            }

            return await this.ReadFromStream(this.CustomStylesheetPath).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public virtual async Task<string> GetJavaScriptAsync()
        {
            if (Uri.TryCreate(this.CustomJavaScriptPath, UriKind.Absolute, out var scriptUri))
            {
                return await this.ReadFromUri(scriptUri).ConfigureAwait(false);
            }

            return await this.ReadFromStream(this.CustomJavaScriptPath).ConfigureAwait(false);
        }

        private string ReadFromFileStream(string path)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{typeof(SwaggerUI).Namespace}.{path}");
            using (var memoryStream = new MemoryStream())
            {
                if (stream.IsNullOrDefault())
                {
                    return path;
                }
                stream.CopyToAsync(memoryStream);
                path = string.Format(@"data:image/png;base64, {0}", Convert.ToBase64String(memoryStream.ToArray()));
            }
            return path;
        }

        private async Task<string> ReadFromStream(string path)
        {        
            using (var stream = this._assembly.GetManifestResourceStream($"{this._assembly.GetName().Name}.{path}"))
            {
                if (stream.IsNullOrDefault())
                {
                    return string.Empty;
                }

                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync().ConfigureAwait(false);
                }
            }
        }

        private async Task<string> ReadFromUri(Uri uri)
        {
            var response = await http.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            return string.Empty;
        }
    }
}
