using System;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes
{
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
}
