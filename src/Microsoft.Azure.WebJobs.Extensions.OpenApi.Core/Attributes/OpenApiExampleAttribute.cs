using System;

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
        /// Gets the type of the example. It MUST be inheriting the <see cref="OpenApiExample{T}"/> class.
        /// </summary>
        public virtual Type Example { get; }
    }
}
