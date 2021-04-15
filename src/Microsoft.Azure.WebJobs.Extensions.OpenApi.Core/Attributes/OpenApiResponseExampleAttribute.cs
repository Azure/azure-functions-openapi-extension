using System;
using System.Net;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes
{
    /// <summary>
    /// This represents the attribute entity for the example of the response body payload.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class OpenApiResponseExampleAttribute : OpenApiExampleAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiResponseExampleAttribute"/> class.
        /// </summary>
        public OpenApiResponseExampleAttribute(HttpStatusCode statusCode, Type example)
            : base(example)
        {
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Gets the <see cref="HttpStatusCode"/> value.
        /// </summary>
        public virtual HttpStatusCode StatusCode { get; }
    }
}
