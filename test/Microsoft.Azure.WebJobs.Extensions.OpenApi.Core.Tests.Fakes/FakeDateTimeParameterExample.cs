using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeDateTimeParameterExample : OpenApiExample<DateTime>
    {
        public override IOpenApiExample<DateTime> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("dateTimeValue1", DateTime.Parse("2021-01-01"), namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("dateTimeValue2", DateTime.Parse("2021-01-01T12:34:56Z"), namingStrategy));
            return this;
        }
    }
}
