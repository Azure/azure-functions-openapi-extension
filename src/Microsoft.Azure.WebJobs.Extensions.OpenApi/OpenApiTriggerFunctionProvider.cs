using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Script.Description;

using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi
{
    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP triggers.
    /// </summary>
    public class OpenApiTriggerFunctionProvider : IFunctionProvider
    {
        private const string RenderSwaggerDocumentKey = nameof(OpenApiTriggerFunctions.RenderSwaggerDocument);
        private const string RenderOpenApiDocumentKey = nameof(OpenApiTriggerFunctions.RenderOpenApiDocument);
        private const string RenderSwaggerUIKey = nameof(OpenApiTriggerFunctions.RenderSwaggerUI);
        private const string RenderOAuth2RedirectKey = nameof(OpenApiTriggerFunctions.RenderOAuth2Redirect);

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
        public ImmutableDictionary<string, ImmutableArray<string>> FunctionErrors { get; } = new Dictionary<string, ImmutableArray<string>>().ToImmutableDictionary();

        /// <inheritdoc />
        public async Task<ImmutableArray<FunctionMetadata>> GetFunctionMetadataAsync()
        {
            var functionMetadataList = this.GetFunctionMetadataList();

            return await Task.FromResult(functionMetadataList.ToImmutableArray()).ConfigureAwait(false);
        }

        private Dictionary<string, HttpBindingMetadata> SetupOpenApiHttpBindings()
        {
            var bindings = new Dictionary<string, HttpBindingMetadata>();

            if (this._settings.HideDocument)
            {
                return bindings;
            }

            var renderSwaggerDocument = new HttpBindingMetadata()
            {
                Methods = new List<string>() { HttpMethods.Get },
                Route = "swagger.{extension}",
                AuthLevel = this._settings.AuthLevel?.Document ?? AuthorizationLevel.Anonymous,
            };

            bindings.Add(RenderSwaggerDocumentKey, renderSwaggerDocument);

            var renderOpenApiDocument = new HttpBindingMetadata()
            {
                Methods = new List<string>() { HttpMethods.Get },
                Route = "openapi/{version}.{extension}",
                AuthLevel = this._settings.AuthLevel?.Document ?? AuthorizationLevel.Anonymous,
            };

            bindings.Add(RenderOpenApiDocumentKey, renderOpenApiDocument);

            if (!this._settings.HideSwaggerUI)
            {
                var renderOAuth2Redirect = new HttpBindingMetadata()
                {
                    Methods = new List<string>() { HttpMethods.Get },
                    Route = "oauth2-redirect.html",
                    AuthLevel = this._settings.AuthLevel?.UI ?? AuthorizationLevel.Anonymous,
                };

                bindings.Add(RenderOAuth2RedirectKey, renderOAuth2Redirect);

                var renderSwaggerUI = new HttpBindingMetadata()
                {
                    Methods = new List<string>() { HttpMethods.Get },
                    Route = "swagger/ui",
                    AuthLevel = this._settings.AuthLevel?.UI ?? AuthorizationLevel.Anonymous,
                };

                bindings.Add(RenderSwaggerUIKey, renderSwaggerUI);
            }

            return bindings;
        }

        private List<FunctionMetadata> GetFunctionMetadataList()
        {
            var list = new List<FunctionMetadata>();

            if (this._settings.HideDocument)
            {
                return list;
            };

            list.AddRange(new[]
            {
                this.GetFunctionMetadata(RenderSwaggerDocumentKey),
                this.GetFunctionMetadata(RenderOpenApiDocumentKey)
            });

            if (!this._settings.HideSwaggerUI)
            {
                list.AddRange(new[]
                {
                    this.GetFunctionMetadata(RenderSwaggerUIKey),
                    this.GetFunctionMetadata(RenderOAuth2RedirectKey)
                });
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
                EntryPoint = $"{assembly.GetName().Name}.{typeof(OpenApiTriggerFunctions).Name}.{functionName}",
                Language = "DotNetAssembly"
            };

            AddBindingMetadata(functionMetadata, this._bindings[functionName]);
            AddBindingMetadata(functionMetadata, new OpenApiHttpTriggerContextBindingMetadata());

            return functionMetadata;
        }

        private static void AddBindingMetadata(FunctionMetadata functionMetadata, object bindingInfo)
        {
            var jsonObject = JObject.FromObject(bindingInfo);
            var bindingMetadata = BindingMetadata.Create(jsonObject);
            functionMetadata.Bindings.Add(bindingMetadata);
        }
    }
}
