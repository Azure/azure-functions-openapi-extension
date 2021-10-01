using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeBooleanParameterExample : OpenApiExample<bool>
    {
        public override IOpenApiExample<bool> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("booleanValue1", true, namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("booleanValue2", false, namingStrategy));
            return this;
        }
    }
}
