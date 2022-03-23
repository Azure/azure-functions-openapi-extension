using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi
{
    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP triggers.
    /// </summary>
    public partial class OpenApiTriggerFunctionProvider
    {
        private const string ContentTypeText = "text/plain";
        private const string ContentTypeHtml = "text/html";
        private const string ContentTypeJson = "application/json";
        private const string ContentTypeYaml = "text/vnd.yaml";

        private static readonly IOpenApiHttpTriggerContext context = new OpenApiHttpTriggerContext();

        /// <summary>
        /// Invokes the HTTP trigger endpoint to get OpenAPI document.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="extension">File extension representing the document format. This MUST be either "json" or "yaml".</param>
        /// <param name="ctx"><see cref="ExecutionContext"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>OpenAPI document in a format of either JSON or YAML.</returns>
        [OpenApiIgnore]
        public static async Task<IActionResult> RenderSwaggerDocument(HttpRequest req, string extension, ExecutionContext ctx, ILogger log)
        {
            log.LogInformation($"swagger.{extension} was requested.");

            var request = new HttpRequestObject(req);
            var result = default(string);
            var content = default(ContentResult);
            try
            {
                var auth = await context.SetApplicationAssemblyAsync(ctx.FunctionAppDirectory)
                                        .AuthorizeAsync(request)
                                        .ConfigureAwait(false);
                if (!auth.IsNullOrDefault())
                {
                    content = new ContentResult()
                    {
                        Content = auth.Payload,
                        ContentType = auth.ContentType,
                        StatusCode = (int)auth.StatusCode,
                    };

                    return content;
                }

                result = await context.Document
                                      .InitialiseDocument()
                                      .AddMetadata(context.OpenApiConfigurationOptions.Info)
                                      .AddServer(request, context.HttpSettings.RoutePrefix, context.OpenApiConfigurationOptions)
                                      .AddNamingStrategy(context.NamingStrategy)
                                      .AddVisitors(context.GetVisitorCollection())
                                      .Build(context.ApplicationAssembly, context.OpenApiConfigurationOptions.OpenApiVersion)
                                      .ApplyDocumentFilters(context.GetDocumentFilterCollection())
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
                    ContentType = ContentTypeText,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };
            }

            return content;
        }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to get OpenAPI document.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="version">OpenAPI document spec version. This MUST be either "v2" or "v3".</param>
        /// <param name="extension">File extension representing the document format. This MUST be either "json" or "yaml".</param>
        /// <param name="ctx"><see cref="ExecutionContext"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>OpenAPI document in a format of either JSON or YAML.</returns>
        [OpenApiIgnore]
        public static async Task<IActionResult> RenderOpenApiDocument(HttpRequest req, string version, string extension, ExecutionContext ctx, ILogger log)
        {
            log.LogInformation($"{version}.{extension} was requested.");

            var request = new HttpRequestObject(req);
            var result = default(string);
            var content = default(ContentResult);
            try
            {
                var auth = await context.SetApplicationAssemblyAsync(ctx.FunctionAppDirectory)
                                        .AuthorizeAsync(request)
                                        .ConfigureAwait(false);
                if (!auth.IsNullOrDefault())
                {
                    content = new ContentResult()
                    {
                        Content = auth.Payload,
                        ContentType = auth.ContentType,
                        StatusCode = (int)auth.StatusCode,
                    };

                    return content;
                }

                result = await context.Document
                                      .InitialiseDocument()
                                      .AddMetadata(context.OpenApiConfigurationOptions.Info)
                                      .AddServer(new HttpRequestObject(req), context.HttpSettings.RoutePrefix, context.OpenApiConfigurationOptions)
                                      .AddNamingStrategy(context.NamingStrategy)
                                      .AddVisitors(context.GetVisitorCollection())
                                      .Build(context.ApplicationAssembly, context.GetOpenApiVersionType(version))
                                      .ApplyDocumentFilters(context.GetDocumentFilterCollection())
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
                    ContentType = ContentTypeText,
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

            var request = new HttpRequestObject(req);
            var result = default(string);
            var content = default(ContentResult);
            try
            {
                var auth = await context.SetApplicationAssemblyAsync(ctx.FunctionAppDirectory)
                                        .AuthorizeAsync(request)
                                        .ConfigureAwait(false);
                if (!auth.IsNullOrDefault())
                {
                    content = new ContentResult()
                    {
                        Content = auth.Payload,
                        ContentType = auth.ContentType,
                        StatusCode = (int)auth.StatusCode,
                    };

                    return content;
                }

                result = await context.SwaggerUI
                                      .AddMetadata(context.OpenApiConfigurationOptions.Info)
                                      .AddServer(new HttpRequestObject(req), context.HttpSettings.RoutePrefix, context.OpenApiConfigurationOptions)
                                      .BuildAsync(context.PackageAssembly, context.OpenApiCustomUIOptions)
                                      .RenderAsync("swagger.json", context.GetDocumentAuthLevel(), context.GetSwaggerAuthKey())
                                      .ConfigureAwait(false);

                content = new ContentResult()
                {
                    Content = result,
                    ContentType = ContentTypeHtml,
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
                    ContentType = ContentTypeText,
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

            var request = new HttpRequestObject(req);
            var result = default(string);
            var content = default(ContentResult);
            try
            {
                await context.SetApplicationAssemblyAsync(ctx.FunctionAppDirectory)
                             .ConfigureAwait(false);

                result = await context.SwaggerUI
                                      .AddServer(request, context.HttpSettings.RoutePrefix, context.OpenApiConfigurationOptions)
                                      .BuildOAuth2RedirectAsync(context.PackageAssembly)
                                      .RenderOAuth2RedirectAsync("oauth2-redirect.html", context.GetDocumentAuthLevel(), context.GetSwaggerAuthKey())
                                      .ConfigureAwait(false);

                content = new ContentResult()
                {
                    Content = result,
                    ContentType = ContentTypeHtml,
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
                    ContentType = ContentTypeText,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };
            }

            return content;
        }
    }
}
