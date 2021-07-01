using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Functions
{
    /// <summary>
    /// This represents the entity of the OpenAPI related HTTP trigger functions.
    /// </summary>
    public class OpenApiTriggerFunction : IOpenApiTriggerFunction
    {
        private const string ContentTypeText = "text/plain";
        private const string ContentTypeHtml = "text/html";
        private const string ContentTypeJson = "application/json";
        private const string ContentTypeYaml = "text/vnd.yaml";

        private readonly IOpenApiHttpTriggerContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerFunction"/> class.
        /// </summary>
        /// <param name="context"><see cref="IOpenApiHttpTriggerContext"/> instance.</param>
        public OpenApiTriggerFunction(IOpenApiHttpTriggerContext context)
        {
            this._context = context.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        public async Task<HttpResponseData> RenderSwaggerDocument(HttpRequestData req, string extension, FunctionContext ctx)
        {
            var log = ctx.GetLogger(nameof(OpenApiTriggerFunction));
            log.LogInformation($"swagger.{extension} was requested.");

            var fi = new FileInfo(ctx.FunctionDefinition.PathToAssembly);
            var result = default(string);
            var response = default(HttpResponseData);
            try
            {
                result = await (await this._context.SetApplicationAssemblyAsync(fi.Directory.FullName, appendBin: false))
                                          .Document
                                          .InitialiseDocument()
                                          .AddMetadata(this._context.OpenApiConfigurationOptions.Info)
                                          .AddServer(new HttpRequestObject(req), this._context.HttpSettings.RoutePrefix, this._context.OpenApiConfigurationOptions)
                                          .AddNamingStrategy(this._context.NamingStrategy)
                                          .AddVisitors(this._context.GetVisitorCollection())
                                          .Build(this._context.ApplicationAssembly, this._context.OpenApiConfigurationOptions.OpenApiVersion)
                                          .RenderAsync(this._context.GetOpenApiSpecVersion(this._context.OpenApiConfigurationOptions.OpenApiVersion), this._context.GetOpenApiFormat(extension))
                                          .ConfigureAwait(false);

                response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", this._context.GetOpenApiFormat(extension).GetContentType());
                await response.WriteStringAsync(result).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);

                result = ex.Message;
                if (this._context.IsDevelopment)
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

        /// <inheritdoc/>
        public async Task<HttpResponseData> RenderOpenApiDocument(HttpRequestData req, string version, string extension, FunctionContext ctx)
        {
            var log = ctx.GetLogger(nameof(OpenApiTriggerFunction));
            log.LogInformation($"{version}.{extension} was requested.");

            var fi = new FileInfo(ctx.FunctionDefinition.PathToAssembly);
            var result = default(string);
            var response = default(HttpResponseData);
            try
            {
                result = await (await this._context.SetApplicationAssemblyAsync(fi.Directory.FullName, appendBin: false))
                                          .Document
                                          .InitialiseDocument()
                                          .AddMetadata(this._context.OpenApiConfigurationOptions.Info)
                                          .AddServer(new HttpRequestObject(req), this._context.HttpSettings.RoutePrefix, this._context.OpenApiConfigurationOptions)
                                          .AddNamingStrategy(this._context.NamingStrategy)
                                          .AddVisitors(this._context.GetVisitorCollection())
                                          .Build(this._context.ApplicationAssembly, this._context.GetOpenApiVersionType(version))
                                          .RenderAsync(this._context.GetOpenApiSpecVersion(version), this._context.GetOpenApiFormat(extension))
                                          .ConfigureAwait(false);

                response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", this._context.GetOpenApiFormat(extension).GetContentType());
                await response.WriteStringAsync(result).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);

                result = ex.Message;
                if (this._context.IsDevelopment)
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

        /// <inheritdoc/>
        public async Task<HttpResponseData> RenderSwaggerUI(HttpRequestData req, FunctionContext ctx)
        {
            var log = ctx.GetLogger(nameof(OpenApiTriggerFunction));
            log.LogInformation("SwaggerUI page was requested.");

            var fi = new FileInfo(ctx.FunctionDefinition.PathToAssembly);
            var result = default(string);
            var response = default(HttpResponseData);
            try
            {
                result = await (await this._context.SetApplicationAssemblyAsync(fi.Directory.FullName, appendBin: false))
                                          .SwaggerUI
                                          .AddMetadata(this._context.OpenApiConfigurationOptions.Info)
                                          .AddServer(new HttpRequestObject(req), this._context.HttpSettings.RoutePrefix, this._context.OpenApiConfigurationOptions)
                                          .BuildAsync(this._context.PackageAssembly, this._context.OpenApiCustomUIOptions)
                                          .RenderAsync("swagger.json", this._context.GetDocumentAuthLevel(), this._context.GetSwaggerAuthKey())
                                          .ConfigureAwait(false);

                response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", ContentTypeHtml);
                await response.WriteStringAsync(result).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);

                result = ex.Message;
                if (this._context.IsDevelopment)
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

        /// <inheritdoc/>
        public async Task<HttpResponseData> RenderOAuth2Redirect(HttpRequestData req, FunctionContext ctx)
        {
            var log = ctx.GetLogger(nameof(OpenApiTriggerFunction));
            log.LogInformation("The oauth2-redirect.html page was requested.");

            var fi = new FileInfo(ctx.FunctionDefinition.PathToAssembly);
            var result = default(string);
            var response = default(HttpResponseData);
            try
            {
                result = await (await this._context.SetApplicationAssemblyAsync(fi.Directory.FullName, appendBin: false))
                                          .SwaggerUI
                                          .AddServer(new HttpRequestObject(req), this._context.HttpSettings.RoutePrefix, this._context.OpenApiConfigurationOptions)
                                          .BuildOAuth2RedirectAsync(this._context.PackageAssembly)
                                          .RenderOAuth2RedirectAsync("oauth2-redirect.html", this._context.GetDocumentAuthLevel(), this._context.GetSwaggerAuthKey())
                                          .ConfigureAwait(false);

                response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", ContentTypeHtml);
                await response.WriteStringAsync(result).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);

                result = ex.Message;
                if (this._context.IsDevelopment)
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
