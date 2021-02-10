using System;
using System.Net;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes
{
    /// <summary>
    /// This represents the attribute entity for HTTP triggers to define response body payload.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class OpenApiResponseWithoutBodyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiResponseWithoutBodyAttribute"/> class.
        /// </summary>
        /// <param name="statusCode">HTTP status code.</param>
        public OpenApiResponseWithoutBodyAttribute(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Gets the HTTP status code value.
        /// </summary>
        public virtual HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets or sets the type containing the collection of the additional response headers.
        /// </summary>
        public virtual Type HeaderType { get; set; }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        public virtual string Summary { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public virtual string Description { get; set; }
    }
}
