using System.Linq;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Enums
{
    [TestClass]
    public class OpenApiVersionTypeTests
    {
        [DataTestMethod]
        [DataRow("V2")]
        [DataRow("V3")]
        public void Given_Enum_Should_Have_Member(string memberName)
        {
            var members = typeof(OpenApiVersionType).GetMembers().Select(p => p.Name);

            members.Should().Contain(memberName);
        }

        [DataTestMethod]
        [DataRow("V2", "v2")]
        [DataRow("V3", "v3")]
        public void Given_Enum_Should_Have_Decorator(string memberName, string displayName)
        {
            var member = this.GetMemberInfo(memberName);
            var attribute = member.GetCustomAttribute<DisplayAttribute>(inherit: false);

            attribute.Should().NotBeNull();
            attribute.Name.Should().Be(displayName);
        }

        private MemberInfo GetMemberInfo(string name)
        {
            var member = typeof(OpenApiVersionType).GetMember(name).First();

            return member;
        }
    }
}
