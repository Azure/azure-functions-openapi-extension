using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums
{
    /// <summary>
    /// This specifices the Open API security scheme value.
    /// </summary>
    public enum OpenApiSecuritySchemeType
    {
        /// <summary>
        /// Identifies no scheme.
        /// </summary>
        None = 0,

        /// <summary>
        /// Identifies the basic scheme.
        /// </summary>
        [Display("basic")]
        Basic = 1,

        /// <summary>
        /// Identifies the bearer scheme.
        /// </summary>
        [Display("bearer")]
        Bearer = 2
    }
}
