
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi
{
    /// <summary>
    /// Document helper for Azure Functions Worker
    /// </summary>
    public class WorkerDocumentHelper : DocumentHelper<FunctionAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerDocumentHelper"/> class.
        /// </summary>
        public WorkerDocumentHelper(RouteConstraintFilter filter, IOpenApiSchemaAcceptor acceptor)
            : base(filter, acceptor, f => f.Name)
        {
        }
    }
}
