namespace Jdev.UniTest.Models;

using Moq;
using System.Reflection;
using System.Linq.Expressions;

public class MockPromise<TInterface> where TInterface : class
{
    public Mock<TInterface> Mock { get; init; } = default!;
    public Delegate Promise = default!;
    
    public Verification IsGiven(object providedArgument)
    {
        var action = Promise;
        var parameters = action.Method.GetParameters();
        var expression = GetExpression(parameters, action.Method.Name, providedArgument);
        return new Verification
        {
            Action = () => Mock.Verify(expression, Times.Once)
        };
    }

    private static Expression<Action<TInterface>> GetExpression(ParameterInfo[] parameters,
        string methodName, object providedArgument)
    {
        var expressionArguments = new List<Expression>();
        foreach (var parameter in parameters)
        {
            if (providedArgument.GetType() == parameter.ParameterType)
            {
                expressionArguments.Add(Expression.Constant(providedArgument));
            }
            else
            {
                var isAnyMethodInfo = typeof(It).GetMethod(nameof(It.IsAny)).MakeGenericMethod(parameter.ParameterType);
                var isAnyArg = Expression.Call(isAnyMethodInfo);
                expressionArguments.Add(isAnyArg);
            }
        }

        var methodInfo =
            typeof(TInterface).GetMethod(methodName, parameters.Select(x => x.ParameterType).ToArray());
        var param = Expression.Parameter(typeof(TInterface));
        var body = Expression.Call(param, methodInfo, expressionArguments);
        return Expression.Lambda<Action<TInterface>>(body, param);
    }
}