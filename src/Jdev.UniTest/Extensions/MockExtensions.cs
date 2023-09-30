namespace Jdev.UniTest.Extensions;

using Moq;
using AutoFixture;

internal static class MockExtensions
{
    internal static TReturn GenericSetUp<TInterface, TReturn>(this Mock<TInterface> mock, Delegate func)
        where TInterface : class
    {
        var parameters = func.Method.GetParameters();
        var expression = MockExpressions.CreateUnanimousItIsAny<TInterface, TReturn>(parameters, func.Method.Name);
        var returnObject = new Fixture().Create<TReturn>();
        mock.Setup(expression)
            .Returns(returnObject);
        return returnObject;
    }
}