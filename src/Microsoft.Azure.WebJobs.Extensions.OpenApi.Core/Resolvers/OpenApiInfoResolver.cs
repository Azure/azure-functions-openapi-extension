using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configuration.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configuration.AppSettings.Resolvers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers
{
    /// <summary>
    /// This represents the resolver entity for <see cref="OpenApiInfo"/> from one of host.json, openapisettings.json and environment variables.
    /// </summary>
    public static class OpenApiInfoResolver
    {
        /// <summary>
        /// Gets the <see cref="OpenApiInfo"/> instance from one of host.json, openapisettings.json and environment variables.
        /// </summary>
        /// <param name="host"><see cref="IConfiguration"/> instance representing host.json.</param>
        /// <param name="openapi"><see cref="IConfiguration"/> instance representing openapisettings.json.</param>
        /// <param name="appsettings"><see cref="IConfiguration"/> instance representing environment variables.</param>
        /// <returns>Returns <see cref="OpenApiInfo"/> instance resolved.</returns>
        public static OpenApiInfo Resolve(IConfiguration host = null, IConfiguration openapi = null, IConfiguration appsettings = null)
        {
            if (host.IsNullOrDefault())
            {
                host = HostJsonResolver.Resolve();
            }

            var info = host.Get<OpenApiInfo>("openApi:info");
            if (info.IsValid())
            {
                return info;
            }

            info = new OpenApiInfo()
            {
                Version = "1.0.0",
                Title = "Azure Functions Open API"
            };

  //     "version": "3.0.0",
  //     "title": "Open API Sample on Azure Functions (STATIC)",
  //     "description": "A sample API that runs on Azure Functions (STATIC) 3.x using Open API specification.",
  //     "termsOfService": "https://github.com/Azure/azure-functions-openapi-extension",
  //     "contact": {
  //       "name": "Contoso",
  //       "email": "azfunc-openapi@contoso.com",
  //       "url": "https://github.com/Azure/azure-functions-openapi-extension/issues"
  //     },
  //     "license": {
  //       "name": "MIT",
  //       "url": "http://opensource.org/licenses/MIT"
  //     }


            // if (openapi.IsNullOrDefault())
            // {
            //     openapi = OpenApiSettingsJsonResolver.Resolve();
            // }

            // info = openapi.Get<OpenApiInfo>("info");
            // if (info.IsValid())
            // {
            //     return info;
            // }

            // if (appsettings.IsNullOrDefault())
            // {
            //     appsettings = ConfigurationResolver.Resolve();
            // }

            // info = appsettings.Get<OpenApiInfo>("OpenApi:Info");
            // if (info.IsValid())
            // {
            //     return info;
            // }

            throw new InvalidOperationException("Open API metadata not found");
        }
    }
}
