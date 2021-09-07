using System;
using System.Reflection;
using System.Reflection.Emit;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Extensions
{
    [TestClass]
    public class AssemblyExtensionsTests
    {
        [TestMethod]
        public void Given_Assembly_With_Unloadable_Types_When_GetLoadableTypes_Invoked_Then_It_Should_Return_Loadable_Types()
        {
            // Arrange
            var assemblyName = new AssemblyName("AssemblyWithUnloadableTypes");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
            BuildType(moduleBuilder, "UnloadableType", false);
            BuildType(moduleBuilder, "LoadableType", true);
            var assembly = (Assembly)assemblyBuilder;

            // Act
            var types = assembly.GetLoadableTypes();

            // Assert
            types.Should().HaveCount(1);
            types[0].Name.Should().Be("LoadableType");
        }

        private static void BuildType(ModuleBuilder moduleBuilder, string name, bool isLoadable)
        {
            if (isLoadable)
            {
                moduleBuilder.DefineType(name, TypeAttributes.Public).CreateType();
            }
            else
            {
                // We define a type that has an interface declared but interface method is not implemented.
                moduleBuilder.DefineType(name, TypeAttributes.Public, null, new[] { typeof(IDisposable) });
            }
        }
    }
}
