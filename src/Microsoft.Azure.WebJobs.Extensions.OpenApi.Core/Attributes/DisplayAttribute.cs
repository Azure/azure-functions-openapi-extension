using System;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes
{
    /// <summary>
    /// This represents the attribute entity for enums.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public class DisplayAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayAttribute"/> class.
        /// </summary>
        /// <param name="name">The display name.</param>
        public DisplayAttribute(string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public virtual string Name { get; }
    }
}
