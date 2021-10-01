using System;
using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3Static.Examples
{
    public class DummyListModelExample : OpenApiExample<DummyListModel>
    {
        public override IOpenApiExample<DummyListModel> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "dummy",
                    new DummyListModel()
                    {
                        ListValues1 = new List<DummyStringModel>() { new DummyStringModel() { StringValue = "Hello World", UriValue = new Uri("https://localhost") } },
                        ListValues2 = new HashSet<int>() { 1, 2 },
                        ListValues3 = new HashSet<DummyStringModel>() { new DummyStringModel() { StringValue = "Hello World", UriValue = new Uri("https://localhost") } },
                    },
                    namingStrategy
                ));

            return this;
        }
    }
}
