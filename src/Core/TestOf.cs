namespace Core;

using Moq;

public class TestOf<TClass> where TClass : class
{
    private readonly Dictionary<Type, Mock> _mocks;
    
    public TClass Cut { get; }
    
    public TestOf()
    {
        _mocks = new Dictionary<Type, Mock>();
        var constructorArguments = new List<object>();
        var constructor = typeof(TClass).GetConstructors()[0];
        var parameterInfos = constructor.GetParameters();
        foreach (var pi in parameterInfos)
        {
            if (pi.ParameterType.IsInterface)
            {
                var mockType = typeof(Mock<>).MakeGenericType(pi.ParameterType);
                var mock = (Mock)Activator.CreateInstance(mockType);
                constructorArguments.Add(mock.Object);
                _mocks.Add(pi.ParameterType, mock);
            }
        }

        Cut = (TClass)constructor.Invoke(constructorArguments.ToArray());
    }

    public Mock<TDependency> MockOf<TDependency>() where TDependency : class =>
        (Mock<TDependency>)_mocks[typeof(TDependency)];
}