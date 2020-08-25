using System;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configuration.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI
{
    /// <summary>
    /// This represents the project info entity.
    /// </summary>
    public class ProjectInfo
    {
        private static readonly char directorySeparator = System.IO.Path.DirectorySeparatorChar;

        private string _projectPath;
        private string _filename;
        private string _configuration;
        private string _targetFramework;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectInfo"/> class.
        /// </summary>
        public ProjectInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectInfo"/> class.
        /// </summary>
        /// <param name="projectPath">Project path.</param>
        /// <param name="configuration">Configuration value.</param>
        /// <param name="targetFramework">Target framework.</param>
        public ProjectInfo(string projectPath, string configuration, string targetFramework)
        {
            this.SetProjectPath(projectPath.ThrowIfNullOrWhiteSpace());
            this._configuration = configuration.ThrowIfNullOrWhiteSpace();
            this._targetFramework = targetFramework.ThrowIfNullOrWhiteSpace();

            this.SetHostSettings();
            this.SetOpenApiInfo();
        }

        /// <summary>
        /// Gets or sets the fully qualified project path.
        /// </summary>
        public virtual string Path
        {
            get
            {
                return this._projectPath;
            }
            set
            {
                this.SetProjectPath(value);
            }
        }

        /// <summary>
        /// Gets or sets the compile configuration value.
        /// </summary>
        public virtual string Configuration
        {
            get
            {
                return this._configuration;
            }
            set
            {
                this._configuration = value;
            }
        }

        /// <summary>
        /// Gets or sets the compile target framework value.
        /// </summary>
        public virtual string TargetFramework
        {
            get
            {
                return this._targetFramework;
            }
            set
            {
                this._targetFramework = value;
            }
        }

        /// <summary>
        /// Gets the project filename, ie) *.csproj
        /// </summary>
        public virtual string Filename
        {
            get
            {
                return this._filename;
            }
        }

        /// <summary>
        /// Gets the fully qualified compiled directory path.
        /// </summary>
        public virtual string CompiledPath
        {
            get
            {
                this.Path.ThrowIfNullOrWhiteSpace();
                this.Configuration.ThrowIfNullOrWhiteSpace();
                this.TargetFramework.ThrowIfNullOrWhiteSpace();

                return $"{this.Path.TrimEnd(directorySeparator)}{directorySeparator}bin{directorySeparator}{this.Configuration}{directorySeparator}{this.TargetFramework}";
            }
        }

        /// <summary>
        /// Gets the fully qualified compiled .dll file path.
        /// </summary>
        public virtual string CompiledDllPath
        {
            get
            {
                this.CompiledPath.ThrowIfNullOrWhiteSpace();
                this.Filename.ThrowIfNullOrWhiteSpace();

                return $"{this.CompiledPath}{directorySeparator}bin{directorySeparator}{this.Filename}".Replace(".csproj", ".dll");
            }
        }

        /// <summary>
        /// Gets the fully qualified file path of host.json.
        /// </summary>
        public virtual string HostJsonPath
        {
            get
            {
                this.CompiledPath.ThrowIfNullOrWhiteSpace();

                return $"{this.CompiledPath}{directorySeparator}host.json";
            }
        }

        /// <summary>
        /// Gets the host.json settings.
        /// </summary>
        public virtual IConfiguration HostSettings { get; private set; }

        /// <summary>
        /// Gets the HTTP settings in host.json.
        /// </summary>
        public virtual HttpSettings HostJsonHttpSettings { get; private set; }

        /// <summary>
        /// Gets the fully qualified file path of local.settings.json
        /// </summary>
        public virtual string LocalSettingsJsonPath
        {
            get
            {
                this.CompiledPath.ThrowIfNullOrWhiteSpace();

                return $"{this.CompiledPath}{directorySeparator}local.settings.json";
            }
        }

        /// <summary>
        /// Gets the fully qualified file path of openapisettings.json
        /// </summary>
        public virtual string OpenApiSettingsJsonPath
        {
            get
            {
                this.CompiledPath.ThrowIfNullOrWhiteSpace();

                return $"{this.CompiledPath}{directorySeparator}openapisettings.json";
            }
        }

        /// <summary>
        /// Gets the <see cref="OpenApiInfo"/> instance.
        /// </summary>
        public virtual OpenApiInfo OpenApiInfo { get; private set; }

        private void SetProjectPath(string path)
        {
            if (path.IsNullOrWhiteSpace() || path == ".")
            {
                this._projectPath = Environment.CurrentDirectory.TrimEnd(directorySeparator);

                var filepath = Directory.GetFiles(this._projectPath, "*.csproj").SingleOrDefault();
                if (filepath.IsNullOrWhiteSpace())
                {
                    throw new FileNotFoundException();
                }

                var csproj = new FileInfo(filepath);
                this._filename = csproj.Name;
            }

            var fqpath =
#if NET461
                System.IO.Path.IsPathRooted(path)
#else
                System.IO.Path.IsPathFullyQualified(path)
#endif
                ? path
                : $"{Environment.CurrentDirectory.TrimEnd(directorySeparator)}{directorySeparator}{path}";

            if (fqpath.EndsWith(".csproj"))
            {
                var csproj = new FileInfo(fqpath);

                this._projectPath = csproj.DirectoryName.TrimEnd(directorySeparator);
                this._filename = csproj.Name;
            }

            var di = new DirectoryInfo(fqpath);
            this._projectPath = di.FullName.TrimEnd(directorySeparator);

            var segments = di.FullName.Split(new[] { directorySeparator }, StringSplitOptions.RemoveEmptyEntries);
            this._filename = $"{segments.Last()}.csproj";
        }

        private void SetHostSettings()
        {
            this.HostJsonPath.ThrowIfNullOrWhiteSpace();

            var host = new ConfigurationBuilder()
                       .SetBasePath(this.HostJsonPath.Replace("host.json", ""))
                       .AddJsonFile("host.json")
                       .Build();

            this.HostSettings = host;

            var version = this.HostSettings.GetSection("version").Value;
            this.HostJsonHttpSettings = string.IsNullOrWhiteSpace(version)
                                            ? host.Get<HttpSettings>("http")
                                            : (version.Equals("2.0", StringComparison.CurrentCultureIgnoreCase)
                                                   ? host.Get<HttpSettings>("extensions:http")
                                                   : host.Get<HttpSettings>("http"));
        }

        private void SetOpenApiInfo()
        {
            this.HostSettings.ThrowIfNullOrDefault();
            this.OpenApiSettingsJsonPath.ThrowIfNullOrWhiteSpace();
            this.LocalSettingsJsonPath.ThrowIfNullOrWhiteSpace();

            var openApiInfo = this.HostSettings.Get<OpenApiInfo>("openApi:info");
            if (this.IsValidOpenApiInfo(openApiInfo))
            {
                this.OpenApiInfo = openApiInfo;

                return;
            }

            if (File.Exists(this.OpenApiSettingsJsonPath))
            {
                var openapiSettings = File.ReadAllText(this.OpenApiSettingsJsonPath, Encoding.UTF8);
                openApiInfo = JsonConvert.DeserializeObject<OpenApiSettings>(openapiSettings).Info;
                if (this.IsValidOpenApiInfo(openApiInfo))
                {
                    this.OpenApiInfo = openApiInfo;

                    return;
                }
            }

            var localSettings = new ConfigurationBuilder()
                                .SetBasePath(this.LocalSettingsJsonPath.Replace("local.settings.json", ""))
                                .AddJsonFile("local.settings.json")
                                .Build();

            openApiInfo = new OpenApiInfo()
            {
                Version = localSettings.GetValue<string>("Values:OpenApi__Info__Version"),
                Title = localSettings.GetValue<string>("Values:OpenApi__Info__Title"),
                Description = localSettings.GetValue<string>("Values:OpenApi__Info__Description"),
                TermsOfService = new Uri(localSettings.GetValue<string>("Values:OpenApi__Info__TermsOfService")),
                Contact = new OpenApiContact()
                {
                    Name = localSettings.GetValue<string>("Values:OpenApi__Info__Contact__Name"),
                    Email = localSettings.GetValue<string>("Values:OpenApi__Info__Contact__Email"),
                    Url = new Uri(localSettings.GetValue<string>("Values:OpenApi__Info__Contact__Url")),
                },
                License = new OpenApiLicense()
                {
                    Name = localSettings.GetValue<string>("Values:OpenApi__Info__License__Name"),
                    Url = new Uri(localSettings.GetValue<string>("Values:OpenApi__Info__License__Url")),
                }
            };

            this.OpenApiInfo = openApiInfo;
        }

        private bool IsValidOpenApiInfo(OpenApiInfo openApiInfo)
        {
            openApiInfo.ThrowIfNullOrDefault();

            return !openApiInfo.IsNullOrDefault() && !openApiInfo.Version.IsNullOrDefault() && !openApiInfo.Title.IsNullOrWhiteSpace();
        }
    }
}
