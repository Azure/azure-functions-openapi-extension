using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums
{
    /// <summary>
    /// This specifies the version of OpenAPI.
    /// </summary>
    public enum OpenApiVersionType
    {
        /// <summary>
        /// Identifies the OpenAPI version 2.
        /// </summary>
        [Display("v2")]
        V2 = 0,

        /// <summary>
        /// Identifies the OpenAPI version 3.
        /// </summary>
        [Display("v3")]
        V3 = 1
    }
}
