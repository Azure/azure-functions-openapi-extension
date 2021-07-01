namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums
{
    /// <summary>
    /// This specifies the authorisation level of OpenAPI document endpoint.
    /// </summary>
    public enum OpenApiAuthLevelType
    {
        /// <summary>
        /// Identifies "anonymous" access.
        /// </summary>
        Anonymous = 0,

        /// <summary>
        /// Identifies "user" access.
        /// </summary>
        User = 1,

        /// <summary>
        /// Identifies "function" access.
        /// </summary>
        Function = 2,

        /// <summary>
        /// Identifies "system" access.
        /// </summary>
        System = 3,

        /// <summary>
        /// Identifies "admin" access.
        /// </summary>
        Admin = 4
    }
}
