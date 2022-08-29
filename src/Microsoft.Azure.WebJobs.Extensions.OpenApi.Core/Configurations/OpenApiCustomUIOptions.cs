using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// This represents the options entity for custom UI configuration.
    /// </summary>
    [OpenApiCustomUIOptionsIgnore]
    public class OpenApiCustomUIOptions : IOpenApiCustomUIOptions
    {
        private static readonly HttpClient http = new HttpClient();

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiCustomUIOptions"/> class.
        /// </summary>
        /// <param name="assembly"><see cref="Assembly"/> instance.</param>
        public OpenApiCustomUIOptions(Assembly assembly)
            : this(assembly, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiCustomUIOptions"/> class.
        /// </summary>
        /// <param name="assembly"><see cref="Assembly"/> instance.</param>
        /// <param name="funcCSS">The delegation instance for the logic to get stylesheet.</param>
        /// <param name="funcJS">The delegation instance for the logic to get JavaScript.</param>
        public OpenApiCustomUIOptions(Assembly assembly, Func<Task<string>> funcCSS = null, Func<Task<string>> funcJS = null)
        {
            this.Assembly = assembly.ThrowIfNullOrDefault();
            this.GetStylesheet = funcCSS;
            this.GetJavaScript = funcJS;
        }

        /// <summary>
        /// Gets the <see cref="System.Reflection.Assembly"/> instance.
        /// </summary>
        public Assembly Assembly { get; set; }

        /// <inheritdoc/>
        public virtual string CustomStylesheetPath { get; set; }

        /// <inheritdoc/>
        public virtual string CustomJavaScriptPath { get; set; }

        /// <summary>
        /// Gets or sets the delegation instance for the logic to get the stylesheet.
        /// </summary>
        public Func<Task<string>> GetStylesheet { get; set; }

        /// <summary>
        /// Gets or sets the delegation instance for the custom logic to get the JavaScript.
        /// </summary>
        public Func<Task<string>> GetJavaScript { get; set; }

        /// <inheritdoc/>
        public virtual async Task<string> GetStylesheetAsync()
        {
            if (this.GetStylesheet.IsNullOrDefault())
            {
                this.GetStylesheet = new Func<Task<string>>(async () =>
                {
                    if (Uri.TryCreate(this.CustomStylesheetPath, UriKind.Absolute, out var stylesheetUri))
                    {
                        return await this.ReadFromUriAsync(stylesheetUri).ConfigureAwait(false);
                    }

                    return await this.ReadFromStreamAsync(this.CustomStylesheetPath).ConfigureAwait(false);
                });
            }

            return await this.GetStylesheet.Invoke().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public virtual async Task<string> GetJavaScriptAsync()
        {
            if (this.GetJavaScript.IsNullOrDefault())
            {
                this.GetJavaScript = new Func<Task<string>>(async () =>
                {
                    if (Uri.TryCreate(this.CustomJavaScriptPath, UriKind.Absolute, out var scriptUri))
                    {
                        return await this.ReadFromUriAsync(scriptUri).ConfigureAwait(false);
                    }

                    return await this.ReadFromStreamAsync(this.CustomJavaScriptPath).ConfigureAwait(false);
                });
            }

            return await this.GetJavaScript.Invoke().ConfigureAwait(false);
        }

        private async Task<string> ReadFromStreamAsync(string path)
        {
            using (var stream = this.Assembly.GetManifestResourceStream($"{this.Assembly.GetName().Name}.{path}"))
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

        private async Task<string> ReadFromUriAsync(Uri uri)
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
