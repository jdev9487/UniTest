namespace Jdev.UniTest.Extensions;

using System.Linq.Expressions;
using Moq;

public static class MockExtensions
{
    public static void Accepts<TInterface, TArgument1, TArgument2>(this Mock<TInterface> mock,
        Func<TInterface, Action<TArgument1, TArgument2>> func, TArgument1 providedArgument)
        where TInterface : class
        where TArgument1 : new()
    {
        var action = func(mock.Object);
        var parameters = action.Method.GetParameters();
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

        var doFooMethod = typeof(TInterface).GetMethod("DoFoo", new[] { typeof(Guid), typeof(int) });
        var param = Expression.Parameter(typeof(TInterface));
        var body = Expression.Call(param, doFooMethod, expressionArguments);
        var expression = Expression.Lambda<Action<TInterface>>(body, param);
        mock.Verify(expression, Times.Once);
    }
}