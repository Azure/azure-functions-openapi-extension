using System.Runtime.Serialization;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models
{
    /// <summary>
    /// This specifies an enum that will be serialized as a string.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StringEnum
    {
        /// <summary>
        /// Identifies "zero".
        /// </summary>
        Zero = 0,

        /// <summary>
        /// Identifies "one".
        /// </summary>
        One = 1,

        /// <summary>
        /// Identifies "yi".
        /// </summary>
        [EnumMember(Value = "dul")]
        Two = 2,

        /// <summary>
        /// Identifies "yi".
        /// </summary>
        [Display("sam")]
        Three = 3,

        /// <summary>
        /// Identifies "yi".
        /// </summary>
        [Display("sa")]
        [EnumMember(Value = "net")]
        Four = 4,
    }
}
