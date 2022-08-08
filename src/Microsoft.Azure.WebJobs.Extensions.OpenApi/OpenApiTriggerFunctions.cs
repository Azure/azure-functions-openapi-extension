using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi
{
    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP triggers.
    /// </summary>
    public class OpenApiTriggerFunctions
    {
        private const string ContentTypeText = "text/plain";
        private const string ContentTypeHtml = "text/html";
        private const string ContentTypeJson = "application/json";
        private const string ContentTypeYaml = "text/vnd.yaml";

        /// <summary>
        /// Invokes the HTTP trigger endpoint to get OpenAPI document.
        /// </summary>
        /// <param name="openApiContext"><see cref="OpenApiHttpTriggerContext"/> instance.</param>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="extension">File extension representing the document format. This MUST be either "json" or "yaml".</param>
        /// <param name="ctx"><see cref="ExecutionContext"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>OpenAPI document in a format of either JSON or YAML.</returns>
        [OpenApiIgnore]
        public static async Task<IActionResult> RenderSwaggerDocument(
            [OpenApiHttpTriggerContext] OpenApiHttpTriggerContext openApiContext,
            HttpRequest req, string extension, ExecutionContext ctx, ILogger log)
        {
            log.LogInformation($"swagger.{extension} was requested.");

            var request = new HttpRequestObject(req);
            var result = default(string);
            var content = default(ContentResult);
            try
            {
                var auth = await openApiContext.SetApplicationAssemblyAsync(ctx.FunctionAppDirectory)
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

                result = await openApiContext.Document
                                      .InitialiseDocument()
                                      .AddMetadata(openApiContext.OpenApiConfigurationOptions.Info)
                                      .AddServer(request, openApiContext.HttpSettings.RoutePrefix, openApiContext.OpenApiConfigurationOptions)
                                      .AddNamingStrategy(openApiContext.NamingStrategy)
                                      .AddVisitors(openApiContext.GetVisitorCollection())
                                      .Build(openApiContext.ApplicationAssembly, openApiContext.OpenApiConfigurationOptions.OpenApiVersion)
                                      .ApplyDocumentFilters(openApiContext.GetDocumentFilterCollection())
                                      .RenderAsync(openApiContext.GetOpenApiSpecVersion(openApiContext.OpenApiConfigurationOptions.OpenApiVersion), openApiContext.GetOpenApiFormat(extension))
                                      .ConfigureAwait(false);

                content = new ContentResult()
                {
                    Content = result,
                    ContentType = openApiContext.GetOpenApiFormat(extension).GetContentType(),
                    StatusCode = (int)HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);

                result = ex.Message;
                if (openApiContext.IsDevelopment)
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
        /// <param name="openApiContext"><see cref="OpenApiHttpTriggerContext"/> instance.</param>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="version">OpenAPI document spec version. This MUST be either "v2" or "v3".</param>
        /// <param name="extension">File extension representing the document format. This MUST be either "json" or "yaml".</param>
        /// <param name="ctx"><see cref="ExecutionContext"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>OpenAPI document in a format of either JSON or YAML.</returns>
        [OpenApiIgnore]
        public static async Task<IActionResult> RenderOpenApiDocument(
            [OpenApiHttpTriggerContext] OpenApiHttpTriggerContext openApiContext,
            HttpRequest req, string version, string extension, ExecutionContext ctx, ILogger log)
        {
            log.LogInformation($"{version}.{extension} was requested.");

            var request = new HttpRequestObject(req);
            var result = default(string);
            var content = default(ContentResult);
            try
            {
                var auth = await openApiContext.SetApplicationAssemblyAsync(ctx.FunctionAppDirectory)
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

                result = await openApiContext.Document
                                      .InitialiseDocument()
                                      .AddMetadata(openApiContext.OpenApiConfigurationOptions.Info)
                                      .AddServer(new HttpRequestObject(req), openApiContext.HttpSettings.RoutePrefix, openApiContext.OpenApiConfigurationOptions)
                                      .AddNamingStrategy(openApiContext.NamingStrategy)
                                      .AddVisitors(openApiContext.GetVisitorCollection())
                                      .Build(openApiContext.ApplicationAssembly, openApiContext.GetOpenApiVersionType(version))
                                      .ApplyDocumentFilters(openApiContext.GetDocumentFilterCollection())
                                      .RenderAsync(openApiContext.GetOpenApiSpecVersion(version), openApiContext.GetOpenApiFormat(extension))
                                      .ConfigureAwait(false);

                content = new ContentResult()
                {
                    Content = result,
                    ContentType = openApiContext.GetOpenApiFormat(extension).GetContentType(),
                    StatusCode = (int)HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);

                result = ex.Message;
                if (openApiContext.IsDevelopment)
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
        /// <param name="openApiContext"><see cref="OpenApiHttpTriggerContext"/> instance.</param>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="ctx"><see cref="ExecutionContext"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Swagger UI in HTML.</returns>
        [OpenApiIgnore]
        public static async Task<IActionResult> RenderSwaggerUI(
            [OpenApiHttpTriggerContext] OpenApiHttpTriggerContext openApiContext,
            HttpRequest req, ExecutionContext ctx, ILogger log)
        {
            log.LogInformation("SwaggerUI page was requested.");

            var request = new HttpRequestObject(req);
            var result = default(string);
            var content = default(ContentResult);
            try
            {
                var auth = await openApiContext.SetApplicationAssemblyAsync(ctx.FunctionAppDirectory)
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

                result = await openApiContext.SwaggerUI
                                      .AddMetadata(openApiContext.OpenApiConfigurationOptions.Info)
                                      .AddServer(new HttpRequestObject(req), openApiContext.HttpSettings.RoutePrefix, openApiContext.OpenApiConfigurationOptions)
                                      .BuildAsync(openApiContext.PackageAssembly, openApiContext.OpenApiCustomUIOptions)
                                      .RenderAsync("swagger.json", openApiContext.GetDocumentAuthLevel(), openApiContext.GetSwaggerAuthKey())
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
                if (openApiContext.IsDevelopment)
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
        /// <param name="openApiContext"><see cref="OpenApiHttpTriggerContext"/> instance.</param>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="ctx"><see cref="ExecutionContext"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>oauth2-redirect.html.</returns>
        [OpenApiIgnore]
        public static async Task<IActionResult> RenderOAuth2Redirect(
            [OpenApiHttpTriggerContext] OpenApiHttpTriggerContext openApiContext,
            HttpRequest req, ExecutionContext ctx, ILogger log)
        {
            log.LogInformation("The oauth2-redirect.html page was requested.");

            var request = new HttpRequestObject(req);
            var result = default(string);
            var content = default(ContentResult);
            try
            {
                await openApiContext.SetApplicationAssemblyAsync(ctx.FunctionAppDirectory)
                             .ConfigureAwait(false);

                result = await openApiContext.SwaggerUI
                                      .AddServer(request, openApiContext.HttpSettings.RoutePrefix, openApiContext.OpenApiConfigurationOptions)
                                      .BuildOAuth2RedirectAsync(openApiContext.PackageAssembly)
                                      .RenderOAuth2RedirectAsync("oauth2-redirect.html", openApiContext.GetDocumentAuthLevel(), openApiContext.GetSwaggerAuthKey())
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
                if (openApiContext.IsDevelopment)
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
