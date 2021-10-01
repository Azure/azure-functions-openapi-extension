using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeUint16ParameterExample : OpenApiExample<ushort>
    {
        public override IOpenApiExample<ushort> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("uint16Value1", (ushort)1, namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("uint16Value2", (ushort)0, namingStrategy));
            return this;
        }
    }
}
