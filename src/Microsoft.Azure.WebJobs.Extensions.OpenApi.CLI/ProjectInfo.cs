using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configuration.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

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

                return;
            }

            var fqpath = System.IO.Path.IsPathFullyQualified(path)
                ? path
                : $"{Environment.CurrentDirectory.TrimEnd(directorySeparator)}{directorySeparator}{path}";

            if (fqpath.EndsWith(".csproj"))
            {
                var csproj = new FileInfo(fqpath);

                this._projectPath = csproj.DirectoryName.TrimEnd(directorySeparator);
                this._filename = csproj.Name;

                return;
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
            var assembly = Assembly.LoadFrom(this.CompiledDllPath);

            var type = assembly.GetTypes()
                               .SingleOrDefault(p => p.GetInterface("IOpenApiConfigurationOptions", ignoreCase: true).IsNullOrDefault() == false);
            if (type.IsNullOrDefault())
            {
                var settings = new DefaultOpenApiConfigurationOptions();
                this.OpenApiInfo = settings.Info;

                return;
            }

            var options = Activator.CreateInstance(type);

            this.OpenApiInfo = (options as IOpenApiConfigurationOptions).Info;
        }

        private bool IsValidOpenApiInfo(OpenApiInfo openApiInfo)
        {
            openApiInfo.ThrowIfNullOrDefault();

            return !openApiInfo.IsNullOrDefault() && !openApiInfo.Version.IsNullOrDefault() && !openApiInfo.Title.IsNullOrWhiteSpace();
        }
    }
}
