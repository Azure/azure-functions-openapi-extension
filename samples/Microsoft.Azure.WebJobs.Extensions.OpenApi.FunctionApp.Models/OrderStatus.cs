using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models
{
    /// <summary>
    /// This specifies the order status.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderStatus
    {
        /// <summary>
        /// Identifies as "placed".
        /// </summary>
        [EnumMember(Value = "placed")]
        Placed = 1,

        /// <summary>
        /// Identifies as "approved".
        /// </summary>
        [EnumMember(Value = "approved")]
        Approved = 2,

        /// <summary>
        /// Identifies as "delivered".
        /// </summary>
        [EnumMember(Value = "delivered")]
        Delivered = 3
    }
}
