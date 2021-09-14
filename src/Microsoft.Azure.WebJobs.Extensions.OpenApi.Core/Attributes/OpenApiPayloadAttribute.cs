using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes
{
    /// <summary>
    /// This represents the attribute entity for HTTP triggers to define payload. This MUST be inherited.
    /// </summary>
    public abstract class OpenApiPayloadAttribute : Attribute
    {
        private string _typeFullNameAlias = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiPayloadAttribute"/> class.
        /// </summary>
        /// <param name="contentType">Content type.</param>
        /// <param name="bodyType">Type of payload.</param>
        protected OpenApiPayloadAttribute(string contentType, Type bodyType)
        {
            this.ContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
            this.BodyType = bodyType ?? throw new ArgumentNullException(nameof(bodyType));
        }

        /// <summary>
        /// Gets the content type.
        /// </summary>
        public virtual string ContentType { get; }

        /// <summary>
        /// Gets the payload body type.
        /// </summary>
        public virtual Type BodyType { get; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the example. It SHOULD be inheriting the <see cref="OpenApiExample{T}"/> class.
        /// </summary>
        public virtual Type Example { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether or not the response's type's name should be used. 
        /// </summary>
        /// <remarks>
        /// The default value is false.
        /// </remarks>
        public bool UseTypeFullName { get; set; }

        /// <summary>
        /// Gets or optionally sets the type full name's alias. 
        /// </summary>
        /// <remarks>
        /// This property is taken into account if UseTypeFullName set to true.
        /// </remarks>
        public string BodyTypeAlias
        {
            get => this._typeFullNameAlias;
            set
            {
                if(value.Contains(".") || value.Contains("+"))
                {
                    throw new ArgumentException($"The alias name should not contain dot (.) or plus (+).");
                }
                this._typeFullNameAlias = value;
            }
        }
    }
}
