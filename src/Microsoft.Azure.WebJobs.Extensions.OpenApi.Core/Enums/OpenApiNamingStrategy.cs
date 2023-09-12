namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums
{
    /// <summary>
    /// This specifies the naming strategy for OpenAPI.
    /// </summary>
    public enum OpenApiNamingStrategy
    {
        /// <summary>
        /// Identifies the default naming strategy.
        /// </summary>
        CamelCase = 0,

        /// <summary>
        /// Identifies the PascalCase naming strategy.
        /// </summary>
        PascalCase = 1,

        /// <summary>
        /// Identifies the snake_case naming strategy.
        /// </summary>
        SnakeCase = 2,

        /// <summary>
        /// Identifies the kebab-case naming strategy.
        /// </summary>
        KebabCase = 3
    }
}