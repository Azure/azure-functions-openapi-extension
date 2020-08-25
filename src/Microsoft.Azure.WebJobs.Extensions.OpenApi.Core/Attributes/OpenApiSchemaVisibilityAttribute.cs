using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes
{
    /// <summary>
    /// This represents the attribute entity for Open API schema visibility.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class OpenApiSchemaVisibilityAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiSchemaVisibilityAttribute"/> class.
        /// </summary>
        /// <param name="visibility"><see cref="OpenApiVisibilityType"/> value.</param>
        public OpenApiSchemaVisibilityAttribute(OpenApiVisibilityType visibility)
        {
            this.Visibility = visibility == OpenApiVisibilityType.Undefined
                                  ? throw new ArgumentOutOfRangeException(nameof(visibility))
                                  : visibility;
        }

        /// <summary>
        /// Gets the <see cref="OpenApiVisibilityType"/> value.
        /// </summary>
        public virtual OpenApiVisibilityType Visibility { get; }
    }
}
