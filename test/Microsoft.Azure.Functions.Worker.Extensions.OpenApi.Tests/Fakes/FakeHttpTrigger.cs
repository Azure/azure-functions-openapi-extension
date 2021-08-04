using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Http;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Tests.Fakes
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

            var res = req.CreateResponse(HttpStatusCode.OK);

            return await Task.FromResult(res).ConfigureAwait(false);
        }
    }
}
