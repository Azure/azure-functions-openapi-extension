using System.Collections.Generic;
using System.Reflection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// This provides an interface to allow additional bpdy parameters to be included in the open api
    /// document, however, only the first body parameter is used at the moment
    /// </summary>
    public interface IAdditionalOpenApiRequestBody
    {
        /// <summary>
        /// Allows generating the open api body parameters for the provided method
        /// </summary>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <returns><see cref="OpenApiRequestBodyAttribute"/> instance.</returns>
        IEnumerable<OpenApiRequestBodyAttribute> OpenApiRequestBody(MethodInfo element);
    }
}
