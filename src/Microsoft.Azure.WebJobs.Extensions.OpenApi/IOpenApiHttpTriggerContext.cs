using System;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;
using Microsoft.OpenApi;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi
{
    /// <summary>
    /// This provides interfaces to <see cref="OpenApiHttpTriggerContext"/>.
    /// </summary>
    public interface IOpenApiHttpTriggerContext
    {
        /// <summary>
        /// Gets the <see cref="Assembly"/> instance representing the Azure Functions app.
        /// </summary>
        Assembly ApplicationAssembly { get; }

        /// <summary>
        /// Gets the <see cref="Assembly"/> instance representing the Azure Functions OpenAPI Extension package.
        /// </summary>
        Assembly PackageAssembly { get; }

        /// <summary>
        /// Gets the <see cref="IOpenApiConfigurationOptions"/> instance.
        /// </summary>
        IOpenApiConfigurationOptions OpenApiConfigurationOptions { get; }

        /// <summary>
        /// Gets the <see cref="IOpenApiCustomUIOptions"/> instance.
        /// </summary>
        IOpenApiCustomUIOptions OpenApiCustomUIOptions { get; }

        /// <summary>
        /// Gets the <see cref="HttpSettings"/> instance.
        /// </summary>
        HttpSettings HttpSettings { get; }

        /// <summary>
        /// Gets the <see cref="IDocument"/> instance.
        /// </summary>
        IDocument Document { get; }

        /// <summary>
        /// Gets the <see cref="ISwaggerUI"/> instance.
        /// </summary>
        ISwaggerUI SwaggerUI { get; }

        /// <summary>
        /// Gets the <see cref="NamingStrategy"/> instance.
        /// </summary>
        NamingStrategy NamingStrategy { get; }

        /// <summary>
        /// Gets the executing assembly.
        /// </summary>
        /// <returns>Returns the executing assembly.</returns>
        [Obsolete("This method is obsolete.", error: true)]
        Assembly GetExecutingAssembly();

        /// <summary>
        /// Sets the application assembly from the function app directory.
        /// </summary>
        /// <param name="functionAppDirectory">Function app directory.</param>
        /// <param name="appendBin">Value indicating whether to append the "bin" directory or not.</param>
        IOpenApiHttpTriggerContext SetApplicationAssembly(string functionAppDirectory, bool appendBin = true);

        /// <summary>
        /// Gets the <see cref="VisitorCollection"/> instance.
        /// </summary>
        /// <returns>Returns the <see cref="VisitorCollection"/> instance.</returns>
        VisitorCollection GetVisitorCollection();

        /// <summary>
        /// Gets the <see cref="OpenApiSpecVersion"/> value.
        /// </summary>
        /// <param name="version">OpenAPI spec version. It can be either <c>v2</c> or <c>v3</c>.</param>
        /// <returns>Returns the <see cref="OpenApiSpecVersion"/> value.</returns>
        OpenApiSpecVersion GetOpenApiSpecVersion(string version = "v2");

        /// <summary>
        /// Gets the <see cref="OpenApiSpecVersion"/> value.
        /// </summary>
        /// <param name="version"><see cref="OpenApiVersionType"/> value.</param>
        /// <returns>Returns the <see cref="OpenApiSpecVersion"/> value.</returns>
        OpenApiSpecVersion GetOpenApiSpecVersion(OpenApiVersionType version = OpenApiVersionType.V2);

        /// <summary>
        /// Gets the <see cref="OpenApiFormat"/> value.
        /// </summary>
        /// <param name="format">OpenAPI document format. It can be either <c>json</c> or <c>yaml</c>.</param>
        /// <returns>Returns the <see cref="OpenApiFormat"/> value.</returns>
        OpenApiFormat GetOpenApiFormat(string format = "json");

        /// <summary>
        /// Gets the <see cref="OpenApiFormat"/> value.
        /// </summary>
        /// <param name="format"><see cref="OpenApiFormatType"/> value.</param>
        /// <returns>Returns the <see cref="OpenApiFormat"/> value.</returns>
        OpenApiFormat GetOpenApiFormat(OpenApiFormatType format = OpenApiFormatType.Json);

        /// <summary>
        /// Gets the auth level of the document rendering page endpoint.
        /// </summary>
        /// <param name="key">Environment variables key to look for.</param>
        /// <returns>Returns the auth level of the document rendering page endpoint.</returns>
        AuthorizationLevel GetDocumentAuthLevel(string key = "OpenApi__AuthLevel__Document");

        /// <summary>
        /// Gets the auth level of the UI rendering page endpoint.
        /// </summary>
        /// <param name="key">Environment variables key to look for.</param>
        /// <returns>Returns the auth level of the UI rendering page endpoint.</returns>
        AuthorizationLevel GetUIAuthLevel(string key = "OpenApi__AuthLevel__UI");

        /// <summary>
        /// Gets the API key for endpoints from environment variables.
        /// </summary>
        /// <param name="key">Environment variables key to look for.</param>
        /// <returns>Returns the API key for endpoints.</returns>
        string GetSwaggerAuthKey(string key = "OpenApi__ApiKey");
    }
}
