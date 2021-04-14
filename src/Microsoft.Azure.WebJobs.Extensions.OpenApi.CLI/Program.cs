using System;
using System.IO;
using System.Text;

using Cocona;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;

using Moq;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI
{
    /// <summary>
    /// This represents the console app entity.
    /// </summary>
    public class Program
    {
        private static readonly char directorySeparator = Path.DirectorySeparatorChar;

        /// <summary>
        /// Invokes the console app.
        /// </summary>
        /// <param name="args">List of arguments.</param>
        public static void Main(string[] args)
        {
            CoconaLiteApp.Run<Program>(args);
        }

        /// <summary>
        /// Generates the OpenAPI document.
        /// </summary>
        /// <param name="project">Project path.</param>
        /// <param name="configuration">Copile configuration.</param>
        /// <param name="version">OpenAPI version.</param>
        /// <param name="format">OpenAPI output format.</param>
        /// <param name="output">Output path.</param>
        /// <param name="console">Value indicating whether to render the document on console or not.</param>
        public void Generate(
            [Option('p', Description = "Project path. Default is current directory")] string project = ".",
            [Option('c', Description = "Configuration. Default is 'Debug'")] string configuration = "Debug",
            [Option('t', Description = "Target framework. Default is 'netcoreapp2.1'")] string target = "netcoreapp2.1",
            [Option('v', Description = "OpenAPI spec version. Value can be either 'v2' or 'v3'. Default is 'v2'")] OpenApiVersionType version = OpenApiVersionType.V2,
            [Option('f', Description = "OpenAPI output format. Value can be either 'json' or 'yaml'. Default is 'yaml'")] OpenApiFormatType format = OpenApiFormatType.Json,
            [Option('o', Description = "Generated OpenAPI output location. Default is 'output'")] string output = "output",
            bool console = false)
        {
            var pi = default(ProjectInfo);
            try
            {
                pi = new ProjectInfo(project, configuration, target);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                return;
            }

            var req = new Mock<IHttpRequestDataObject>();
            req.SetupGet(p => p.Scheme).Returns("http");
            req.SetupGet(p => p.Host).Returns(new HostString("localhost", 7071));

            var filter = new RouteConstraintFilter();
            var acceptor = new OpenApiSchemaAcceptor();
            var namingStrategy = new CamelCaseNamingStrategy();
            var collection = VisitorCollection.CreateInstance();
            var helper = new DocumentHelper(filter, acceptor);
            var document = new Document(helper);

            var swagger = default(string);
            try
            {
                swagger = document.InitialiseDocument()
                                  .AddMetadata(pi.OpenApiInfo)
                                  .AddServer(req.Object, pi.HostJsonHttpSettings.RoutePrefix)
                                  .AddNamingStrategy(namingStrategy)
                                  .AddVisitors(collection)
                                  .Build(pi.CompiledDllPath)
                                  .RenderAsync(version.ToOpenApiSpecVersion(), format.ToOpenApiFormat())
                                  .Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            if (console)
            {
                Console.WriteLine(swagger);
            }

            var outputpath = Path.IsPathFullyQualified(output)
                                 ? output
                                 : $"{pi.CompiledPath}{directorySeparator}{output}";

            if (!Directory.Exists(outputpath))
            {
                Directory.CreateDirectory(outputpath);
            }

            File.WriteAllText($"{outputpath}{directorySeparator}swagger.{format.ToDisplayName()}", swagger, Encoding.UTF8);
        }
    }
}
