#if NET461
using System.Net;
using System.Net.Http;
#endif
using System.Threading.Tasks;

#if !NET461
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
#endif
using Microsoft.Azure.WebJobs;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    /// <summary>
    /// This represents the fake class entity.
    /// </summary>
    public class FakeHttpTrigger
    {
#if NET461
        /// <summary>
        /// Gets something.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestMessage"/> instance.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> instance.</returns>
        [FunctionName("FakeFunction")]
        public async Task<HttpResponseMessage> DoSomething(
            [HttpTrigger] HttpRequestMessage req
        )
        {
            return await Task.FromResult(req.CreateResponse(HttpStatusCode.OK)).ConfigureAwait(false);
        }
#else
        /// <summary>
        /// Gets something
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <returns>Returns <see cref="OkResult"/> instance.</returns>
        [FunctionName("FakeFunction")]
        public async Task<IActionResult> DoSomething(
            [HttpTrigger] HttpRequest req
        )
        {
            return await Task.FromResult(new OkResult()).ConfigureAwait(false);
        }
#endif
    }
}
