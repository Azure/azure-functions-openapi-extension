using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Script.Description;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi
{
    /// <summary>
    /// This represents the function provider entity for Open API HTTP triggers.
    /// </summary>
    public class OpenApiTriggerFunctionProvider : IFunctionProvider
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

        private readonly static IOpenApiHttpTriggerContext context = new OpenApiHttpTriggerContext();

        /// <summary>
        /// Invokes the HTTP trigger endpoint to get Open API document.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="extension">File extension representing the document format. This MUST be either "json" or "yaml".</param>
        /// <param name="ctx"><see cref="ExecutionContext"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Open API document in a format of either JSON or YAML.</returns>
        [OpenApiIgnore]
        public static async Task<IActionResult> RenderSwaggerDocument(HttpRequest req, string extension, ExecutionContext ctx, ILogger log)
        {
            log.LogInformation($"swagger.{extension} was requested.");

            var result = default(string);
            var content = default(ContentResult);
            try
            {
                result = await context.SetApplicationAssembly(ctx.FunctionAppDirectory)
                                      .Document
                                      .InitialiseDocument()
                                      .AddMetadata(context.OpenApiConfigurationOptions.Info)
                                      .AddServer(req, context.HttpSettings.RoutePrefix, context.OpenApiConfigurationOptions)
                                      .AddNamingStrategy(context.NamingStrategy)
                                      .AddVisitors(context.GetVisitorCollection())
                                      .Build(context.ApplicationAssembly)
                                      .RenderAsync(context.GetOpenApiSpecVersion(context.OpenApiConfigurationOptions.OpenApiVersion), context.GetOpenApiFormat(extension))
                                      .ConfigureAwait(false);

                content = new ContentResult()
                {
                    Content = result,
                    ContentType = context.GetOpenApiFormat(extension).GetContentType(),
                    StatusCode = (int)HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);

                result = ex.Message;
                if (context.IsDevelopment)
                {
                    result += "\r\n\r\n";
                    result += ex.StackTrace;
                }
                content = new ContentResult()
                {
                    Content = result,
                    ContentType = "text/plain",
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };
            }

            return content;
        }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to get Open API document.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="version">Open API document spec version. This MUST be either "v2" or "v3".</param>
        /// <param name="extension">File extension representing the document format. This MUST be either "json" or "yaml".</param>
        /// <param name="ctx"><see cref="ExecutionContext"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Open API document in a format of either JSON or YAML.</returns>
        [OpenApiIgnore]
        public static async Task<IActionResult> RenderOpenApiDocument(HttpRequest req, string version, string extension, ExecutionContext ctx, ILogger log)
        {
            log.LogInformation($"{version}.{extension} was requested.");

            var result = default(string);
            var content = default(ContentResult);
            try
            {
                result = await context.SetApplicationAssembly(ctx.FunctionAppDirectory)
                                      .Document
                                      .InitialiseDocument()
                                      .AddMetadata(context.OpenApiConfigurationOptions.Info)
                                      .AddServer(req, context.HttpSettings.RoutePrefix, context.OpenApiConfigurationOptions)
                                      .AddNamingStrategy(context.NamingStrategy)
                                      .AddVisitors(context.GetVisitorCollection())
                                      .Build(context.ApplicationAssembly)
                                      .RenderAsync(context.GetOpenApiSpecVersion(version), context.GetOpenApiFormat(extension))
                                      .ConfigureAwait(false);

                content = new ContentResult()
                {
                    Content = result,
                    ContentType = context.GetOpenApiFormat(extension).GetContentType(),
                    StatusCode = (int)HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);

                result = ex.Message;
                if (context.IsDevelopment)
                {
                    result += "\r\n\r\n";
                    result += ex.StackTrace;
                }
                content = new ContentResult()
                {
                    Content = result,
                    ContentType = "text/plain",
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };
            }

            return content;
        }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to render Swagger UI in HTML.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="ctx"><see cref="ExecutionContext"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Swagger UI in HTML.</returns>
        [OpenApiIgnore]
        public static async Task<IActionResult> RenderSwaggerUI(HttpRequest req, ExecutionContext ctx, ILogger log)
        {
            log.LogInformation("SwaggerUI page was requested.");

            var result = default(string);
            var content = default(ContentResult);
            try
            {
                result = await context.SetApplicationAssembly(ctx.FunctionAppDirectory)
                                      .SwaggerUI
                                      .AddMetadata(context.OpenApiConfigurationOptions.Info)
                                      .AddServer(req, context.HttpSettings.RoutePrefix, context.OpenApiConfigurationOptions)
                                      .BuildAsync(context.PackageAssembly, context.OpenApiCustomUIOptions)
                                      .RenderAsync("swagger.json", context.GetDocumentAuthLevel(), context.GetSwaggerAuthKey())
                                      .ConfigureAwait(false);

                content = new ContentResult()
                {
                    Content = result,
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);

                result = ex.Message;
                if (context.IsDevelopment)
                {
                    result += "\r\n\r\n";
                    result += ex.StackTrace;
                }
                content = new ContentResult()
                {
                    Content = result,
                    ContentType = "text/plain",
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };
            }

            return content;
        }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to render oauth2-redirect.html.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="ctx"><see cref="ExecutionContext"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>oauth2-redirect.html.</returns>
        [OpenApiIgnore]
        public static async Task<IActionResult> RenderOAuth2Redirect(HttpRequest req, ExecutionContext ctx, ILogger log)
        {
            log.LogInformation("The oauth2-redirect.html page was requested.");

            var result = default(string);
            var content = default(ContentResult);
            try
            {
                result = await context.SetApplicationAssembly(ctx.FunctionAppDirectory)
                                      .SwaggerUI
                                      .AddServer(req, context.HttpSettings.RoutePrefix, context.OpenApiConfigurationOptions)
                                      .BuildOAuth2RedirectAsync(context.PackageAssembly)
                                      .RenderOAuth2RedirectAsync("oauth2-redirect.html", context.GetDocumentAuthLevel(), context.GetSwaggerAuthKey())
                                      .ConfigureAwait(false);

                content = new ContentResult()
                {
                    Content = result,
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);

                result = ex.Message;
                if (context.IsDevelopment)
                {
                    result += "\r\n\r\n";
                    result += ex.StackTrace;
                }
                content = new ContentResult()
                {
                    Content = result,
                    ContentType = "text/plain",
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };
            }

            return content;
        }
    }
}
