using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configuration.AppSettings;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// This represents the base settings entity from the configurations for Open API.
    /// </summary>
    public abstract class OpenApiAppSettingsBase : AppSettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiAppSettingsBase"/> class.
        /// </summary>
        protected OpenApiAppSettingsBase()
            : base()
        {
            var basePath = this.GetBasePath();
            var host = HostJsonResolver.Resolve(this.Config, basePath);
            var options = OpenApiConfigurationResolver.Resolve(Assembly.GetExecutingAssembly());

            this.OpenApiInfo = options.Info;
            this.SwaggerAuthKey = this.Config.GetValue<string>("OpenApi:ApiKey");

            this.HttpSettings = host.GetHttpSettings();
        }

        /// <summary>
        /// Gets the <see cref="Microsoft.OpenApi.Models.OpenApiInfo"/> instance.
        /// </summary>
        public virtual OpenApiInfo OpenApiInfo { get; }

        /// <summary>
        /// Gets the Function API key for Open API document.
        /// </summary>
        public virtual string SwaggerAuthKey { get; }

        /// <summary>
        /// Gets the <see cref="Configurations.HttpSettings"/> instance.
        /// </summary>
        public virtual HttpSettings HttpSettings { get; }
    }
}
