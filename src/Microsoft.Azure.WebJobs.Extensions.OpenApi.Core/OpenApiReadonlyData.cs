using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core
{
    public static class OpenApiReadonlyData
    {
        public static HashSet<string> NonReferentialsTypeString { get; } = new HashSet<string>
        {
            "OBJECT",
            "JTOKEN",
            "JOBJECT",
            "JARRAY"
        };

        public static bool IsNonReferentialsTypeString(string typeName)
            => NonReferentialsTypeString.Contains(typeName.ToUpperInvariant());
    }
}
