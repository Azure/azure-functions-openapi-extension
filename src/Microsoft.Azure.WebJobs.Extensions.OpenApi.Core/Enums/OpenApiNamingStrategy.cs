using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums
{
    /// <summary>
    /// This specifies the NamingStrategy of OpenAPI Serialization.
    /// </summary>
    public enum OpenApiNamingStrategy
    {
        /// <summary>
        /// Identifies "CamelCase" NamingStrategy
        /// </summary>
        CamelCase,

        /// <summary>
        /// Identifies "PascalCase" NamingStrategy.
        /// </summary>
        PascalCase,

        /// <summary>
        /// Identifies "SnakeCase" NamingStrategy.
        /// </summary>
        SnakeCase,

        /// <summary>
        /// Identifies "KebabCase" NamingStrategy.
        /// </summary>
        KebabCase
    }
}
