using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
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
        /// <param name="namespaceType"><see cref="OpenApiNamespaceType"/> value.</param>
        void Accept(VisitorCollection collection, NamingStrategy namingStrategy, OpenApiNamespaceType namespaceType);
    }
}
