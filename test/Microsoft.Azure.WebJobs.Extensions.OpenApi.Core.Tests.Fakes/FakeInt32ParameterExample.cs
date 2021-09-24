using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeInt32ParameterExample : OpenApiExample<int>
    {
        public override IOpenApiExample<int> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("int32Value1", 1, namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("int32Value2", 0, namingStrategy));
            return this;
        }
    }
}
