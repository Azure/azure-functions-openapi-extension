using System.IO;

#if NET461
using System.Net.Http;
#endif

using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

#if NETSTANDARD2_0
using Microsoft.AspNetCore.Http;
#endif

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
#if NET461
        /// <inheritdoc />
        public ISwaggerUI AddServer(HttpRequestMessage req, string routePrefix)
        {
            var prefix = string.IsNullOrWhiteSpace(routePrefix) ? string.Empty : $"/{routePrefix}";
            var baseUrl = $"{req.RequestUri.Scheme}://{req.RequestUri.Authority}{prefix}";
            this._baseUrl = baseUrl;

            return this;
        }
#elif NETSTANDARD2_0
        /// <inheritdoc />
        public ISwaggerUI AddServer(HttpRequest req, string routePrefix)
        {
            var prefix = string.IsNullOrWhiteSpace(routePrefix) ? string.Empty : $"/{routePrefix}";
            var baseUrl = $"{req.Scheme}://{req.Host}{prefix}";
            this._baseUrl = baseUrl;

            return this;
        }
#endif
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
            var swaggerUrl = $"{this._baseUrl}/{endpoint}";
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
