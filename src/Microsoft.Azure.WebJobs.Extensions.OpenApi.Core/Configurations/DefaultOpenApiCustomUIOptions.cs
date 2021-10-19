using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
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

        private static IEnumerable<string> _faviconPaths = new List<string>()
        {
            "dist.favicon-32x32.png",
            "dist.favicon-16x16.png"
        };

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
            (this.CustomFaviconMetaTags as List<string>).ForEach(async x =>
                metaTags.Add(await this.ResolveFaviconMetaTagAsync(x).ConfigureAwait(false))
            );
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

        /// <inheritdoc/>
        public void GetFaviconPaths(IEnumerable<string> metatags, string hrefPattern)
        {
            var href = new List<string>();
            (metatags as List<string>).ForEach(x => href.Add(Regex.Match(x, hrefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromSeconds(1)).Groups[1].Value));
            _faviconPaths = href;
        }

        private async Task<string> ResolveFaviconMetaTagAsync(string metatag)
        {
            var faviconPath = _faviconPaths.SingleOrDefault(p => metatag.Contains(p));
            if (Uri.TryCreate(faviconPath, UriKind.Absolute, out var faviconUri))
            {
                return metatag;
            }

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{typeof(SwaggerUI).Namespace}.{faviconPath}") ?? this._assembly.GetManifestResourceStream($"{this._assembly.GetName().Name}.{faviconPath}"))
            using (var memoryStream = new MemoryStream())
            {
                if (stream.IsNullOrDefault())
                {
                    return metatag;
                }
                await stream.CopyToAsync(memoryStream).ConfigureAwait(false);
                metatag = metatag.Replace(faviconPath, string.Format("data:image/{0};base64,{1}", (Path.GetExtension(faviconPath)?? "png").Replace(".",""), Convert.ToBase64String(memoryStream.ToArray())));
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
