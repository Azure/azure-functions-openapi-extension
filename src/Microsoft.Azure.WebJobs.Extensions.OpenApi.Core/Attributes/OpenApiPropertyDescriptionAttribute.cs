using System;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes
{
    /// <summary>
    /// This represents the attribute entity for OpenAPI model property description.
    /// </summary>
    [Obsolete("This is no longer used. Instead, use the OpenApiPropertyAttribute class.", error: true)]
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class OpenApiPropertyDescriptionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiPropertyDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="description">Property description.</param>
        public OpenApiPropertyDescriptionAttribute(string description)
        {
            this.Description = description;
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public virtual string Description { get; }
    }
}
