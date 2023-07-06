using System.Collections.Generic;
using System.Net;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public static class FakeFunctions
    {
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<FakeClassModel>), Summary = "List of the fake responses", Description = "This returns the list of fake responses", CustomHeaderType = typeof(FakeResponseHeader))]
        public static void GetFakes()
        {
        }

        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(FakeClassModel), Required = true, Description = "Fake request model")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(FakeClassModel), Summary = "Fake response", Description = "This returns the fake response", CustomHeaderType = typeof(FakeResponseHeader))]
        public static void AddFakes()
        {
        }

        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(FakeListModel), Required = true, Description = "Fake list model")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<FakeStringModel>), Summary = "Fake response", Description = "This returns the fake response", CustomHeaderType = typeof(FakeResponseHeader))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(FakeStringModel), Summary = "Fake response", Description = "This returns the fake response", Deprecated = true, CustomHeaderType = typeof(FakeResponseHeader))]
        public static void UpdateFakes()
        {
        }

        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(FakeGenericModel<FakeClassModel>), Required = true, Description = "Fake list model")]
        public static void GenericMethodOne()
        {
        }

        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(FakeGenericModel<FakeOtherClassModel>), Required = true, Description = "Fake list model")]
        public static void GenericMethodTwo()
        {
        }

        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(FakeOtherGenericModel<FakeClassModel, FakeOtherClassModel>), Required = true, Description = "Fake list model")]
        public static void GenericMethodThree()
        {
        }
    }
}
