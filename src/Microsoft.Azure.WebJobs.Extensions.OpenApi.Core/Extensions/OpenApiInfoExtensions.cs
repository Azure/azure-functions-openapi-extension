using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="OpenApiInfo"/>.
    /// </summary>
    public static class OpenApiInfoExtensions
    {
        /// <summary>
        /// Checks whether the given <see cref="OpenApiInfo"/> is valid or not.
        /// </summary>
        /// <param name="openApiInfo"><see cref="OpenApiInfo"/> instance.</param>
        /// <returns>Returns <c>True</c>, if the given <see cref="OpenApiInfo"/> is valid; otherwise returns <c>False</c>.</returns>
        public static bool IsValid(this OpenApiInfo openApiInfo)
        {
            openApiInfo.ThrowIfNullOrDefault();

            return !openApiInfo.IsNullOrDefault() && !openApiInfo.Version.IsNullOrDefault() && !openApiInfo.Title.IsNullOrWhiteSpace();
        }
    }
}
