using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using System.Linq;
using System.Reflection;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Enums
{
    [TestClass]
    public class OpenApiSecuritySchemeTypeTests
    {
        [DataTestMethod]
        [DataRow("None")]
        [DataRow("Basic")]
        [DataRow("Bearer")]
        public void Given_Enum_Should_Have_Member(string memberName)
        {
            var members = typeof(OpenApiSecuritySchemeType).GetMembers().Select(p => p.Name);

            members.Should().Contain(memberName);
        }

        [DataTestMethod]
        //[DataRow("None", "")]
        [DataRow("Basic", "basic")]
        [DataRow("Bearer", "bearer")]
        public void Given_Enum_Should_Have_Decorator(string memberName, string displayName)
        {
            var member = this.GetMemberInfo(memberName);
            var attribute = member.GetCustomAttribute<DisplayAttribute>(inherit: false);

            attribute.Should().NotBeNull();
            attribute.Name.Should().Be(displayName);
        }

        private MemberInfo GetMemberInfo(string name)
        {
            var member = typeof(OpenApiSecuritySchemeType).GetMember(name).First();

            return member;
        }
    }
}
