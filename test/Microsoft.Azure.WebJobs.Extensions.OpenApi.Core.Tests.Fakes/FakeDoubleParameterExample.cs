using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeDoubleParameterExample : OpenApiExample<double>
    {
        public override IOpenApiExample<double> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("doubleValue1", 1.0, namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("doubleValue2", 0.0, namingStrategy));
            return this;
        }
    }
}
