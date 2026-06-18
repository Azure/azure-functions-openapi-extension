using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Generator;

// Usage: --assembly <path-to-function-app.dll> --output <swagger.json> [--route-prefix api]
return await GeneratorCli.RunAsync(args);
