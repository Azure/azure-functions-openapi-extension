using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Services
{
    /// <summary>
    /// This provides interfaces to <see cref="DummyHttpService"/>.
    /// </summary>
    public interface IDummyHttpService
    {
        /// <summary>
        /// Gets the list of <see cref="DummyResponseModel"/> objects.
        /// </summary>
        /// <returns>Returns the list of <see cref="DummyResponseModel"/> objects.</returns>
        Task<List<DummyResponseModel>> GetDummies();

        /// <summary>
        /// Adds dummy data.
        /// </summary>
        /// <returns>Returns the <see cref="DummyResponseModel"/> instance.</returns>
        Task<DummyResponseModel> AddDummy();
    }
}
