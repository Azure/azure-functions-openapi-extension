using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi
{
    /// <summary>
    /// This represents the HTTP request data entity.
    /// </summary>
    public class HttpRequestObject : IHttpRequestDataObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestObject"/> class.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestData"/> instance.</param>
        public HttpRequestObject(HttpRequestData req)
        {
            // req.ThrowIfNullOrDefault();

            this.Scheme = req.Url.Scheme;
            this.Host = new[] { 80, 443 }.Contains(req.Url.Port)
                        ? new HostString(req.Url.Authority)
                        : new HostString(req.Url.Host, req.Url.Port);
        }

        /// <inheritdoc/>
        public virtual string Scheme { get; }

        /// <inheritdoc/>
        public virtual HostString Host { get; }
    }
}
