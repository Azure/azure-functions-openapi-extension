using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Services
{
    /// <summary>
    /// This represents the service entity for the sample HTTP trigger.
    /// </summary>
    public class DummyHttpService : IDummyHttpService
    {
        /// <inheritdoc />
        public async Task<List<DummyResponseModel>> GetDummies()
        {
            var result = new List<DummyResponseModel>()
            {
                new DummyResponseModel(),
            };

            return await Task.FromResult(result).ConfigureAwait(false);
        }

        public async Task<DummyResponseModel> AddDummy()
        {
            var result = new DummyResponseModel();

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
