using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Examples
{
    public class Uint32ParameterExample : OpenApiExample<uint>
    {
        public override IOpenApiExample<uint> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("uint32Value1", (uint)1, namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("uint32Value2", (uint)0, namingStrategy));
            return this;
        }
    }
}
