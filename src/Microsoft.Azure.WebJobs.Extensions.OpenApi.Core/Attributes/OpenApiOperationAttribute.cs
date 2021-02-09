using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes
{
    /// <summary>
    /// This represents the attribute entity for HTTP triggers to define Open API operation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class OpenApiOperationAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiOperationAttribute"/> class.
        /// </summary>
        /// <param name="operationId">Operation ID.</param>
        /// <param name="tags">List of tags.</param>
        public OpenApiOperationAttribute(string operationId = null, params string[] tags)
        {
            this.OperationId = operationId;
            this.Tags = tags;
        }

        /// <summary>
        /// Gets the operation ID.
        /// </summary>
        public virtual string OperationId { get; }

        /// <summary>
        /// Gets the list of tags.
        /// </summary>
        public virtual string[] Tags { get; }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        public virtual string Summary { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="OpenApiVisibilityType"/> value. Default is <see cref="OpenApiVisibilityType.Undefined"/>.
        /// </summary>
        public virtual OpenApiVisibilityType Visibility { get; set; } = OpenApiVisibilityType.Undefined;

        /// <summary>
        /// Gets or sets the value indicating whether the operation is deprecated or not.
        /// </summary>
        public virtual bool Deprecated { get; set; }
    }
}
