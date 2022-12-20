using System.Collections.Generic;
using System.IO;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi
{
    /// <summary>
    /// This represents the HTTP request data entity.
    /// </summary>
    public class HttpRequestObject : IHttpRequestDataObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestObject"/> class.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        public HttpRequestObject(HttpRequest req)
        {
            req.ThrowIfNullOrDefault();

            this.Scheme = req.Scheme;
            this.Host = req.Host;
            this.Headers = req.Headers;
            this.Query = req.Query;
            this.Identities = req.HttpContext.User.Identities;
            this.Body = req.Body;
        }

        /// <inheritdoc/>
        public virtual string Scheme { get; }

        /// <inheritdoc/>
        public virtual HostString Host { get; }

        /// <inheritdoc/>
        public virtual IHeaderDictionary Headers { get; }

        /// <inheritdoc/>
        public virtual IQueryCollection Query { get; }

        /// <inheritdoc/>
        /// <remarks>This property has implementation but will appear on the interface from v2.0.0.</remarks>
        public virtual IEnumerable<ClaimsIdentity> Identities { get; }

        /// <inheritdoc/>
        public virtual Stream Body { get; }
    }
}
