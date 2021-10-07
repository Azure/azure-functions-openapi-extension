using System.Net;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Tests.Fakes
{
    public class FakeOpenApiHttpTriggerAuthorization : DefaultOpenApiHttpTriggerAuthorization
    {
        public const HttpStatusCode StatusCode = HttpStatusCode.Redirect;
        public const string ContentType = "text/html";
        public const string Payload = "<html><head><meta http-equiv=\"refresh\" content=\"0; url=http://example.com/\" /></head><body></body></html>";

        public override async Task<OpenApiAuthorizationResult> AuthorizeAsync(IHttpRequestDataObject req)
        {
            var result = new OpenApiAuthorizationResult()
            {
                StatusCode = StatusCode,
                ContentType = ContentType,
                Payload = Payload,
            };

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
