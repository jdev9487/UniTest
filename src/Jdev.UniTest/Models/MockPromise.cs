namespace Jdev.UniTest.Models;

using Moq;

public class MockPromise<TInterface> where TInterface : class
{
    public Mock<TInterface> Mock { get; init; } = default!;
    public Delegate Promise = default!;
}