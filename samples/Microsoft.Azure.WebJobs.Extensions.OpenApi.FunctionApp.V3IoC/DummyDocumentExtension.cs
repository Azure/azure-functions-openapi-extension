using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3IoC
{
    public class DummyDocumentExtension : IDocumentExtension
    {
        public IDocument ExtendDocument(IDocument document, HttpRequest request)
        {
            var okResponse = new OpenApiResponse
            {
                Description = "Dummy document extension",
            };

            okResponse.Content.Add("application/json", new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Type = "json"
                }
            });

            var operation = new OpenApiOperation
            {
                OperationId = "dummy",
                Responses = new OpenApiResponses
                {
                    {"200", okResponse}
                }
            };

            var healthcheckPath = new OpenApiPathItem
            {
                Description = "Dummy Document Extension",
                Summary = "Provides an example of extending the OpenApi spec through code"
            };

            healthcheckPath.Operations.Add(OperationType.Get, operation);
            document.OpenApiDocument.Paths["/dummy"] = healthcheckPath;

            return document;
        }
    }
}
