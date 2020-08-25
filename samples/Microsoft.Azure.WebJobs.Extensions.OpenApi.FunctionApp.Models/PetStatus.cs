using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models
{
    /// <summary>
    /// This specifices the pet status.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PetStatus
    {
        /// <summary>
        /// Identifies as "available".
        /// </summary>
        [Display("available")]
        Available = 1,

        /// <summary>
        /// Identifies as "pending".
        /// </summary>
        [Display("pending")]
        Pending = 2,

        /// <summary>
        /// Identifies as "sold".
        /// </summary>
        [Display("sold")]
        Sold = 3
    }
}
