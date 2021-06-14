using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeExample : OpenApiExample<FakeClassModel>
    {
        public override IOpenApiExample<FakeClassModel> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("first", new FakeClassModel() { Number = 1, Text = "Hello World" }, namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("second", "this is summary", new FakeClassModel() { Number = 3, Text = "Lorem Ipsum" }, namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("third", "this is summary", "this is description", new FakeClassModel() { Number = 2, Text = "Hello Ipsum" }, namingStrategy));

            return this;
        }
    }
}
