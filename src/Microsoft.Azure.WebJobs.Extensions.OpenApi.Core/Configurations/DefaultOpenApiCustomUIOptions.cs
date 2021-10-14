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

        private static readonly List<string> faviconPaths = new List<string>()
        {
            "dist.favicon-16x16.png",
            "dist.favicon-32x32.png",
        };

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
        public virtual async Task<IEnumerable<string>> GetFaviconMetaTagsAsync()
        {
            var metaTags = new List<string>();
            foreach(var faviconMetaTag in this.CustomFaviconMetaTags)
            {
                metaTags.Add(await this.ResolveFaviconMetaTagAsync(faviconMetaTag).ConfigureAwait(false));
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

        private async Task<string> ResolveFaviconMetaTagAsync(string metatag)
        {
            var faviconPath = faviconPaths.SingleOrDefault(p => metatag.Contains(p));
            if (faviconPath.IsNullOrWhiteSpace())
            {
                return metatag;
            }

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{typeof(SwaggerUI).Namespace}.{faviconPath}"))
            {
                var memoryStream = new MemoryStream();
                if (stream.IsNullOrDefault())
                {
                    return metatag;
                }
                await stream.CopyToAsync(memoryStream).ContinueWith((t) =>
                {
                    metatag = metatag.Replace(faviconPath, string.Format(@"data:image/png;base64, {0}", Convert.ToBase64String(memoryStream.ToArray())));
                });
            }
            return metatag;
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
