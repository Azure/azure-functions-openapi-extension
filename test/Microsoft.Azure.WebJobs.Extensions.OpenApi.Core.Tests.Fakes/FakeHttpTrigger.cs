using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
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
#if NET5_0
        [Microsoft.Azure.Functions.Worker.Function("FakeFunction")]
#else
        [FunctionName("FakeFunction")]
#endif
        public async Task<IActionResult> DoSomething(
            [HttpTrigger] HttpRequest req
        )
        {
            return await Task.FromResult(new OkResult()).ConfigureAwait(false);
        }
    }
}
