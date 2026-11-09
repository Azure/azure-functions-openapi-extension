using System;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NSubstitute;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Extensions
{
    [TestClass]
    public class OpenApiHttpTriggerContextExtensionsTests
    {
        [TestMethod]
        public void Given_Null_Parameter_When_AuthorizeAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var context = Substitute.For<IOpenApiHttpTriggerContext>();

            Func<Task> func = async () => await OpenApiHttpTriggerContextExtensions.AuthorizeAsync(null, null).ConfigureAwait(false);
            func.Should().ThrowAsync<ArgumentNullException>();

            func = async () => await OpenApiHttpTriggerContextExtensions.AuthorizeAsync(Task.FromResult(context), null).ConfigureAwait(false);
            func.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        public async Task Given_Parameter_When_AuthorizeAsync_Invoked_Then_It_Should_Return_Result()
        {
            var context = Substitute.For<IOpenApiHttpTriggerContext>();
            context.AuthorizeAsync(Arg.Any<IHttpRequestDataObject>()).Returns(Task.FromResult(default(OpenApiAuthorizationResult)));

            var req = Substitute.For<IHttpRequestDataObject>();

            var result = await OpenApiHttpTriggerContextExtensions.AuthorizeAsync(Task.FromResult(context), req).ConfigureAwait(false);

            result.Should().BeNull();
        }
    }
}
