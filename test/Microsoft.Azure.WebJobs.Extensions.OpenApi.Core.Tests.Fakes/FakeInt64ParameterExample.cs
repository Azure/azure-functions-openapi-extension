using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeInt64ParameterExample : OpenApiExample<long>
    {
        public override IOpenApiExample<long> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("int64Value1", (long)1, namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("int64Value2", (long)0, namingStrategy));
            return this;
        }
    }
}
