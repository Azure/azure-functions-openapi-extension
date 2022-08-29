using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// This represents the options entity for custom UI configuration.
    /// </summary>
    [OpenApiCustomUIOptionsIgnore]
    public class DefaultOpenApiCustomUIOptions : OpenApiCustomUIOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultOpenApiCustomUIOptions"/> class.
        /// </summary>
        /// <param name="assembly"><see cref="Assembly"/> instance.</param>
        public DefaultOpenApiCustomUIOptions(Assembly assembly) : base(assembly)
        {
        }

        /// <inheritdoc/>
        public override string CustomStylesheetPath { get; set; } = "dist.custom.css";

        /// <inheritdoc/>
        public override string CustomJavaScriptPath { get; set; } = "dist.custom.js";
    }
}
