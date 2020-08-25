using System;
using System.Net;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes
{
    /// <summary>
    /// This represents the attribute entity for HTTP triggers to define response body payload.
    /// </summary>
    [Obsolete("This class is obsolete from 2.0.0. Use OpenApiResponseWithBodyAttribute instead", error: true)]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class OpenApiResponseBodyAttribute : OpenApiPayloadAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiResponseBodyAttribute"/> class.
        /// </summary>
        /// <param name="statusCode">HTTP status code.</param>
        /// <param name="contentType">Content type.</param>
        /// <param name="bodyType">Type of payload.</param>
        public OpenApiResponseBodyAttribute(HttpStatusCode statusCode, string contentType, Type bodyType)
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
