namespace Jdev.UniTest.Extensions;

using Moq;
using System.Linq.Expressions;
using System.Reflection;
using Models;

public static class MockMethodGroupExtensions
{
    public static void IsGiven<TInterface>(
        this MockMethodGroup<TInterface> mockMethodGroup, object providedArgument)
        where TInterface : class
    {
        var action = mockMethodGroup.Method;
        var parameters = action.Method.GetParameters();
        var expression = GetExpression<TInterface>(parameters, action.Method.Name, providedArgument);
        mockMethodGroup.Mock.Verify(expression, Times.Once);
    }

    private static Expression<Action<TInterface>> GetExpression<TInterface>(ParameterInfo[] parameters,
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
