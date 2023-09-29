namespace Jdev.UniTest.Models;

using Moq;

public class MockMethodGroup<TInterface>
    where TInterface : class
{
    public Mock<TInterface> Mock { get; init; } = default!;
    public Delegate Method { get; init; } = default!;
}