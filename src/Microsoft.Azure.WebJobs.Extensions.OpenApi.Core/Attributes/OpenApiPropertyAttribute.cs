using System;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes
{
    /// <summary>
    /// This represents the attribute entity for OpenAPI model property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class OpenApiPropertyAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the value indicating whether the property is nullable or not.
        /// </summary>
        /// <remark>This property will be ignored, if the property itself is marked as nullable, eg) int?, DateTime? or bool?</remark>
        public virtual bool Nullable { get; set; }

        /// <summary>
        /// Gets or sets the default value of the property.
        /// </summary>
        public virtual object Default { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the deprecated flag.
        /// </summary>
        public virtual bool Deprecated { get; set; }
    }
}
