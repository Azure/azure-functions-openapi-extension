using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes
{
    /// <summary>
    /// This represents the attribute entity for <see cref="IOpenApiHttpTriggerAuthorization"/> to be excluded from auto-loading.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class OpenApiHttpTriggerAuthorizationIgnoreAttribute : Attribute
    {
    }
}
