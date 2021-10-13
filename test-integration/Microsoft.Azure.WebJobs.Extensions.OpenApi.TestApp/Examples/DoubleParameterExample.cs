using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Examples
{
    public class DoubleParameterExample : OpenApiExample<double>
    {
        public override IOpenApiExample<double> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("doubleValue1", 1.1, namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("doubleValue2", 0.0, namingStrategy));
            return this;
        }
    }
}
