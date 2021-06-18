using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.V3Net5
{
    public class Program
    {
        private const string OpenApiSettingsKey = "OpenApi";

        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
                .ConfigureOpenApi()
                .ConfigureServices(services =>
                 {
                     services.AddTransient<ISample, Sample>();
                 })
                .Build();

            host.Run();
        }
    }

    public interface ISample
    {
        string GetValue();
    }

    public class Sample : ISample
    {
        public string GetValue()
        {
            return "hello world";
        }
    }
}
