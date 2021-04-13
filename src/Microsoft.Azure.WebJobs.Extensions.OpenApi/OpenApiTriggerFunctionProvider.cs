using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations;
using Microsoft.Azure.WebJobs.Script.Description;

using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi
{
    /// <summary>
    /// This represents the function provider entity for Open API HTTP triggers.
    /// </summary>
    public partial class OpenApiTriggerFunctionProvider : IFunctionProvider
    {
        private const string RenderSwaggerDocumentKey = "RenderSwaggerDocument";
        private const string RenderOpenApiDocumentKey = "RenderOpenApiDocument";
        private const string RenderSwaggerUIKey = "RenderSwaggerUI";
        private const string RenderOAuth2RedirectKey = "RenderOAuth2Redirect";

        private readonly OpenApiSettings _settings;
        private readonly Dictionary<string, HttpBindingMetadata> _bindings;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerFunctionProvider"/> class.
        /// </summary>
        public OpenApiTriggerFunctionProvider(OpenApiSettings settings)
        {
            this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this._bindings = this.SetupOpenApiHttpBindings();
        }

        /// <inheritdoc />
        public ImmutableDictionary<string, ImmutableArray<string>> FunctionErrors  { get; } = new Dictionary<string, ImmutableArray<string>>().ToImmutableDictionary();

        /// <inheritdoc />
        public async Task<ImmutableArray<FunctionMetadata>> GetFunctionMetadataAsync()
        {
            var functionMetadataList = this.GetFunctionMetadataList();

            return await Task.FromResult(functionMetadataList.ToImmutableArray()).ConfigureAwait(false);
        }

        private Dictionary<string, HttpBindingMetadata> SetupOpenApiHttpBindings()
        {
            var renderSwaggerDocument = new HttpBindingMetadata()
            {
                Methods = { HttpMethods.Get },
                Route = "swagger.{extension}",
                AuthLevel = this._settings.AuthLevel?.Document ?? AuthorizationLevel.Anonymous,
            };

            var renderOpenApiDocument = new HttpBindingMetadata()
            {
                Methods = { HttpMethods.Get },
                Route = "openapi/{version}.{extension}",
                AuthLevel = this._settings.AuthLevel?.Document ?? AuthorizationLevel.Anonymous,
            };

            var renderOAuth2Redirect = new HttpBindingMetadata()
            {
                Methods = { HttpMethods.Get },
                Route = "oauth2-redirect.html",
                AuthLevel = this._settings.AuthLevel?.UI ?? AuthorizationLevel.Anonymous,
            };

            var bindings = new Dictionary<string, HttpBindingMetadata>()
            {
                { RenderSwaggerDocumentKey, renderSwaggerDocument },
                { RenderOpenApiDocumentKey, renderOpenApiDocument },
                { RenderOAuth2RedirectKey, renderOAuth2Redirect },
            };

            if (!this._settings.HideSwaggerUI)
            {
                var renderSwaggerUI = new HttpBindingMetadata()
                {
                    Methods = { HttpMethods.Get },
                    Route = "swagger/ui",
                    AuthLevel = this._settings.AuthLevel?.UI ?? AuthorizationLevel.Anonymous,
                };

                bindings.Add(RenderSwaggerUIKey, renderSwaggerUI);
            }

            return bindings;
        }

        private List<FunctionMetadata> GetFunctionMetadataList()
        {
            var list = new List<FunctionMetadata>()
            {
                this.GetFunctionMetadata(RenderSwaggerDocumentKey),
                this.GetFunctionMetadata(RenderOpenApiDocumentKey),
                this.GetFunctionMetadata(RenderOAuth2RedirectKey),
            };

            if (!this._settings.HideSwaggerUI)
            {
                list.Add(this.GetFunctionMetadata(RenderSwaggerUIKey));
            }

            return list;
        }

        private FunctionMetadata GetFunctionMetadata(string functionName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var functionMetadata = new FunctionMetadata()
            {
                Name = functionName,
                FunctionDirectory = null,
                ScriptFile = $"assembly:{assembly.FullName}",
                EntryPoint = $"{assembly.GetName().Name}.{typeof(OpenApiTriggerFunctionProvider).Name}.{functionName}",
                Language = "DotNetAssembly"
            };

            var jo = JObject.FromObject(this._bindings[functionName]);
            var binding = BindingMetadata.Create(jo);
            functionMetadata.Bindings.Add(binding);

            return functionMetadata;
        }
    }
}
