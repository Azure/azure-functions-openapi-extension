using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes
{
    /// <summary>
    /// This represents the attribute entity for HTTP triggers to define Open API operation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class OpenApiSecurityAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiSecurityAttribute"/> class.
        /// </summary>
        /// <param name="schemeName">Open API security scheme name.</param>
        /// <param name="schemeType">Open API security scheme type.</param>
        public OpenApiSecurityAttribute(string schemeName, SecuritySchemeType schemeType)
        {
            this.SchemeName = schemeName ?? throw new ArgumentNullException(nameof(schemeName));
            this.SchemeType = schemeType;
        }

        /// <summary>
        /// Gets the Open API security scheme name.
        /// </summary>
        public virtual string SchemeName { get; }

        /// <summary>
        /// Gets the Open API security scheme type.
        /// </summary>
        public virtual SecuritySchemeType SchemeType { get; }

        /// <summary>
        /// Gets or sets the Open API security scheme description.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the header, query or cookie. This MUST be provided when the <see cref="SecuritySchemeType"/> value is <see cref="SecuritySchemeType.ApiKey"/>.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the location of the API key. This MUST be provided when the <see cref="SecuritySchemeType"/> value is <see cref="SecuritySchemeType.ApiKey"/>.
        /// </summary>
        public virtual OpenApiSecurityLocationType In { get; set; }

        /// <summary>
        /// Gets or sets the name of the authorisation scheme. This MUST be provided when the <see cref="SecuritySchemeType"/> value is <see cref="SecuritySchemeType.Http"/>.
        /// </summary>
        public virtual OpenApiSecuritySchemeType Scheme { get; set; }

        /// <summary>
        /// Gets or sets the hint to the client to identify how the bearer token is formatted. This MUST be provided when the <see cref="SecuritySchemeType"/> value is <see cref="SecuritySchemeType.Http"/>.
        /// </summary>
        public virtual string BearerFormat { get; set; }

        /// <summary>
        /// Gets or sets the configuration information for the flow types supported. This MUST be the type inheriting <see cref="OpenApiOAuthSecurityFlows"/> This MUST be provided when the <see cref="SecuritySchemeType"/> value is <see cref="SecuritySchemeType.OAuth2"/>.
        /// </summary>
        public virtual Type Flows { get; set;}

        /// <summary>
        /// Gets or sets the OpenId Connect URL to discover OAuth2 configration values. This MUST be provided when the <see cref="SecuritySchemeType"/> value is <see cref="SecuritySchemeType.OpenIdConnect"/>.
        /// </summary>
        public virtual string OpenIdConnectUrl { get; set; }

        /// <summary>
        /// Gets or sets the comma delimited list of scopes of OpenId Connect. This MUST be provided when the <see cref="SecuritySchemeType"/> value is <see cref="SecuritySchemeType.OpenIdConnect"/>.
        /// </summary>
        public virtual string OpenIdConnectScopes { get; set; }
    }
}
