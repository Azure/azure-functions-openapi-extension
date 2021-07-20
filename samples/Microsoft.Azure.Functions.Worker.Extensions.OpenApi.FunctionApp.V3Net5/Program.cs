using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.V3Net5.Extensions;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.V3Net5
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
                .Build();

            host.Run();
        }
    }
}
