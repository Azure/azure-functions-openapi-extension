using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Examples
{
    public class ByteArrayParameterExample : OpenApiExample<byte[]>
    {
        public override IOpenApiExample<byte[]> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("byteArrayValue1", new byte[] { 172, 24, 18, 240 }, namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("byteArrayValue2", new byte[] { 0xFF, 0x32, 0x11 }, namingStrategy));
            return this;
        }
    }
}
