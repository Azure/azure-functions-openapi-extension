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
        private OpenApiSecurityRequirementComparer comparer = new OpenApiSecurityRequirementComparer();

        [TestMethod]
        public void Given_DifferentRequirementId_When_Call_Equals_Should_Be_False()
        {
            var requirement1 = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "securitySchemeId",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string> { "scope1", "scope2" }
                }
            };
            var requirement2 = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "securitySchemeId2",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string> { "string1", "string2" }
                }
            };
            comparer.Equals(requirement1, requirement2).Should().Be(false);
        }

        [TestMethod]
        public void Given_EqualRequirementId_When_Call_Equals_Should_Be_True()
        {
            var requirement1 = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "securitySchemeId",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string> { "string1", "string2" }
                }
            };
            var requirement2 = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "securitySchemeId",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string> { "string1", "string2" }
                }
            };

            comparer.Equals(requirement1, requirement2).Should().Be(true);
        }

        [TestMethod]
        public void Given_EqualRequirements_Should_Be_Return_SameHashCode()
        {
            //Given
            var requirement1 = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "securitySchemeId",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string> { "string1", "string2" }
                }
            };
            var requirement2 = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "securitySchemeId",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string> { "string1", "string2" }
                }
            };
            //When
            var hashCode1 = comparer.GetHashCode(requirement1);
            var hashCode2 = comparer.GetHashCode(requirement2);

            bool result = hashCode1.Equals(hashCode2);

            //Then
            result.Should().Be(true);
        }
    }
}
