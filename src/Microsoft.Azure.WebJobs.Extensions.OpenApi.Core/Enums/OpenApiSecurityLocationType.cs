namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums
{
    /// <summary>
    /// This specifies the OpenAPI security key location.
    /// </summary>
    public enum OpenApiSecurityLocationType
    {
        /// <summary>
        /// Identifies no location.
        /// </summary>
        None = 0,

        /// <summary>
        /// Identifies the query as the API key location.
        /// </summary>
        Query = 1,

        /// <summary>
        /// Identifies the header as the API key location.
        /// </summary>
        Header = 2,

        /// <summary>
        /// Identifies the cookie as the API key location.
        /// </summary>
        Cookie = 4
    }
}
