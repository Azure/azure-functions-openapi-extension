using System.Collections.Generic;
using System.Reflection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// This provides an interface to allow additional api parameters to be included in the open api
    /// </summary>
    public interface IAdditionalOpenApiParameter
    {
        /// <summary>
        /// Allows generating the open api parameters for the provided method
        /// </summary>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <returns><see cref="OpenApiParameterAttribute"/> instance.</returns>
        IEnumerable<OpenApiParameterAttribute> OpenApiParameters(MethodInfo element);
    }
}
