using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors
{
    /// <summary>
    /// This represents the collection entity for <see cref="IVisitor"/> instances.
    /// </summary>
    public class VisitorCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisitorCollection"/> class.
        /// </summary>
        public VisitorCollection()
        {
            this.Visitors = new List<IVisitor>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisitorCollection"/> class.
        /// </summary>
        /// <param name="visitors">List of <see cref="IVisitor"/> instances.</param>
        public VisitorCollection(List<IVisitor> visitors)
        {
            this.Visitors = visitors.ThrowIfNullOrDefault();
        }

        /// <summary>
        /// Gets the list of <see cref="IVisitor"/> instances.
        /// </summary>
        public List<IVisitor> Visitors { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="VisitorCollection"/> class by scanning the current assembly for <see cref="IVisitor"/>.
        /// </summary>
        /// <remarks>
        /// There is an expectation that the <see cref="IVisitor"/> implementation has a constructor that takes a <see cref="VisitorCollection"/> as the only parameter.
        /// </remarks>
        /// <returns>Returns the <see cref="VisitorCollection"/> instance.</returns>
        public static VisitorCollection CreateInstance()
        {
            var collection = new VisitorCollection();
            collection.Visitors = typeof(IVisitor).Assembly
                                           .GetLoadableTypes()
                                           .Where(p => p.HasInterface<IVisitor>() && p.IsClass && !p.IsAbstract)
                                           .Select(p => (IVisitor)Activator.CreateInstance(p, collection)).ToList(); // NOTE: there is no direct enforcement on the constructor arguments of the visitors
            return collection;
        }

        /// <summary>
        /// Processes the parameter type.
        /// </summary>
        /// <param name="type">Type of the parameter.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance.</param>
        /// <returns>Returns <see cref="OpenApiSchema"/> instance.</returns>
        public OpenApiSchema ParameterVisit(Type type, NamingStrategy namingStrategy)
        {
            var schema = default(OpenApiSchema);
            foreach (var visitor in this.Visitors)
            {
                if (!visitor.IsParameterVisitable(type))
                {
                    continue;
                }

                schema = visitor.ParameterVisit(type, namingStrategy);
                break;
            }

            return schema;
        }

        /// <summary>
        /// Processes the request/response payload type.
        /// </summary>
        /// <param name="type">Type of the payload.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance.</param>
        /// <param name="useFullName">Value indicating whether to use Fullname or not</param>
        /// <returns>Returns <see cref="OpenApiSchema"/> instance.</returns>
        public OpenApiSchema PayloadVisit(Type type, NamingStrategy namingStrategy, bool useFullName = false)
        {
            var schema = default(OpenApiSchema);
            foreach (var visitor in this.Visitors)
            {
                if (!visitor.IsPayloadVisitable(type))
                {
                    continue;
                }

                schema = visitor.PayloadVisit(type, namingStrategy,useFullName);
                break;
            }

            return schema;
        }
    }
}
