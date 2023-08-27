using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums
{
        /// <summary>
    /// This specifies the NamingStrategy of OpenAPI Serialization.
    /// </summary>
    public enum OpenApiNamingStrategyType
    {
                /// <summary>
        /// Identifies "CamelCase" NamingStrategy
        /// </summary>
        CamelCase = 0,

        /// <summary>
        /// Identifies "PascalCase" NamingStrategy.
        /// </summary>
        PascalCase = 1,

        /// <summary>
        /// Identifies "SnakeCase" NamingStrategy.
        /// </summary>
        SnakeCase = 2,

        /// <summary>
        /// Identifies "KebabCase" NamingStrategy.
        /// </summary>
        KebabCase = 3

    }
}