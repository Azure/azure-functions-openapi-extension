using System.Diagnostics;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Extension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Tests.Extension
{
    [TestClass]
    public class ProjectPathExtensionsTests
    {
        private string _compiledPath;
        private string _configuration;
        private bool _isDebug;
        private string _projectPath;
        private string _target;

        [TestInitialize]
        public void Init()
        {
            this.IsDebugCheck(ref this._isDebug);

            var directory = Assembly.GetExecutingAssembly().Location;
            var solutionDirectory = Directory.GetParent(directory).Parent.Parent.Parent.Parent.Parent.FullName;
            this._projectPath = $"{solutionDirectory}/samples/Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.V3Net5";
            this._configuration = this._isDebug ? "Debug" : "Release";
            this._target = "net6.0";
            this._compiledPath = $"{this._projectPath}{ProjectPathExtensions.DirectorySeparator}bin{ProjectPathExtensions.DirectorySeparator}{this._configuration}{ProjectPathExtensions.DirectorySeparator}{this._target}";
        }

        [Conditional("DEBUG")]
        private void IsDebugCheck(ref bool isDebug)
        {
            isDebug = true;
        }

        [TestMethod]
        public void TrimProjectPath()
        {
            // Arrange
            var projectPath = $"{this._projectPath}{ProjectPathExtensions.DirectorySeparator}";

            // Act
            var result = projectPath.TrimProjectPath();

            // Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void GetCsProjFileName()
        {
            // Arrange
            var projectPath = this._projectPath;

            // Act
            var result = projectPath.GetCsProjFileName();

            // Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void GetProjectDllFileName()
        {
            // Arrange
            var csprojFileName = "Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.V3Net5.csproj";

            // Act
            var result = this._projectPath.GetProjectDllFileName(csprojFileName);

            // Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void GetProjectCompiledPath()
        {
            // Arrange
            var projectPath = this._projectPath;

            // Act
            var result = projectPath.GetProjectCompiledPath(this._configuration, this._target);

            // Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void GetProjectCompiledDllPath()
        {
            // Arrange
            var dllFileName = "Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.V3Net5.dll";

            // Act
            var result = this._compiledPath.GetProjectCompiledDllPath(dllFileName);

            // Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void GetProjectHostJsonPath()
        {
            // Arrange
            var projectPath = this._projectPath;

            // Act
            var result = this._compiledPath.GetProjectHostJsonPath();

            // Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void GetOutputPath()
        {
            // Arrange
            var output = "output";

            // Act
            var result = output.GetOutputPath(this._compiledPath);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
