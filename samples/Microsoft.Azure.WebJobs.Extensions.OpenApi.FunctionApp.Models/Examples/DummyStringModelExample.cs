using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models.Examples
{
    public class DummyStringModelExample : OpenApiExample<DummyStringModel>
    {
        public override IOpenApiExample<DummyStringModel> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("lorem", new DummyStringModel() { StringValue = "Hello World", UriValue = new Uri("http://localhost:80") }, namingStrategy));
            this.Examples.Add(OpenApiExampleResolver.Resolve("ipsum", new DummyStringModel() { StringValue = "Hello World 2", UriValue = new Uri("https://localhost:443") }, namingStrategy));

            return this;
        }
    }
}
