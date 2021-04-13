namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations
{
    /// <summary>
    /// This represents the environment variable settings entity for OpenAPI document.
    /// </summary>
    public class OpenApiSettings
    {
        /// <summary>
        /// Gets or sets the value indicating whether to hide the Swagger UI page or not.
        /// </summary>
        public virtual bool HideSwaggerUI { get; set; }

        /// <summary>
        /// Gets or sets the API key to access to OpenAPI document.
        /// </summary>
        public virtual string AuthKey { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="OpenApiAuthLevelSettings"/> object.
        /// </summary>
        public virtual OpenApiAuthLevelSettings AuthLevel { get; set; }

        /// <summary>
        /// Gets or sets the backend URL for Azure Functions Proxy.
        /// </summary>
        public virtual string BackendProxyUrl { get; set; }
    }
}
