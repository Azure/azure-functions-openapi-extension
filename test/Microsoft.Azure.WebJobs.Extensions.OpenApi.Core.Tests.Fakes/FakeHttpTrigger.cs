using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

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
        [FunctionName("FakeFunction")]
        [OpenApiOperation(operationId: "fake", tags: new[] { "fake" })]
        public async Task<IActionResult> DoSomething(
            [HttpTrigger] HttpRequest req
        )
        {
            return await Task.FromResult(new OkResult()).ConfigureAwait(false);
        }
    }
}
