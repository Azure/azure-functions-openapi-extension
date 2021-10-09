using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Examples
{
    public class BooleanParameterExample : OpenApiExample<bool>
    {
        public override IOpenApiExample<bool> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("booleanValue1", true, namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("booleanValue2", false, namingStrategy));
            return this;
        }
    }
}
