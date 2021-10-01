using System.Net;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// This represents the entity for the authorisation result.
    /// </summary>
    public class OpenApiAuthorizationResult
    {
        /// <summary>
        /// Gets or sets the HTTP status code.
        /// </summary>
        public virtual HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        public virtual string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the payload.
        /// </summary>
        public virtual string Payload { get; set; }
    }
}
