using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeInt16ParameterExample : OpenApiExample<short>
    {
        public override IOpenApiExample<short> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("int16Value1", (short)1, namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("int16Value2", (short)0, namingStrategy));
            return this;
        }
    }
}
