using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Examples
{
    public class Int32ParameterExample : OpenApiExample<int>
    {
        public override IOpenApiExample<int> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("int32Value1", 1, namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("int32Value2", 0, namingStrategy));
            return this;
        }
    }
}
