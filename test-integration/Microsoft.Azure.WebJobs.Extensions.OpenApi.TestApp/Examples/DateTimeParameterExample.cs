using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Examples
{
    public class DateTimeParameterExample : OpenApiExample<DateTime>
    {
        public override IOpenApiExample<DateTime> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("dateTimeValue1", DateTime.Parse("2021-01-01"), namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("dateTimeValue2", DateTime.Parse("2021-01-01T12:34:56Z"), namingStrategy));
            return this;
        }
    }
}
