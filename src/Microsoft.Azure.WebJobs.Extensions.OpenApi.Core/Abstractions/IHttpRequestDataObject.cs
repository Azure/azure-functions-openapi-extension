using System.IO;

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
        /// Gets the query collection.
        /// </summary>
        IQueryCollection Query { get;}

        /// <summary>
        /// Gets the request payload stream.
        /// </summary>
        Stream Body { get;}
    }
}
