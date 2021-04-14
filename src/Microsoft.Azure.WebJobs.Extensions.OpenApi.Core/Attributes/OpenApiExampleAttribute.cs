using System;
using System.Net;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes
{
    /// <summary>
    /// This represents the attribute entity for the example. This MUST be inherited.
    /// </summary>
    public abstract class OpenApiExampleAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiExampleAttribute"/> class.
        /// </summary>
        protected OpenApiExampleAttribute(Type example)
        {
            this.Example = example.ThrowIfNullOrDefault();
        }

        /// <summary>
        /// Gets the type of the example. It MUST be implementing the <see cref="IExample{T}"/> interface.
        /// </summary>
        public virtual Type Example { get; }
    }

    /// <summary>
    /// This represents the attribute entity for the example of the request body payload.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class OpenApiRequestExampleAttribute : OpenApiExampleAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiRequestExampleAttribute"/> class.
        /// </summary>
        public OpenApiRequestExampleAttribute(Type example)
            : base(example)
        {
        }
    }

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
