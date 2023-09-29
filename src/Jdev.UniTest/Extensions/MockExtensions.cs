namespace Jdev.UniTest.Extensions;

using Models;
using Moq;

public static class MockExtensions
{
    public static MockMethodGroup<TInterface> WithMethod<TInterface>(
        this Mock<TInterface> mock, Func<TInterface, Delegate> func)
        where TInterface : class
    {
        return new()
        {
            Method = func(mock.Object),
            Mock = mock
        };
    }
}