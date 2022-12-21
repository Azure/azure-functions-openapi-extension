namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Configurations
{
    /// <summary>
    /// This represents the environment variable settings entity for OpenAPI document auth level.
    /// </summary>
    public class OpenApiAuthLevelSettings
    {
        /// <summary>
        /// Gets or sets the <see cref="AuthorizationLevel"/> value for OpenAPI document rendering endpoints.
        /// </summary>
        public virtual AuthorizationLevel? Document { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="AuthorizationLevel"/> value for Swagger UI page rendering endpoints.
        /// </summary>
        public virtual AuthorizationLevel? UI { get; set; }
    }
}
