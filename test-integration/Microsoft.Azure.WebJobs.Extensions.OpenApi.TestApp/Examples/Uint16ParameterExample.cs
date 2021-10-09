using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Examples
{
    public class Uint16ParameterExample : OpenApiExample<ushort>
    {
        public override IOpenApiExample<ushort> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("uint16Value1", (ushort)1, namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("uint16Value2", (ushort)0, namingStrategy));
            return this;
        }
    }
}
