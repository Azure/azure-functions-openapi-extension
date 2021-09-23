using System;
using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Examples
{
    class ParameterModelExample : OpenApiExample<string>
    {
        public override IOpenApiExample<string> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("intValue", 1, namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("stringValue", "stringValue", namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("doubleValue", 0.123, namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("date-timeValue", DateTime.Parse("2021.01.01"), namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("booleanValue", false, namingStrategy));
            return this;
        }
    }
}

