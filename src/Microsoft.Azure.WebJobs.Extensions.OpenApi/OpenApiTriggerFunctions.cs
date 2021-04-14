using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi
{
    /// <summary>
    /// This represents the function provider entity for Open API HTTP triggers.
    /// </summary>
    public partial class OpenApiTriggerFunctionProvider
    {
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
                                      .AddServer(new HttpRequestObject(req), context.HttpSettings.RoutePrefix, context.OpenApiConfigurationOptions)
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
                                      .AddServer(new HttpRequestObject(req), context.HttpSettings.RoutePrefix, context.OpenApiConfigurationOptions)
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
                                      .AddServer(new HttpRequestObject(req), context.HttpSettings.RoutePrefix, context.OpenApiConfigurationOptions)
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
                                      .AddServer(new HttpRequestObject(req), context.HttpSettings.RoutePrefix, context.OpenApiConfigurationOptions)
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
