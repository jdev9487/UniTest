namespace Jdev.UniTest.Exceptions;

public class ConstructionException : Exception
{
    private readonly Type _type;
    
    public ConstructionException(Type type) => _type = type;

    public override string Message => $"Type {_type} is not an interface and cannot be mocked";
}