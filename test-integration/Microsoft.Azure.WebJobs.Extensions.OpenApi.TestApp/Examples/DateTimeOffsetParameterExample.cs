using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Examples
{
    public class DateTimeOffsetParameterExample : OpenApiExample<DateTimeOffset>
    {
        public override IOpenApiExample<DateTimeOffset> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("dateTimeOffsetValue1", DateTimeOffset.Parse("05/01/2008"), namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("dateTimeOffsetValue2", DateTimeOffset.Parse("11:36 PM"), namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("dateTimeOffsetValue3", DateTimeOffset.Parse("05/01/2008 +1:00"), namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("dateTimeOffsetValue4", DateTimeOffset.Parse("Thu May 01, 2008"), namingStrategy));
            return this;
        }
    }
}
