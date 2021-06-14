using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi
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

        private readonly static IOpenApiHttpTriggerContext context = new OpenApiHttpTriggerContext();

        /// <summary>
        /// Invokes the HTTP trigger endpoint to get OpenAPI document.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestData"/> instance.</param>
        /// <param name="extension">File extension representing the document format. This MUST be either "json" or "yaml".</param>
        /// <param name="ctx"><see cref="FunctionContext"/> instance.</param>
        /// <returns>OpenAPI document in a format of either JSON or YAML.</returns>
        [OpenApiIgnore]
        public static async Task<HttpResponseData> RenderSwaggerDocument(HttpRequestData req, string extension, FunctionContext ctx)
        {
            var log = ctx.GetLogger(nameof(OpenApiTriggerFunctionProvider));
            log.LogInformation($"swagger.{extension} was requested.");

            var fi = new FileInfo(ctx.FunctionDefinition.PathToAssembly);
            var result = default(string);
            var response = default(HttpResponseData);
            try
            {
                result = await (await context.SetApplicationAssemblyAsync(fi.Directory.FullName))
                                      .Document
                                      .InitialiseDocument()
                                      .AddMetadata(context.OpenApiConfigurationOptions.Info)
                                      .AddServer(new HttpRequestObject(req), context.HttpSettings.RoutePrefix, context.OpenApiConfigurationOptions)
                                      .AddNamingStrategy(context.NamingStrategy)
                                      .AddVisitors(context.GetVisitorCollection())
                                      .Build(context.ApplicationAssembly, context.OpenApiConfigurationOptions.OpenApiVersion)
                                      .RenderAsync(context.GetOpenApiSpecVersion(context.OpenApiConfigurationOptions.OpenApiVersion), context.GetOpenApiFormat(extension))
                                      .ConfigureAwait(false);

                response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", context.GetOpenApiFormat(extension).GetContentType());
                await response.WriteStringAsync(result).ConfigureAwait(false);
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

                response = req.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Content-Type", ContentTypeText);
                await response.WriteStringAsync(result).ConfigureAwait(false);
            }

            return response;
         }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to get OpenAPI document.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestData"/> instance.</param>
        /// <param name="version">OpenAPI document spec version. This MUST be either "v2" or "v3".</param>
        /// <param name="extension">File extension representing the document format. This MUST be either "json" or "yaml".</param>
        /// <param name="ctx"><see cref="FunctionContext"/> instance.</param>
        /// <returns>OpenAPI document in a format of either JSON or YAML.</returns>
        [OpenApiIgnore]
        public static async Task<HttpResponseData> RenderOpenApiDocument(HttpRequestData req, string version, string extension, FunctionContext ctx)
        {
            var log = ctx.GetLogger(nameof(OpenApiTriggerFunctionProvider));
            log.LogInformation($"{version}.{extension} was requested.");

            var fi = new FileInfo(ctx.FunctionDefinition.PathToAssembly);
            var result = default(string);
            var response = default(HttpResponseData);
            try
            {
                result = await (await context.SetApplicationAssemblyAsync(fi.Directory.FullName))
                                      .Document
                                      .InitialiseDocument()
                                      .AddMetadata(context.OpenApiConfigurationOptions.Info)
                                      .AddServer(new HttpRequestObject(req), context.HttpSettings.RoutePrefix, context.OpenApiConfigurationOptions)
                                      .AddNamingStrategy(context.NamingStrategy)
                                      .AddVisitors(context.GetVisitorCollection())
                                      .Build(context.ApplicationAssembly, context.GetOpenApiVersionType(version))
                                      .RenderAsync(context.GetOpenApiSpecVersion(version), context.GetOpenApiFormat(extension))
                                      .ConfigureAwait(false);

                response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", context.GetOpenApiFormat(extension).GetContentType());
                await response.WriteStringAsync(result).ConfigureAwait(false);
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
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Content-Type", ContentTypeText);
                await response.WriteStringAsync(result).ConfigureAwait(false);
            }

            return response;
        }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to render Swagger UI in HTML.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestData"/> instance.</param>
        /// <param name="ctx"><see cref="FunctionContext"/> instance.</param>
        /// <returns>Swagger UI in HTML.</returns>
        [OpenApiIgnore]
        public static async Task<HttpResponseData> RenderSwaggerUI(HttpRequestData req, FunctionContext ctx)
        {
            var log = ctx.GetLogger(nameof(OpenApiTriggerFunctionProvider));
            log.LogInformation("SwaggerUI page was requested.");

            var fi = new FileInfo(ctx.FunctionDefinition.PathToAssembly);
            var result = default(string);
            var response = default(HttpResponseData);
            try
            {
                result = await (await context.SetApplicationAssemblyAsync(fi.Directory.FullName))
                                      .SwaggerUI
                                      .AddMetadata(context.OpenApiConfigurationOptions.Info)
                                      .AddServer(new HttpRequestObject(req), context.HttpSettings.RoutePrefix, context.OpenApiConfigurationOptions)
                                      .BuildAsync(context.PackageAssembly, context.OpenApiCustomUIOptions)
                                      .RenderAsync("swagger.json", context.GetDocumentAuthLevel(), context.GetSwaggerAuthKey())
                                      .ConfigureAwait(false);

                response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", ContentTypeHtml);
                await response.WriteStringAsync(result).ConfigureAwait(false);
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
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Content-Type", ContentTypeText);
                await response.WriteStringAsync(result).ConfigureAwait(false);
            }

            return response;
        }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to render oauth2-redirect.html.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="ctx"><see cref="FunctionContext"/> instance.</param>
        /// <returns>oauth2-redirect.html.</returns>
        [OpenApiIgnore]
        public static async Task<HttpResponseData> RenderOAuth2Redirect(HttpRequestData req, FunctionContext ctx)
        {
            var log = ctx.GetLogger(nameof(OpenApiTriggerFunctionProvider));
            log.LogInformation("The oauth2-redirect.html page was requested.");

            var fi = new FileInfo(ctx.FunctionDefinition.PathToAssembly);
            var result = default(string);
            var response = default(HttpResponseData);
            try
            {
                result = await (await context.SetApplicationAssemblyAsync(fi.Directory.FullName))
                                      .SwaggerUI
                                      .AddServer(new HttpRequestObject(req), context.HttpSettings.RoutePrefix, context.OpenApiConfigurationOptions)
                                      .BuildOAuth2RedirectAsync(context.PackageAssembly)
                                      .RenderOAuth2RedirectAsync("oauth2-redirect.html", context.GetDocumentAuthLevel(), context.GetSwaggerAuthKey())
                                      .ConfigureAwait(false);

                response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", ContentTypeHtml);
                await response.WriteStringAsync(result).ConfigureAwait(false);
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
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Content-Type", ContentTypeText);
                await response.WriteStringAsync(result).ConfigureAwait(false);
            }

            return response;
        }
    }
}
