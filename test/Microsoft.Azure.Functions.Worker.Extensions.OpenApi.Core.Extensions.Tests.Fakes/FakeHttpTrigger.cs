using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker.Http;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Core.Extensions.Tests.Fakes
{
    /// <summary>
    /// This represents the fake class entity.
    /// </summary>
    public class FakeHttpTrigger
    {
        /// <summary>
        /// Gets something
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <returns>Returns <see cref="OkResult"/> instance.</returns>
        [Function("FakeFunction")]
        public async Task<HttpResponseData> DoSomething(
            [HttpTrigger] HttpRequestData req
        )
        {
            var context = new FakeFunctionContext();
            var response = new FakeHttpResponseData(context);

            return await Task.FromResult(response).ConfigureAwait(false);
        }
    }
}
