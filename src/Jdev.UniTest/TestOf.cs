namespace Jdev.UniTest;

using Exceptions;
using Moq;
using NUnit.Framework;

public abstract class TestOf<TClass> where TClass : class
{
    private readonly Dictionary<Type, Mock> _mocks;

    protected TClass Cut { get; }

    protected TestOf()
    {
        _mocks = new Dictionary<Type, Mock>();
        var constructorArguments = new List<object>();
        var constructors = typeof(TClass).GetConstructors();
        if (constructors.Length > 1)
            throw new AmbiguousConstructorException(typeof(TClass));
        var constructor = constructors.Single();
        var parameterInfos = constructor.GetParameters();
        foreach (var pi in parameterInfos)
        {
            if (pi.ParameterType.IsInterface)
            {
                var mockType = typeof(Mock<>).MakeGenericType(pi.ParameterType);
                var mock = (Mock?)Activator.CreateInstance(mockType);
                if (mock is null)
                    throw new MockActivationException(pi.ParameterType);
                constructorArguments.Add(mock.Object);
                _mocks.Add(pi.ParameterType, mock);
            }
            else
                throw new ConstructionException(pi.ParameterType);
        }

        Cut = (TClass)constructor.Invoke(constructorArguments.ToArray());
    }

    protected Mock<TDependency> MockOf<TDependency>() where TDependency : class =>
        (Mock<TDependency>)_mocks[typeof(TDependency)];

    [SetUp]
    protected abstract void SetUp();
}