using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core
{
    /// <summary>
    /// This represents the entity to render UI for OpenAPI.
    /// </summary>
    public class SwaggerUI : ISwaggerUI
    {
        private const string SwaggerUITitlePlaceholder = "[[SWAGGER_UI_TITLE]]";
        private const string SwaggerUICssPlaceholder = "[[SWAGGER_UI_CSS]]";
        private const string SwaggerUICustomCssPlaceholder = "[[SWAGGER_UI_CUSTOM_CSS]]";
        private const string SwaggerUIBundleJsPlaceholder = "[[SWAGGER_UI_BUNDLE_JS]]";
        private const string SwaggerUICustomJsPlaceholder = "[[SWAGGER_UI_CUSTOM_JS]]";
        private const string SwaggerUIStandalonePresetJsPlaceholder = "[[SWAGGER_UI_STANDALONE_PRESET_JS]]";
        private const string SwaggerUIApiPrefix = "[[SWAGGER_UI_API_PREFIX]]";
        private const string SwaggerUrlPlaceholder = "[[SWAGGER_URL]]";

        private readonly string indexHtml = $"{typeof(SwaggerUI).Namespace}.dist.index.html";
        private readonly string oauth2RedirectHtml = $"{typeof(SwaggerUI).Namespace}.dist.oauth2-redirect.html";
        private readonly string swaggerUiCss = $"{typeof(SwaggerUI).Namespace}.dist.swagger-ui.css";
        private readonly string swaggerUiBundleJs = $"{typeof(SwaggerUI).Namespace}.dist.swagger-ui-bundle.js";
        private readonly string swaggerUiStandalonePresetJs = $"{typeof(SwaggerUI).Namespace}.dist.swagger-ui-standalone-preset.js";

        private OpenApiInfo _info;
        private IHttpRequestDataObject _req;
        private string _baseUrl;
        private string _swaggerUiCss;
        private string _swaggerUiCustomCss;
        private string _swaggerUiBundleJs;
        private string _swaggerUiCustomJs;
        private string _swaggerUiStandalonePresetJs;
        private string _swaggerUiApiPrefix;
        private string _indexHtml;
        private string _oauth2RedirectHtml;

        /// <inheritdoc />
        public ISwaggerUI AddMetadata(OpenApiInfo info)
        {
            this._info = info;

            return this;
        }

        /// <inheritdoc />
        public ISwaggerUI AddServer(IHttpRequestDataObject req, string routePrefix, IOpenApiConfigurationOptions options = null)
        {
            this._req = req;

            var prefix = string.IsNullOrWhiteSpace(routePrefix) ? string.Empty : $"/{routePrefix}";
            var baseUrl = $"{this._req.GetScheme(options)}://{this._req.Host}{prefix}";
            var absolutePath = default(string);

            if (options.IsNullOrDefault())
            {
                this._baseUrl = baseUrl;

                absolutePath = new Uri(this._baseUrl).AbsolutePath.TrimEnd('/');
                this._swaggerUiApiPrefix = absolutePath;

                return this;
            }

            var server = new OpenApiServer { Url = baseUrl };
            // Filters out the existing base URLs that are the same as the current host URL.
            var servers = options.Servers
                                 .Where(p => p.Url.TrimEnd('/') != baseUrl.TrimEnd('/'))
                                 .ToList();
            if (!servers.Any())
            {
                servers.Insert(0, server);
            }

            if (options.IncludeRequestingHostName
                && !servers.Any(p => p.Url.TrimEnd('/') == baseUrl.TrimEnd('/')))
            {
                servers.Insert(0, server);
            }

            this._baseUrl = servers.First().Url;

            absolutePath = new Uri(this._baseUrl).AbsolutePath.TrimEnd('/');
            this._swaggerUiApiPrefix = absolutePath;

            return this;
        }

        /// <inheritdoc />
        public async Task<ISwaggerUI> BuildAsync(Assembly assembly, IOpenApiCustomUIOptions options = null)
        {
            if (!options.IsNullOrDefault())
            {
                this._swaggerUiCustomCss = await options.GetStylesheetAsync();
                this._swaggerUiCustomJs = await options.GetJavaScriptAsync();
            }

            using (var stream = assembly.GetManifestResourceStream(swaggerUiCss))
            using (var reader = new StreamReader(stream))
            {
                this._swaggerUiCss = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            using (var stream = assembly.GetManifestResourceStream(swaggerUiBundleJs))
            using (var reader = new StreamReader(stream))
            {
                var bundleJs = await reader.ReadToEndAsync().ConfigureAwait(false);
                this._swaggerUiBundleJs = bundleJs.Replace(SwaggerUIApiPrefix, this._swaggerUiApiPrefix);
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
        public async Task<ISwaggerUI> BuildOAuth2RedirectAsync(Assembly assembly)
        {
            using (var stream = assembly.GetManifestResourceStream(oauth2RedirectHtml))
            using (var reader = new StreamReader(stream))
            {
                this._oauth2RedirectHtml = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            return this;
        }

        /// <inheritdoc />
        public async Task<string> RenderAsync(string endpoint, OpenApiAuthLevelType authLevel = OpenApiAuthLevelType.Anonymous, string authKey = null)
        {
            endpoint.ThrowIfNullOrWhiteSpace();

            var html = await Task.Factory
                                 .StartNew(() => this.Render(endpoint, authLevel, authKey))
                                 .ConfigureAwait(false);

            return html;
        }

        /// <inheritdoc />
        public async Task<string> RenderOAuth2RedirectAsync(string endpoint, OpenApiAuthLevelType authLevel = OpenApiAuthLevelType.Anonymous, string authKey = null)
        {
            var html = await Task.Factory
                                 .StartNew(() => this.RenderOAuth2Redirect(endpoint, authLevel, authKey))
                                 .ConfigureAwait(false);

            return html;
        }

        private string Render(string endpoint, OpenApiAuthLevelType authLevel = OpenApiAuthLevelType.Anonymous, string authKey = null)
        {
            var swaggerUiTitle = $"{this._info.Title} - Swagger UI";
            var swaggerUrl = $"{this._baseUrl.TrimEnd('/')}/{endpoint}";

            var queries = this._req.Query.ToDictionary(p => p.Key, p => p.Value);
            if (this.IsAuthKeyRequired(authLevel, authKey))
            {
                if (!queries.ContainsKey("code"))
                {
                    queries.Add("code", new StringValues(authKey));
                }
            }

            if (queries.Any())
            {
                swaggerUrl += "?" + string.Join("&", queries.SelectMany(p => p.Value.Select(q => $"{p.Key}={q}")));
            }

            var html = this._indexHtml.Replace(SwaggerUITitlePlaceholder, swaggerUiTitle)
                                      .Replace(SwaggerUICssPlaceholder, this._swaggerUiCss)
                                      .Replace(SwaggerUICustomCssPlaceholder, this._swaggerUiCustomCss)
                                      .Replace(SwaggerUIBundleJsPlaceholder, this._swaggerUiBundleJs)
                                      .Replace(SwaggerUICustomJsPlaceholder, this._swaggerUiCustomJs)
                                      .Replace(SwaggerUIStandalonePresetJsPlaceholder, this._swaggerUiStandalonePresetJs)
                                      .Replace(SwaggerUrlPlaceholder, swaggerUrl);

            return html;
        }

        /// <inheritdoc />
        private string RenderOAuth2Redirect(string endpoint, OpenApiAuthLevelType authLevel = OpenApiAuthLevelType.Anonymous, string authKey = null)
        {
            var pageUrl = $"{this._baseUrl.TrimEnd('/')}/{endpoint}";
            if (this.IsAuthKeyRequired(authLevel, authKey))
            {
                pageUrl += $"?code={authKey}";
            }

            var html = this._oauth2RedirectHtml;

            return html;
        }

        private bool IsAuthKeyRequired(OpenApiAuthLevelType authLevel = OpenApiAuthLevelType.Anonymous, string authKey = null)
        {
            if (authLevel == OpenApiAuthLevelType.Anonymous)
            {
                return false;
            }

            if (authKey.IsNullOrWhiteSpace())
            {
                throw new InvalidOperationException("API key is required to access OpenAPI document");
            }

            return true;
        }
    }
}
