using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Script.Description;

using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi
{
    /// <summary>
    /// This represents the metadata entity for HTTP trigger binding.
    /// </summary>
    public class HttpBindingMetadata
    {
        /// <summary>
        /// Gets or sets the name of the binding parameter. Default value is <c>req</c>.
        /// </summary>
        [JsonRequired]
        [JsonProperty("name")]
        public virtual string Name { get; set; } = "req";

        /// <summary>
        /// Gets or sets the binding type. Default value is <c>httpTrigger</c>.
        /// </summary>
        [JsonRequired]
        [JsonProperty("type")]
        public virtual string Type { get; set; } = "httpTrigger";

        /// <summary>
        /// Gets or sets the binding direction. Default value is <see cref="BindingDirection.In"/>.
        /// </summary>
        [JsonRequired]
        [JsonProperty("direction")]
        public virtual BindingDirection Direction { get; set; } = BindingDirection.In;

        /// <summary>
        /// Gets or sets the binding data type.
        /// </summary>
        [JsonProperty("dataType", NullValueHandling = NullValueHandling.Ignore)]
        public virtual DataType? DataType { get; set; }

        /// <summary>
        /// Gets or sets the HTTP endpoint route template.
        /// </summary>
        [JsonProperty("route", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Route { get; set; }

        /// <summary>
        /// Gets or sets the webhook type, handled by the trigger.
        /// </summary>
        [JsonProperty("webHookType", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string WebHookType { get; set; }

        /// <summary>
        /// Gets or sets the HTTP endpoint authorisation level. Default value is <cref see="AuthorizationLevel.Function"/>.
        /// </summary>
        [JsonRequired]
        [JsonProperty("authLevel")]
        public virtual AuthorizationLevel AuthLevel { get; set; } = AuthorizationLevel.Function;

        /// <summary>
        /// Gets or sets the list of the HTTP verbs. Default values are <c>GET</c> and <c>POST</c>.
        /// </summary>
        [JsonRequired]
        [JsonProperty("methods")]
        public virtual List<string> Methods { get; set; } = new List<string>() { "GET", "POST" };
    }
}
