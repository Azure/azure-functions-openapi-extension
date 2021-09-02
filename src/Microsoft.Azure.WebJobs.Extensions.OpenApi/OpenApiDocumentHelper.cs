
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi
{
    /// <summary>
    /// Document helper for Azure Functions
    /// </summary>
    public class OpenApiDocumentHelper : DocumentHelper<FunctionNameAttribute>
    {
        /// <summary
        /// Initializes a new instance of the <see cref="OpenApiDocumentHelper"/> class.
        /// </summary>
        public OpenApiDocumentHelper(RouteConstraintFilter filter, IOpenApiSchemaAcceptor acceptor)
            : base(filter, acceptor, f => f.Name)
        {
        }
    }
}
