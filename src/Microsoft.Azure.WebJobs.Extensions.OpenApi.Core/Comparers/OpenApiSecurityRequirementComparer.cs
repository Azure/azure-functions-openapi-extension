using System.Collections.Generic;
using System.Linq;

using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Comparers
{
    /// <summary>
    /// This represents the comparer entity for <see cref="OpenApiSecurityRequirement"/> class.
    /// /// </summary>
    public class OpenApiSecurityRequirementComparer : IEqualityComparer<OpenApiSecurityRequirement>
    {
        /// <inheritdoc />
        public bool Equals(OpenApiSecurityRequirement x, OpenApiSecurityRequirement y)
        {
            var refX = string.Join(",", x.Keys.Select(p => p.Reference.Id).OrderBy(p => p));
            var refY = string.Join(",", y.Keys.Select(p => p.Reference.Id).OrderBy(p => p));

            return refX == refY;
        }

        /// <inheritdoc />
        public int GetHashCode(OpenApiSecurityRequirement obj)
        {
            if (object.ReferenceEquals(obj, null))
            {
                return 0;
            }

            var hashCode = string.Join(",", obj.Keys.Select(p => p.Reference.Id).OrderBy(p => p)).GetHashCode();

            return hashCode;
        }
    }
}
