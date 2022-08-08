using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// This represents the metadata entity for OpenAPI HTTP Trigger context binding.
    /// </summary>
    public class OpenApiHttpTriggerContextBindingMetadata
    {
        /// <summary>
        /// Gets or sets the name of the binding parameter. Default value is <c>openApiContext</c>.
        /// </summary>
        [JsonRequired]
        [JsonProperty("name")]
        public virtual string Name { get; set; } = "openApiContext";

        /// <summary>
        /// Gets or sets the binding type. Default value is <c>openApiHttpTriggerContext</c>.
        /// </summary>
        [JsonRequired]
        [JsonProperty("type")]
        public virtual string Type { get; set; } = "openApiHttpTriggerContext";

        /// <summary>
        /// Gets or sets the binding direction. Default value is <see cref="BindingDirectionType.In"/>.
        /// </summary>
        [JsonRequired]
        [JsonProperty("direction")]
        public virtual BindingDirectionType Direction { get; set; } = BindingDirectionType.In;
    }
}
