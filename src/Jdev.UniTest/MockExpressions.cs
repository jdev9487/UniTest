namespace Jdev.UniTest;

using Moq;
using System.Reflection;
using System.Linq.Expressions;

internal static class MockExpressions
{
    internal static Expression<Func<TInterface, TReturn>> CreateUnanimousItIsAny<TInterface, TReturn>(
        ParameterInfo[] parameters,
        string methodName)
    {
        var expressionArguments = parameters
            .Select(parameter => typeof(It).GetMethod(nameof(It.IsAny)).MakeGenericMethod(parameter.ParameterType))
            .Select(isAnyMethodInfo => Expression.Call(isAnyMethodInfo)).Cast<Expression>().ToList();
        
        var (body, param) = CreateMethodCall<TInterface>(expressionArguments, parameters, methodName);
        return Expression.Lambda<Func<TInterface, TReturn>>(body, param);
    }

    internal static Expression<Action<TInterface>> CreateOneArgumentItIsAny<TInterface>(ParameterInfo[] parameters,
        string methodName,
        object providedArgument)
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

        var (body, param) = CreateMethodCall<TInterface>(expressionArguments, parameters, methodName);
        return Expression.Lambda<Action<TInterface>>(body, param);
    }

    private static (MethodCallExpression, ParameterExpression) CreateMethodCall<TInterface>(
        IEnumerable<Expression>? expressionArguments,
        IEnumerable<ParameterInfo> parameters,
        string methodName)
    {
        var methodInfo =
            typeof(TInterface).GetMethod(methodName, parameters.Select(x => x.ParameterType).ToArray());
        var param = Expression.Parameter(typeof(TInterface));
        return (Expression.Call(param, methodInfo, expressionArguments), param);
    }
}