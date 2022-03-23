using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;
using Microsoft.OpenApi.Models;
using Moq;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Services
{
    public class CustomOpenApiCreator : ICustomOpenApiCreator
    {
        public async Task<string> CreateOpenApiDocument(
            string apiBaseUrl,
            string compiledDllPath,
            string routePrefix,
            OpenApiInfo openApiInfo,
            OpenApiVersionType version,
            OpenApiFormatType format)
        {
            try
            {
                var query = new Mock<IQueryCollection>();
                var request = new Mock<IHttpRequestDataObject>();
                request.SetupGet(p => p.Scheme).Returns("https");
                request.SetupGet(p => p.Host).Returns(new HostString(apiBaseUrl));
                request.SetupGet(p => p.Query).Returns(query.Object);

                var document = new Functions.Worker.Extensions.OpenApi.Document(new DocumentHelper(new RouteConstraintFilter(), new OpenApiSchemaAcceptor()));

                var swagger = await document.InitialiseDocument()
                    .AddMetadata(openApiInfo)
                    .AddServer(request.Object, routePrefix)
                    .AddNamingStrategy(new CamelCaseNamingStrategy())
                    .AddVisitors(VisitorCollection.CreateInstance())
                    .Build(compiledDllPath)
                    .RenderAsync(version.ToOpenApiSpecVersion(), format.ToOpenApiFormat());

                return swagger;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}