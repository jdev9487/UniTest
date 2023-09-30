namespace Jdev.UniTest.Models;

using Moq;

public class MockPromise<TInterface> where TInterface : class
{
    public Mock<TInterface> Mock { get; init; } = default!;
    public Delegate Promise = default!;
    
    public Verification IsGiven(object providedArgument)
    {
        var action = Promise;
        var parameters = action.Method.GetParameters();
        var expression =
            MockExpressions.CreateOneArgumentItIsAny<TInterface>(parameters, action.Method.Name, providedArgument);
        return new Verification
        {
            Action = () => Mock.Verify(expression, Times.Once)
        };
    }
}