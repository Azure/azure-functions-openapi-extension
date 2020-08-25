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
        /// <param name="contentType">Content type.</param>
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
        /// Gets or sets the summary.
        /// </summary>
        public virtual string Summary { get; set; }
    }
}
