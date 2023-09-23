using System;
using System.Net;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes
{
    /// <summary>
    /// This represents the attribute entity for HTTP triggers to define response body payload.
    /// </summary>

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class OpenApiResponseWithBodyAttribute : OpenApiPayloadAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiResponseWithBodyAttribute"/> class.
        /// </summary>
        /// <param name="statusCode">HTTP status code.</param>
        /// <param name="contentType">Content type. This can be a comma seperated list for additional content types.</param>
        /// <param name="bodyType">Type of payload.</param>
        public OpenApiResponseWithBodyAttribute(HttpStatusCode statusCode, string contentType, Type bodyType)
            : base(contentType, bodyType)
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
        public virtual Type CustomHeaderType { get; set; }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        public virtual string Summary { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether the response payload is deprecated or not.
        /// </summary>
        public virtual bool Deprecated { get; set; }
    }
}
