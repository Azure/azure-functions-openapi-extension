using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums
{
    /// <summary>
    /// This specifies the parameter collection delimiter.
    /// </summary>
    public enum OpenApiParameterCollectionDelimiterType
    {
        /// <summary>
        /// Identifies "comma".
        /// </summary>
        [Display("comma")]
        Comma = 0,

        /// <summary>
        /// Identifies "space".
        /// </summary>
        [Display("space")]
        Space = 1,

        /// <summary>
        /// Identifies "pipe".
        /// </summary>
        [Display("pipe")]
        Pipe = 2
    }
}
