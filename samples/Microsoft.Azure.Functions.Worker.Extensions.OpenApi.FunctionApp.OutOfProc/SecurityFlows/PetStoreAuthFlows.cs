using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.OutOfProc.SecurityFlows
{
    public class PetStoreAuth : OpenApiOAuthSecurityFlows
    {
        public PetStoreAuth()
        {
            this.Implicit = new OpenApiOAuthFlow()
            {
                AuthorizationUrl = new Uri("http://petstore.swagger.io/oauth/dialog"),
                Scopes = { { "write:pets", "modify pets in your account" }, { "read:pets", "read your pets" } }
            };
        }
    }
}
