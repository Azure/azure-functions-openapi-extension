using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Attributes
{
    /// <summary>
    /// This represents the extension attribute that registers the <see cref="OpenApiHttpTriggerContextAttribute"/> to inject the <see cref="OpenApiHttpTriggerContext"/> instance.
    /// </summary>
    [Extension(nameof(OpenApiHttpTriggerContextBinding))]
    public class OpenApiHttpTriggerContextBinding : IExtensionConfigProvider
    {
        /// <inheritdoc/>
        public void Initialize(ExtensionConfigContext context)
        {
            var rule = context.AddBindingRule<OpenApiHttpTriggerContextAttribute>();
            rule.BindToInput((attr, vbContext) =>
            {
                var httpContext = vbContext.FunctionContext.CreateObjectInstance<OpenApiHttpTriggerContext>();

                return Task.FromResult(httpContext);
            });
        }
    }
}
