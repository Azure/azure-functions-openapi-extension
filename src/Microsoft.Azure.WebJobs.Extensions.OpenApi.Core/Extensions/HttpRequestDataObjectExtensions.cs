using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="IHttpRequestDataObject"/>.
    /// </summary>
    public static class HttpRequestDataObjectExtensions
    {
        private const string HTTP = "http";
        private const string HTTPS = "https";

        /// <summary>
        /// Gets the scheme whether to return either HTTP or HTTPS depending on the condition.
        /// /// </summary>
        /// <param name="req"><see cref="IHttpRequestDataObject"/> instance.</param>
        /// <param name="options"><see cref="IOpenApiConfigurationOptions"/> instance.</param>
        /// <returns>Return either "HTTP" or "HTTPS", depending on the condition.</returns>
        public static string GetScheme(this IHttpRequestDataObject req, IOpenApiConfigurationOptions options)
        {
            req.ThrowIfNullOrDefault();

            if (options.IsNullOrDefault())
            {
                return req.Scheme;
            }

            if (options.ForceHttps)
            {
                return HTTPS;
            }

            if (options.ForceHttp)
            {
                return HTTP;
            }

            return req.Scheme;
        }
    }
}
