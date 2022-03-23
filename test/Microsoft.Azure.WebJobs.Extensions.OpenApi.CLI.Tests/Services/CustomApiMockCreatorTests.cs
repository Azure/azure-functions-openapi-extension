using System.Diagnostics;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Tests.Services
{
    [TestClass]
    public class CustomApiMockCreatorTests
    {
        private bool _isDebug;

        [TestInitialize]
        public void Init()
        {
            this.IsDebugCheck(ref this._isDebug);
        }

        [Conditional("DEBUG")]
        private void IsDebugCheck(ref bool isDebug)
        {
            isDebug = true;
        }

        [TestMethod]
        public void SetupApi()
        {
            // Arrange
            var directory = Assembly.GetExecutingAssembly().Location;
            var solutionDirectory = Directory.GetParent(directory).Parent.Parent.Parent.Parent.Parent.FullName;
            var projectPath = $"{solutionDirectory}/samples/Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.OutOfProc";
            var configuration = this._isDebug ? "Debug" : "Release";
            var target = "net6.0";

            var service = this.SetupSut();

            // Act
            var result = service.SetupApi(
                projectPath,
                configuration,
                target);

            // Assert
            result.Should().NotBeNull();
        }

        private CustomApiMockCreator SetupSut()
        {
            var service = new CustomApiMockCreator();
            return service;
        }
    }
}
