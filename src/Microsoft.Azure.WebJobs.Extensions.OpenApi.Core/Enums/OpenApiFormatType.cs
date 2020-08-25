using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums
{
    /// <summary>
    /// This specifies the Open API format.
    /// </summary>
    public enum OpenApiFormatType
    {
        /// <summary>
        /// Identifies the JSON format.
        /// </summary>
        [Display("json")]
        Json = 0,

        /// <summary>
        /// Identifies the YAML format.
        /// </summary>
        [Display("yaml")]
        Yaml = 1
    }
}
