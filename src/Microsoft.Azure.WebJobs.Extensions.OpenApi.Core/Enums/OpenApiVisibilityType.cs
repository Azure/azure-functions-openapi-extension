using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums
{
    /// <summary>
    /// This specifies the visibility of an attribute, which is used for Logic Apps and/or PowerApps.
    /// </summary>
    public enum OpenApiVisibilityType
    {
        /// <summary>
        /// Identifies <c>undefined</c>.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Identifies the field is <c>important</c>. This field is always shown.
        /// </summary>
        [Display("important")]
        Important = 1,

        /// <summary>
        /// Identifies the field is <c>advanced</c>. This field is hidden under the additional menu.
        /// </summary>
        [Display("advanced")]
        Advanced = 2,

        /// <summary>
        /// Identifies the field is <c>internal</c>. This field is always hidden.
        /// </summary>
        [Display("internal")]
        Internal = 3
    }
}
