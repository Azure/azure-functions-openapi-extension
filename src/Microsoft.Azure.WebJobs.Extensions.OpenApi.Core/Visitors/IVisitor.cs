using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// This provides interfaces to visitor classes.
    /// </summary>
    public interface IVisitor
    {
        /// <summary>
        /// Checks whether the type is visitable or not.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>Returns <c>True</c>, if the type is visitable; otherwise returns <c>False</c>.</returns>
        bool IsVisitable(Type type);

        /// <summary>
        /// Visits and process the acceptor.
        /// </summary>
        /// <param name="acceptor"><see cref="IAcceptor"/> instance.</param>
        /// <param name="type">Type to check.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance.</param>
        /// <param name="attributes">List of attribute instances.</param>
        void Visit(IAcceptor acceptor, KeyValuePair<string, Type> type, NamingStrategy namingStrategy, params Attribute[] attributes);

        /// <summary>
        /// Checks whether the type is navigatable to its sub-type or not.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>Returns <c>True</c>, if the type is navigatable; otherwise returns <c>False</c>.</returns>
        bool IsNavigatable(Type type);

        /// <summary>
        /// Checks whether the type is visitable or not for parameters.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>Returns <c>True</c>, if the type is visitable for parameters; otherwise returns <c>False</c>.</returns>
        bool IsParameterVisitable(Type type);

        /// <summary>
        /// Visits and process the type for parameters.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance.</param>
        OpenApiSchema ParameterVisit(Type type, NamingStrategy namingStrategy);

        /// <summary>
        /// Checks whether the type is visitable or not for payloads.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>Returns <c>True</c>, if the type is visitable for payloads; otherwise returns <c>False</c>.</returns>
        bool IsPayloadVisitable(Type type);

        /// <summary>
        /// Visits and process the type for payloads.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance.</param>
        OpenApiSchema PayloadVisit(Type type, NamingStrategy namingStrategy);
    }
}
