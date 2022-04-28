using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeFunctionsWithOverlappingModel
    {
        public class OverlappingClass1
        {
            public class FakeInternalModel
            {
            }

            [OpenApiOperation("test-function")]
            [OpenApiRequestBody("application/json", typeof(FakeInternalModel))]
            [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(FakeInternalModel), Summary = "Logs Retrieved.", Description = "Returns the logs matching search parameters.")]
            public void Run()
            {
            }
        }

        public class OverlappingClass2
        {
            public class FakeInternalModel
            {
            }

            [OpenApiOperation("test-function")]
            [OpenApiRequestBody("application/json", typeof(FakeInternalModel))]
            [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(FakeInternalModel), Summary = "Logs Retrieved.", Description = "Returns the logs matching search parameters.")]
            public void Run()
            {
            }
        }
    }
}
