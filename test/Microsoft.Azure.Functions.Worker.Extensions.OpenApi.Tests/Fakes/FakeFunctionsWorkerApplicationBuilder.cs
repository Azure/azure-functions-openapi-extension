using System;

using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Tests.Fakes
{
    public class FakeFunctionsWorkerApplicationBuilder : IFunctionsWorkerApplicationBuilder
    {
        public FakeFunctionsWorkerApplicationBuilder(IServiceCollection services)
        {
            this.Services = services.ThrowIfNullOrDefault();
        }

        public IServiceCollection Services { get; private set; }

        public IFunctionsWorkerApplicationBuilder Use(Func<FunctionExecutionDelegate, FunctionExecutionDelegate> middleware)
        {
            return this;
        }
    }
}
