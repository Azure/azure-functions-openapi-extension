using System.Collections.Generic;
using System.Reflection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// This provides an interface to allow additional api response parameters to be included in the open api
    /// </summary>
    public interface IAdditionalOpenApiResponse
    {
        /// <summary>
        /// Allows generating the open api response body parameters for the provided method
        /// </summary>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <returns><see cref="OpenApiResponseWithBodyAttribute"/> instance.</returns>
        List<OpenApiResponseWithBodyAttribute> OpenApiResponseWithBody(MethodInfo element);

        /// <summary>
        /// Allows generating the open api response without any body parameters for the provided method
        /// </summary>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <returns><see cref="OpenApiResponseWithoutBodyAttribute"/> instance.</returns>
        List<OpenApiResponseWithoutBodyAttribute> OpenApiResponseWithoutBody(MethodInfo element);
    }
}
