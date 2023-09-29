namespace Jdev.UniTest.Extensions;

using Moq;
using Models;
using System.Reflection;
using System.Linq.Expressions;
using AutoFixture;

public static class MockPromiseExtensions
{
    public static void IsGiven<TInterface>(this MockPromise<TInterface> mockPromise, object providedArgument)
        where TInterface : class
    {
        var action = mockPromise.Promise;
        var parameters = action.Method.GetParameters();
        var expression = GetExpression<TInterface>(parameters, action.Method.Name, providedArgument);
        mockPromise.Mock.Verify(expression, Times.Once);
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

    public static TReturn GenericSetUp<TInterface, TReturn>(this Mock<TInterface> mock, Delegate func)
        where TInterface : class
    {
        var parameters = func.Method.GetParameters();
        var expression = GetExpression<TInterface, TReturn>(parameters, func.Method.Name);
        var returnObject = new Fixture().Create<TReturn>();
        mock.Setup(expression)
            .Returns(returnObject);
        return returnObject;
    }

    private static Expression<Func<TInterface, TReturn>> GetExpression<TInterface, TReturn>(ParameterInfo[] parameters, string methodName)
    {
        var expressionArguments = parameters
            .Select(parameter => typeof(It).GetMethod(nameof(It.IsAny)).MakeGenericMethod(parameter.ParameterType))
            .Select(isAnyMethodInfo => Expression.Call(isAnyMethodInfo)).Cast<Expression>().ToList();
        var methodInfo =
            typeof(TInterface).GetMethod(methodName, parameters.Select(x => x.ParameterType).ToArray());
        var param = Expression.Parameter(typeof(TInterface));
        var body = Expression.Call(param, methodInfo, expressionArguments);
        return Expression.Lambda<Func<TInterface, TReturn>>(body, param);
    }
}