namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    /// <summary>
    /// This provides interfaces to the <see cref="FakeClass"/> class.
    /// </summary>
    public interface IFakeInterface
    {
        /// <summary>
        /// Does something.
        /// </summary>
        /// <param name="input">Input value.</param>
        /// <returns>Output value.</returns>
        bool DoSomething(bool input);

        /// <summary>
        /// Does other thing.
        /// </summary>
        /// <param name="input">Input value.</param>
        /// <returns>Output value.</returns>
        bool DoOtherThing(bool input);
    }
}
