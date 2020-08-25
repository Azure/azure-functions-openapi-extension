using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums
{
    /// <summary>
    /// This specifies the version of Open API.
    /// </summary>
    public enum OpenApiVersionType
    {
        /// <summary>
        /// Identifies the Open API version 2.
        /// </summary>
        [Display("v2")]
        V2 = 0,

        /// <summary>
        /// Identifies the Open API version 3.
        /// </summary>
        [Display("v3")]
        V3 = 1
    }
}
