using Cocona;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.ConsoleApp;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI
{
    /// <summary>
    ///     This represents the console app entity.
    /// </summary>
    public class Program
    {
        /// <summary>
        ///     Invokes the console app.
        /// </summary>
        /// <param name="args">List of arguments.</param>
        public static void Main(string[] args)
        {
            CoconaApp
                .Create()
                .ConfigureServices(services =>
                {
                    services.ConfigureInternalServices();
                })
                .Run<GenerateSwaggerConsoleApp>(args);
        }
    }
}