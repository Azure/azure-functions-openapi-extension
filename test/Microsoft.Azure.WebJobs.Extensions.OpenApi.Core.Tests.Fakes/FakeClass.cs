namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    /// <summary>
    /// This represents the fake class entity.
    /// </summary>
    public class FakeClass : IFakeInterface
    {
        /// <inheritdoc />
        [FakeMethod]
        public bool DoSomething(bool input)
        {
            return input;
        }

        /// <inheritdoc />
        public bool DoOtherThing(bool input)
        {
            return input;
        }
    }
}
