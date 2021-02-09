using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core
{
    /// <summary>
    /// This represents the entity to render UI for Open API.
    /// </summary>
    public class SwaggerUI : ISwaggerUI
    {
        private const string SwaggerUITitlePlaceholder = "[[SWAGGER_UI_TITLE]]";
        private const string SwaggerUICssPlaceholder = "[[SWAGGER_UI_CSS]]";
        private const string SwaggerUIBundleJsPlaceholder = "[[SWAGGER_UI_BUNDLE_JS]]";
        private const string SwaggerUIStandalonePresetJsPlaceholder = "[[SWAGGER_UI_STANDALONE_PRESET_JS]]";
        private const string SwaggerUrlPlaceholder = "[[SWAGGER_URL]]";

        private readonly string indexHtml = $"{typeof(SwaggerUI).Namespace}.dist.index.html";
        private readonly string swaggerUiCss = $"{typeof(SwaggerUI).Namespace}.dist.swagger-ui.css";
        private readonly string swaggerUiBundleJs = $"{typeof(SwaggerUI).Namespace}.dist.swagger-ui-bundle.js";
        private readonly string swaggerUiStandalonePresetJs = $"{typeof(SwaggerUI).Namespace}.dist.swagger-ui-standalone-preset.js";

        private OpenApiInfo _info;
        private string _baseUrl;
        private string _swaggerUiCss;
        private string _swaggerUiBundleJs;
        private string _swaggerUiStandalonePresetJs;
        private string _indexHtml;

        /// <inheritdoc />
        public ISwaggerUI AddMetadata(OpenApiInfo info)
        {
            this._info = info;

            return this;
        }

        /// <inheritdoc />
        public ISwaggerUI AddServer(HttpRequest req, string routePrefix, IOpenApiConfigurationOptions options = null)
        {
            var prefix = string.IsNullOrWhiteSpace(routePrefix) ? string.Empty : $"/{routePrefix}";
            var baseUrl = $"{req.Scheme}://{req.Host}{prefix}";

            if (options.IsNullOrDefault())
            {
                this._baseUrl = baseUrl;

                return this;
            }

            var server = new OpenApiServer { Url = baseUrl };
            // Filters out the existing base URLs that are the same as the current host URL.
            var servers = options.Servers
                                 .Where(p => p.Url.TrimEnd('/') != baseUrl.TrimEnd('/'))
                                 .ToList();
            servers.Insert(0, server);

            this._baseUrl = servers.First().Url;

            return this;
        }

        /// <inheritdoc />
        public async Task<ISwaggerUI> BuildAsync()
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(swaggerUiCss))
            using (var reader = new StreamReader(stream))
            {
                this._swaggerUiCss = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            using (var stream = assembly.GetManifestResourceStream(swaggerUiBundleJs))
            using (var reader = new StreamReader(stream))
            {
                this._swaggerUiBundleJs = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            using (var stream = assembly.GetManifestResourceStream(swaggerUiStandalonePresetJs))
            using (var reader = new StreamReader(stream))
            {
                this._swaggerUiStandalonePresetJs = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            using (var stream = assembly.GetManifestResourceStream(indexHtml))
            using (var reader = new StreamReader(stream))
            {
                this._indexHtml = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            return this;
        }

        /// <inheritdoc />
        public async Task<string> RenderAsync(string endpoint, string authKey = null)
        {
            endpoint.ThrowIfNullOrWhiteSpace();

            var html = await Task.Factory
                                 .StartNew(() => this.Render(endpoint, authKey))
                                 .ConfigureAwait(false);

            return html;
        }

        private string Render(string endpoint, string authKey = null)
        {
            var swaggerUiTitle = $"{this._info.Title} - Swagger UI";
            var swaggerUrl = $"{this._baseUrl.TrimEnd('/')}/{endpoint}";
            if (!string.IsNullOrWhiteSpace(authKey))
            {
                swaggerUrl += $"?code={authKey}";
            }

            var html = this._indexHtml.Replace(SwaggerUITitlePlaceholder, swaggerUiTitle)
                                      .Replace(SwaggerUICssPlaceholder, this._swaggerUiCss)
                                      .Replace(SwaggerUIBundleJsPlaceholder, this._swaggerUiBundleJs)
                                      .Replace(SwaggerUIStandalonePresetJsPlaceholder, this._swaggerUiStandalonePresetJs)
                                      .Replace(SwaggerUrlPlaceholder, swaggerUrl);

            return html;
        }
    }
}
