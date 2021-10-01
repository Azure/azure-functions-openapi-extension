// using System.Globalization;
// using System.Linq;
// using System.Net;
// using System.Threading.Tasks;

// using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
// using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
// using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

// namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3Static.Configurations
// {
//     public class OpenApiHttpTriggerAuthorization : DefaultOpenApiHttpTriggerAuthorization
//     {
//         public override async Task<OpenApiAuthorizationResult> AuthorizeAsync(IHttpRequestDataObject req)
//         {
//             var result = default(OpenApiAuthorizationResult);
//             var authtoken = (string) req.Headers["Authorization"];
//             if (authtoken.IsNullOrWhiteSpace())
//             {
//                 result = new OpenApiAuthorizationResult()
//                 {
//                     StatusCode = HttpStatusCode.Unauthorized,
//                     ContentType = "text/plain",
//                     Payload = "Unauthorized",
//                 };

//                 return await Task.FromResult(result).ConfigureAwait(false);
//             }

//             if (authtoken.StartsWith("Bearer", ignoreCase: true, CultureInfo.InvariantCulture) == false)
//             {
//                 result = new OpenApiAuthorizationResult()
//                 {
//                     StatusCode = HttpStatusCode.Unauthorized,
//                     ContentType = "text/plain",
//                     Payload = "Invalid auth format",
//                 };

//                 return await Task.FromResult(result).ConfigureAwait(false);
//             }

//             var token = authtoken.Split(' ').Last();
//             if (token != "secret")
//             {
//                 result = new OpenApiAuthorizationResult()
//                 {
//                     StatusCode = HttpStatusCode.Forbidden,
//                     ContentType = "text/plain",
//                     Payload = "Invalid auth token",
//                 };

//                 return await Task.FromResult(result).ConfigureAwait(false);
//             }

//             return await Task.FromResult(result).ConfigureAwait(false);
//         }
//     }
// }
