using System.Collections.Generic;
using System.IO;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// This provides interfaces to the classes that wrap HTTP request object.
    /// </summary>
    public interface IHttpRequestDataObject
    {
        /// <summary>
        /// Gets the scheme name for this URI.
        /// </summary>
        string Scheme { get; }

        /// <summary>
        /// Gets the host header that may include the port number.
        /// </summary>
        HostString Host { get; }

        /// <summary>
        /// Gets the header collection.
        /// </summary>
        IHeaderDictionary Headers { get; }

        /// <summary>
        /// Gets the query collection.
        /// </summary>
        IQueryCollection Query { get; }

        /// <summary>
        /// Gets the list of <see cref="ClaimsIdentity"/>
        /// </summary>
        /// <remarks>This will be added to v2.0.0</remarks>
        //IEnumerable<ClaimsIdentity> Identities { get; }

        /// <summary>
        /// Gets the request payload stream.
        /// </summary>
        Stream Body { get; }
    }
}
