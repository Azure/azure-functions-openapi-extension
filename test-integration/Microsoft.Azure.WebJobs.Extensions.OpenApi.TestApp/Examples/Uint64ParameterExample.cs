using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Examples
{
    public class Uint64ParameterExample : OpenApiExample<ulong>
    {
        public override IOpenApiExample<ulong> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("uint64Value1", (ulong)1, namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("uint64Value2", (ulong)0, namingStrategy));
            return this;
        }
    }
}
