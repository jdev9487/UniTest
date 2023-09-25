namespace Jdev.UniTest;

using Moq;
using Exceptions;
using NUnit.Framework;

public abstract class TestOf<TClass> where TClass : class
{
    private readonly Dictionary<Type, object?> _dependencies;

    /// <summary>
    /// Class Under Test
    /// </summary>
    protected TClass Cut { get; }

    protected TestOf()
    {
        _dependencies = new Dictionary<Type, object?>();
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
                _dependencies.Add(pi.ParameterType, mock);
            }
            else
                throw new ConstructionException(pi.ParameterType);
        }

        Cut = (TClass)constructor.Invoke(constructorArguments.ToArray());
    }

    protected Mock<TDependency> MockOf<TDependency>() where TDependency : class
    {
        if (_dependencies.TryGetValue(typeof(TDependency), out var value) && value is Mock<TDependency> mock)
        {
            return mock;
        }

        throw new MockNotFoundException(typeof(Mock<TDependency>));
    }

    [SetUp]
    protected abstract void SetUp();

    [TearDown]
    protected virtual void TearDown()
    {
        foreach (var key in _dependencies.Keys
                     .Where(key => _dependencies[key] is IDisposable)
                     .Where(key => _dependencies[key] is not null))
        {
            ((IDisposable)_dependencies[key]).Dispose();
        }

        foreach (var key in _dependencies.Keys)
        {
            _dependencies[key] = null!;
        }
    }
}