namespace Jdev.UniTest.Extensions;

using Moq;
using AutoFixture;
using System.Reflection;
using System.Linq.Expressions;

public static class MockExtensions
{
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