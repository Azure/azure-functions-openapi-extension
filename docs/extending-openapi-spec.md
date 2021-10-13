# Extending or Modifying the OpenApi Spec at Runtime #

By default, the Azure Functions OpenAPI extensions will locate HTTP function triggers automatically and generate an OpenApi spec
with the appropriate endpoints and descriptions.  However, it is also possible to manipulate the generated spec at runtime before
it is rendered to the client.  This can be done by adding implementations of `IDocumentExtension` to the service collection.

## The `IDocumentExtension` Interface ##

This interface has a single method, which takes an `IDocument` and `HttpRequest` and returns the modified document:

```
public interface IDocumentExtension
{
    IDocument ExtendDocument(IDocument document, HttpRequest request);
}
```

For example, to add an endpoint called `/dummy` to the OpenApi spec with a single *GET* operation which returns a JSON response,
the following implementation could be used:

```
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
```

This type would then need to be added to the service collection using a custom function startup:

```
[assembly: FunctionsStartup(typeof(StartUp))]
public class StartUp : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddSingleton<IDocumentExtension, DummyDocumentExtension>();
    }
}
```

When the `/swagger.json` endpoint is called, the following JSON would be added to the existing endpoints:

```json
{
  "/dummy": {
    "get": {
      "operationId": "dummy",
      "produces": [
        "application/json"
      ],
      "responses": {
        "200": {
          "description": "Dummy document extension",
          "schema": {
            "type": "json"
          }
        }
      }
    },
    "x-summary": "Provides an example of extending the OpenApi spec through code",
    "x-description": "Dummy Document Extension"
  }
}
```

Any `IDocumentExtension` implementations registered will be run before the OpenApi spec is rendered, giving
developers the opportunity to further manipulate the `IDocument` and make any desired changes.
