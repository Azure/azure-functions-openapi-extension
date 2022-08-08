using System;

using Microsoft.Azure.WebJobs.Description;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Attributes
{
    /// <summary>
    /// This represents the parameter binding attribute that injects the <see cref="OpenApiHttpTriggerContext"/> instance to each OpenAPI HTTP trigger endpoint.
    /// </summary>
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public class OpenApiHttpTriggerContextAttribute : Attribute
    {
    }
}
