using System;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeTimeSpanParameterExample : OpenApiExample<TimeSpan>
    {
        public override IOpenApiExample<TimeSpan> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("timeSpanValue1", new TimeSpan(6,12,14).ToString(), namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("timeSpanValue2", new TimeSpan(6,12,14,45).ToString(), namingStrategy));
            return this;
        }
    }
}
