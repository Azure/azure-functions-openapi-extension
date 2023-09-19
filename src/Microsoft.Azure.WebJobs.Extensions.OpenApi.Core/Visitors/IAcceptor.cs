using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// This provides interfaces to the acceptor classes.
    /// </summary>
    public interface IAcceptor
    {
        /// <summary>
        /// Accepts the visitors.
        /// </summary>
        /// <param name="collection"><see cref="VisitorCollection"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance.</param>
        /// <param name="useFullName">instance to get or set the value indicating whether to use the FullName or not.</param>
        void Accept(VisitorCollection collection, NamingStrategy namingStrategy, bool useFullName);
    }
}
