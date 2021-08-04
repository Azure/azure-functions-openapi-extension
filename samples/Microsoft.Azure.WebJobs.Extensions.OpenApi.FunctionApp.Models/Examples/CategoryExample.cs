using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models.PetStore;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models.Examples
{
    public class CategoryExample : OpenApiExample<Category>
    {
        public override IOpenApiExample<Category> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("first", new Category() { Id = (long)123, Name = "Hello World" }, namingStrategy));

            return this;
        }
    }
}
