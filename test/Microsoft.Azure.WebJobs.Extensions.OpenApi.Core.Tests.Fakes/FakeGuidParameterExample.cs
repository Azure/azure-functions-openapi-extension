using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;
using System;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeGuidParameterExample : OpenApiExample<Guid>
    {
        public override IOpenApiExample<Guid> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("guidValue1", new Guid("74be27de-1e4e-49d9-b579-fe0b331d3642"), namingStrategy));
            return this;
        }
    }
}
