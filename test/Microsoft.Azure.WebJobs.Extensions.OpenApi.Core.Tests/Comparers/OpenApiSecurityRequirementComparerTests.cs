using System.Dynamic;
using System.ComponentModel;
using System;

using System.Collections.Generic;
using System.Linq;

using Microsoft.OpenApi.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Comparers;
using FluentAssertions;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Comparers
{
    [TestClass]
    public class OpenApiSecurityRequirementComparerTests
    {
        private OpenApiSecurityRequirementComparer _comparer;

        [TestInitialize]
        public void Init()
        {
            this._comparer = new OpenApiSecurityRequirementComparer();
        }

        [DataTestMethod]
        [DataRow("equalReferenceId", "equalReferenceId", true)]
        [DataRow("ReferenceId", "differentReferenceId",  false)]
        public void Given_OpenApiSecurityRequirements_When_Equals_Invoked_Then_It_Should_Return_Result(string referenceId1, string referenceId2, bool expected)
        {
            var requirement1 = InitializeRequirement(referenceId1);
            var requirement2 = InitializeRequirement(referenceId2);

            bool result = _comparer.Equals(requirement1, requirement2);

            result.Should().Be(expected);
        }


        [DataTestMethod]
        [DataRow("equalReferenceId", "equalReferenceId", true)]
        [DataRow("ReferenceId", "differentReferenceId",  false)]
        public void Given_SameOpenApiSecurityRequirement_Should_Return_SameHashCode(string referenceId1, string referenceId2, bool expected)
        {
            var requirement1 = InitializeRequirement(referenceId1);
            var requirement2 = InitializeRequirement(referenceId2);

            var hashCode1 = _comparer.GetHashCode(requirement1);
            var hashCode2 = _comparer.GetHashCode(requirement2);

            var result = hashCode1.Equals(hashCode2);

            result.Should().Be(expected);
        }

        [TestMethod]
        public void Given_NullRequirement_When_GetHashCode_Invoked_Then_It_Should_Return_Zero()
        {
            OpenApiSecurityRequirement requirement = null;

            var hashCode = _comparer.GetHashCode(requirement);

            hashCode.Should().Be(0);
        }

        private OpenApiSecurityRequirement InitializeRequirement(string referenceId)
        {
            var reference = new OpenApiReference { Id = referenceId };
            var scheme = new OpenApiSecurityScheme { Reference = reference };
            return new OpenApiSecurityRequirement { { scheme, null } };
        }
    }
}
