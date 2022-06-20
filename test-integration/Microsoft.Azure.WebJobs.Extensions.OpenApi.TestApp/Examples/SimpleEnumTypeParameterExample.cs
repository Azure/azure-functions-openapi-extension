using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Examples
{
    public class SimpleEnumTypeParameterExample : OpenApiExample<SimpleEnumType>
    {
        public override IOpenApiExample<SimpleEnumType> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("simpleEnumTypeValue1", SimpleEnumType.TitleCase, namingStrategy));
            return this;
        }
    }
}
